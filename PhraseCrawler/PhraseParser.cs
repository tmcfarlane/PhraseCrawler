using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace PhraseCrawler
{
    public class PhraseParser
    {
        private List<string> _pagesToVisit = new List<string>();
        private HashSet<string> _visitedPages = new HashSet<string>();


        // get links
        public IEnumerable<string> GetLinks(string url, List<string> urls = null)
        {
            if (url == null) return urls;
            if (urls == null) urls = new List<string>();

            Uri t = new Uri(url);
            string domain = t.Host;
            
            var web = new HtmlWeb();
            var doc = web.Load(url);
            foreach (var node in doc.DocumentNode.SelectNodes("//a[@href]").Where(x => x.GetAttributeValue("href", "").Contains(domain)))        //xpath expression https://www.w3schools.com/xml/xpath_syntax.asp
            {
                urls.Add(node.GetAttributeValue("href", ""));
            }
            return urls;
        }

        // spider
        public Dictionary<string, int> FindPhrase(string url, string phrase, int depth = 3)
        {
            if (phrase == null) return null;

            Dictionary<string, int> results = new Dictionary<string, int>();

            _pagesToVisit.Add(url);
            
            while (_pagesToVisit.Count != 0 && depth > 0)
            {
                var links = this.GetLinks(url);
                foreach (var foundLink in links)
                {
                    if (!_visitedPages.Contains(foundLink) && !_pagesToVisit.Contains(foundLink))
                    {
                        _pagesToVisit.Add(foundLink);
                    }
                }
                var link = _pagesToVisit.First();
                _pagesToVisit.RemoveAt(0);
                _visitedPages.Add(link);
                depth--;
                var web = new HtmlWeb();
                var doc = web.Load(link);

                var phrases = doc.DocumentNode.SelectNodes(string.Format("//*[text()[contains(., '{0}')]]", phrase));
                try
                {
                    Console.WriteLine("Adding: {0} with Count: {1}", link, phrases.Count);
                    results.Add(link, phrases.Count);
                }
                catch (ArgumentException e)
                {
                    //do something useful here
                }
            }
            return results;
        }
    }
}
