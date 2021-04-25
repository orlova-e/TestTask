using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestTask.App.Interfaces;
using TestTask.Domain;
using TestTask.Repo.Interfaces;

namespace TestTask.App.Implementation
{
    public class CrawlerService : ICrawlerService
    {
        private IRepository<Crawled> _repository { get; }
        private IWebService _webService { get; }
        private ILogger<CrawlerService> _logger { get; }

        public CrawlerService(IRepository<Crawled> repository,
            IWebService webService,
            ILogger<CrawlerService> logger)
        {
            _repository = repository;
            _webService = webService;
            _logger = logger;
        }

        public Guid StartProcessing(string url)
        {
            Guid id = Guid.NewGuid();
            Crawled crawled = new()
            {
                Url = url,
                Status = ProcessStatus.Processing,
                Result = -1
            };

            _repository.SetOrUpdateEntry(id, crawled);
            BackgroundCount(id, crawled);

            return id;
        }

        public ProcessStatus GetStatus(Guid id)
        {
            if (!_repository.HasEntry(id))
            {
                return ProcessStatus.WithAnError;
            }

            Crawled crawled = _repository.GetEntry(id);
            return crawled.Status;
        }

        public int GetResult(Guid id) => _repository.GetEntry(id)?.Result ?? -1;

        private void BackgroundCount(Guid id, Crawled crawled)
            => Task.Run(async () =>
            {
                if (await _webService.TryGetPageAsync(crawled.Url))
                {
                    try
                    {
                        string text = _webService.GetText();

                        text = Regex.Replace(text, @"([^\w-][^A-Za-zА-Яа-я]+)|[\d]|(- )", " ");

                        crawled.Result = text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)?.Length ?? default;
                        crawled.Status = ProcessStatus.Successfully;
                    }
                    catch (Exception)
                    {
                        _logger.LogError($"Cannot parse a html document with id [{id}]."
                            + Environment.NewLine
                            + $"The resource was on the [{crawled.Url}]");

                        crawled.Status = ProcessStatus.WithAnError;
                        crawled.Result = -1;
                    }
                }
                else
                {
                    crawled.Status = ProcessStatus.WithAnError;
                    crawled.Result = -1;
                }
            });
    }
}
