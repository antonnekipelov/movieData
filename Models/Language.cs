using CsvHelper.Configuration.Attributes;
namespace RecomendMovie.Models
{
    public class Language
    {
        [Name("id")]
        public int MovieId { get; set; }
        [Name("type")]
        public string Type { get; set; } // Primary - оригинальный язык, Spoken - язык дубляжа
        [Name("language")]
        public string LanguageName { get; set; }
    }
}
