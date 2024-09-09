using CsvHelper.Configuration.Attributes;
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
