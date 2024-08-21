using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using RecomendMovie.Models;

namespace RecomendMovie.Services
{
    public class MovieService : IMovieService
    {
        public IEnumerable<Movie> GetMovies(string csvFilePath)
        {
            var movies = new List<Movie>();

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                BadDataFound = context => { /* Обработка некорректных данных, если нужно */ },
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                // Используем стандартный маппинг на основе заголовков
                movies = csv.GetRecords<Movie>().ToList();
            }

            return movies;
        }
    }
}
