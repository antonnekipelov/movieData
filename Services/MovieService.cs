using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;
using CsvHelper;
using CsvHelper.Configuration;
using Prism.Commands;
using RecomendMovie.Models;

namespace RecomendMovie.Services
{
    public class MovieService
    {
        private readonly string _moviesCsvPath;
        private readonly string _countriesCsvPath;
        private readonly string _directorsCsvPath;
        private readonly string _genresCsvPath;
        private readonly string _languagesCsvPath;
        private readonly List<Movie> _movies;
        public MovieService()
        {
            string _projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            _moviesCsvPath = Path.Combine(_projectDirectory, "Data", "movies.csv");
            _countriesCsvPath = Path.Combine(_projectDirectory, "Data", "countries.csv");
            _directorsCsvPath = Path.Combine(_projectDirectory, "Data", "directors.csv");
            _genresCsvPath = Path.Combine(_projectDirectory, "Data", "genres.csv");
            _languagesCsvPath = Path.Combine(_projectDirectory, "Data", "languages.csv");
        }
        public IEnumerable<Movie> GetMovies()
        {
            using (var reader = new StreamReader(_moviesCsvPath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<Movie>().ToList();
            }
        }
        public List<Country> GetCountriesByMovieId(int movieId)
        {
            using (var reader = new StreamReader(_countriesCsvPath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<Country>().Where(c => c.MovieId == movieId).ToList();
            }
        }
        public List<Director> GetDirectorsByMovieId(int movieId)
        {
            using (var reader = new StreamReader(_directorsCsvPath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<Director>().Where(d => d.MovieId == movieId).ToList();
            }
        }
        public List<Genre> GetGenresByMovieId(int movieId)
        {
            using (var reader = new StreamReader(_genresCsvPath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<Genre>().Where(g => g.MovieId == movieId).ToList();
            }
        }
        public List<Language> GetLanguagesByMovieId(int movieId)
        {
            using (var reader = new StreamReader(_languagesCsvPath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<Language>().Where(l => l.MovieId == movieId).ToList();
            }
        }
        public Movie GetMovieById(int movieId)
        {
            var movies = GetMovies();
            return movies.FirstOrDefault(movie => movie.Id == movieId);
        }
    }
}
