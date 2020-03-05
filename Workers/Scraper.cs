using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Data; // Allows use of ScrapeCriteria class
using System.Text.RegularExpressions; //Allows use of Regex functionality

namespace WebScrapper.Workers
{
    class Scraper
    {
        public List<string> Scrape(ScrapeCriteria scrapeCriteria) // Public method (function) that is passed a ScrapeCriteria type argument and return a list of strings
        {
            List<string> scrapedElements = new List<string>();

            MatchCollection matches = Regex.Matches(scrapeCriteria.Data, scrapeCriteria.Regex, scrapeCriteria.RegexOption); //stores all sucessful regex matches returned from a match comparasion function from Regex library into a variable named matches

            foreach (Match match in matches)
            {
                if(!scrapeCriteria.Parts.Any()) //if there is NONE regex and regexOption specified return and add the all thing to our list of scrappedElements
                {
                    scrapedElements.Add(match.Groups[0].Value);
                }
                else
                {
                    foreach (var part in scrapeCriteria.Parts)
                    {
                        Match matchedPart = Regex.Match(match.Groups[0].Value, part.Regex, part.RegexOption);

                        if (matchedPart.Success) scrapedElements.Add(matchedPart.Groups[1].Value);
                    }
                }
            }

            return scrapedElements;
        }
    }
}
