using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecomendMovie.Models
{
    public class Country
    {
        [Name("id")]
        public int MovieId { get; set; }
        [Name("country")]
        public string CountryName { get; set; }
    }
}
