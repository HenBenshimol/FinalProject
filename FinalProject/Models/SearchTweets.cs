using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class SearchTweets
    {
        public string Query { get; set; }

        public string ResultsXml { get; set; }
    }
}
