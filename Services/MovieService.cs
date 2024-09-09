using System.Globalization;
using System.IO;
using System.Windows;
using CsvHelper;
using CsvHelper.Configuration;
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
            try
            {
                using (var reader = new StreamReader(_moviesCsvPath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    return csv.GetRecords<Movie>().ToList();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"File not found: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
            catch (IOException ex)
            {
                MessageBox.Show($"I/O error: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        public List<string> GetCountriesByMovieId(int movieId)
        {
            try
            {
                using (var reader = new StreamReader(_countriesCsvPath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    return csv.GetRecords<Country>().Where(c => c.MovieId == movieId).Select(c => c.CountryName).ToList();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"File not found: {ex.Message}");
                return new List<string>();
            }
            catch (IOException ex)
            {
                MessageBox.Show($"I/O error: {ex.Message}");
                return new List<string>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
                return new List<string>();
            }
        }
        public List<string> GetDirectorsByMovieId(int movieId)
        {
            try
            {
                using (var reader = new StreamReader(_directorsCsvPath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    return csv.GetRecords<Director>().Where(d => d.MovieId == movieId).Select(d => d.Name).ToList();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"File not found: {ex.Message}");
                return new List<string>();
            }
            catch (IOException ex)
            {
                MessageBox.Show($"I/O error: {ex.Message}");
                return new List<string>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
                return new List<string>();
            }
        }

        public List<string> GetGenresByMovieId(int movieId)
        {
            try
            {
                using (var reader = new StreamReader(_genresCsvPath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    return csv.GetRecords<Genre>().Where(g => g.MovieId == movieId).Select(g => g.GenreName).ToList();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"File not found: {ex.Message}");
                return new List<string>();
            }
            catch (IOException ex)
            {
                MessageBox.Show($"I/O error: {ex.Message}");
                return new List<string>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
                return new List<string>();
            }
        }

        public List<string> GetLanguagesByMovieId(int movieId)
        {
            try
            {
                using (var reader = new StreamReader(_languagesCsvPath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    return csv.GetRecords<Language>().Where(l => l.MovieId == movieId).Select(l => l.LanguageName).ToList();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"File not found: {ex.Message}");
                return new List<string>();
            }
            catch (IOException ex)
            {
                MessageBox.Show($"I/O error: {ex.Message}");
                return new List<string>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
                return new List<string>();
            }
        }
        public List<int> GetYearByMovieId(int movieId)
        {
            try
            {
                using (var reader = new StreamReader(_moviesCsvPath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    return csv.GetRecords<Movie>().Where(y => y.Id == movieId).Select(y => y.Date).ToList();
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
                return new List<int>();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"I/O error: {ex.Message}");
                return new List<int>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new List<int>();
            }
        }

        public Movie GetMovieById(int movieId)
        {
            try
            {
                var movies = GetMovies();
                return movies.FirstOrDefault(movie => movie.Id == movieId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error in GetMovieById: {ex.Message}");
                return null;
            }
        }
    }
}
