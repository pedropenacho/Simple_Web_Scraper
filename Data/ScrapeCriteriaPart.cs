using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; //Allows use of Regex
using System.Threading.Tasks;

namespace WebScrapper.Data
{
    class ScrapeCriteriaPart
    {
        public string Regex { get; set; }
        public RegexOptions RegexOption { get; set; }
    }
}
