using CsvHelper.Configuration.Attributes;
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
