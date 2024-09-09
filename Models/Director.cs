using CsvHelper.Configuration.Attributes;
namespace RecomendMovie.Models
{
    public class Director
    {
        [Name("id")]
        public int MovieId { get; set; }
        [Name("name")]
        public string Name { get; set; }
    }
}
