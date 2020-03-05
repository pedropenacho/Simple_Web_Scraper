﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net; //allows use of Webclient and web functionality
using System.Text.RegularExpressions; //Allows use of Regex
using WebScrapper.Data; //allows use of ScrapeCriteria
using WebScrapper.Builders; //allows use of ScrapeCriteriaBuilders
using WebScrapper.Workers; //Allows use of Scraper
using System.IO;

namespace WebScraper
{
    class Program
    {
        private const string method = "search";
        
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Please enter city from which you would like to scrape information from: ");
                string craigslistCity = Console.ReadLine().ToLower() ?? string.Empty;

                Console.WriteLine("Please enter category from which you would like to scrape information from: ");
                string craigslistCategory = Console.ReadLine().ToLower() ?? string.Empty;

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("RESULTS:");
                Console.WriteLine("----------------------------------");
                using (WebClient client = new WebClient())
                {
                    string content = client.DownloadString($"https://{craigslistCity.Replace(" ", string.Empty)}.craigslist.org/{method}/sss?query={craigslistCategory}&sort=rel");
               
                    ScrapeCriteria scrapeCriteria = new ScrapeCriteriaBuilder()
                    .WithData(content)
                    .WithRegex(@"<a href=\""(.*?)\"" data-id=\""(.*?)\"" class=\""result-title hdrlnk\"">(.*?)</a>")
                    .WithRegexOption(RegexOptions.ExplicitCapture)
                    .WithPart(new ScrapeCriteriaPartBuilder()
                        .WithRegex(@">(.*?)</a>")
                        .WithRegexOption(RegexOptions.Singleline)
                        .Build())
                    .WithPart(new ScrapeCriteriaPartBuilder()
                        .WithRegex(@"href=\""(.*?)\""")
                        .WithRegexOption(RegexOptions.Singleline)
                        .Build())
                    .Build();

                    Scraper scraper = new Scraper();

                    var scrapedElements = scraper.Scrape(scrapeCriteria);

                    if (scrapedElements.Any())
                    {
                        foreach (var scrapedElement in scrapedElements)
                        {
                            Console.WriteLine(scrapedElement);
                        }
                    }
                    else
                    {
                        Console.WriteLine("There is no matches for the specified scrape user inputs.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }    
        }
    }
}

#region "Error Handling ("Try...Catch") To Get More Information About The Web Exception"
//try
//{
//    string content = client.DownloadString($"https://{craigslistCity.Replace(" ", string.Empty)}.craigslist.org/{method}/sss?query={craigslistCategory}&sort=rel");
//    //string content = client.DownloadString("https://london.craigslist.org/search/sss?query=ford&sort=rel");
//    Console.WriteLine(content);
//}
//catch (WebException exception)
//{
//    string responseText = string.Empty;
//    var responseStream = exception.Response?.GetResponseStream();
//    if (responseStream != null)
//    {
//    using (var reader = new StreamReader(responseStream))
//    {
//        responseText = reader.ReadToEnd();
//    }
//}
//Console.WriteLine(responseText);
//}
#endregion
