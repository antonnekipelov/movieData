using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using RecomendMovie.Models;

namespace RecomendMovie.Services
{
    public class MovieService
    {
        public IEnumerable<Movie> GetMovies(string csvFilePath)
        {
            if (string.IsNullOrEmpty(csvFilePath))
                throw new ArgumentNullException(nameof(csvFilePath), "CSV file path cannot be null or empty.");
            //var movies = new List<Movie>();
            if (!File.Exists(csvFilePath))
                throw new FileNotFoundException("CSV file not found", csvFilePath);
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                BadDataFound = context => { },
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                var movies = csv.GetRecords<Movie>().ToList();
                return movies;
            }
        }
        public Movie GetMovieById(int movieId, string csvFilePath)
        {
            var movies = GetMovies(csvFilePath);
            return movies.FirstOrDefault(movie => movie.Id == movieId);
        }
    }
}
