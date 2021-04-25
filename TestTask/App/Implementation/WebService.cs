using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TestTask.App.Interfaces;

namespace TestTask.App.Implementation
{
    public class WebService : IWebService
    {
        private ILogger<WebService> _logger { get; }
        private HtmlDocument _document;

        public WebService(ILogger<WebService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> TryGetPageAsync(string url)
        {
            HttpClient httpClient = new();
            _document = new();

            try
            {
                using HttpResponseMessage response = await httpClient.GetAsync(url);
                using HttpContent content = response.Content;
                string result = await content.ReadAsStringAsync();
                _document.LoadHtml(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to load a resource at [{url}]"
                    + Environment.NewLine
                    + ex.Message);

                return false;
            }

            return _document.DocumentNode?.HasChildNodes ?? false;
        }

        public string GetText()
        {
            Queue<HtmlNode> nodes = new(_document.DocumentNode.SelectNodes("./*|./text()"));

            while (nodes.Count > 0)
            {
                HtmlNode node = nodes.Dequeue();
                HtmlNode parentNode = node.ParentNode;

                if (node.Name is not "#text")
                {
                    HtmlNodeCollection childNodes = node.SelectNodes("./*|./text()");

                    if (childNodes is not null)
                    {
                        foreach (HtmlNode child in childNodes)
                        {
                            nodes.Enqueue(child);
                            parentNode.InsertBefore(child, node);
                        }
                    }

                    parentNode.RemoveChild(node);
                }
            }

            return _document.DocumentNode.InnerHtml ?? string.Empty;
        }
    }
}
