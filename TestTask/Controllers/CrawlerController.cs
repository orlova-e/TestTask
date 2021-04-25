using Microsoft.AspNetCore.Mvc;
using System;
using TestTask.App.Interfaces;
using TestTask.Domain;
using TestTask.Repo.Interfaces;

namespace TestTask.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CrawlerController : ControllerBase
    {
        private ICrawlerService _crawlerService { get; }
        private IRepository<Crawled> _repository { get; }

        public CrawlerController(ICrawlerService crawlerService,
            IRepository<Crawled> repository)
        {
            _crawlerService = crawlerService;
            _repository = repository;
        }

        [HttpPost("[action]")]
        public IActionResult Crawl([FromBody] string url)
        {
            Guid id = _crawlerService.StartProcessing(url);
            return Ok(new { RequestId = id });
        }

        [HttpGet("[action]/{id:guid}")]
        public IActionResult Status([FromRoute] Guid id)
        {
            if (!_repository.HasEntry(id))
            {
                return NotFound(id);
            }

            ProcessStatus status = _crawlerService.GetStatus(id);
            return Ok(new { Status = (int)status });
        }

        [HttpGet("[action]/{id:guid}")]
        public IActionResult Result([FromRoute] Guid id)
        {
            Crawled crawled = _repository.GetEntry(id);

            if (crawled is null || crawled.Status is not ProcessStatus.Successfully)
            {
                return NotFound(id);
            }

            return Ok(new { WordCount = crawled.Result });
        }
    }
}
