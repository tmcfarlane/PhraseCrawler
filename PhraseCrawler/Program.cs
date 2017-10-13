using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhraseCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            PhraseParser parser = new PhraseParser();
            parser.FindPhrase("https://reddit.com/all", "html");
        }
    }
}
