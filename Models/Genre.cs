using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecomendMovie.Models
{
    public class Genre
    {
        [Name("id")]
        public int MovieId { get; set; }
        [Name("genre")]
        public string GenreName { get; set; }
    }
}
