using Newtonsoft.Json;
using RecomendMovie.Models;
using System.IO;
using System.Windows;

public class ServiceMovieRating
{
    private const string _ratingsFilePath = "ratings.json";
    private List<MovieRating> _ratings;
    public event Action _ratingsUpdated;

    public ServiceMovieRating()
    {
        try
        {
            _ratings = LoadRatings();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while loading ratings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _ratings = new List<MovieRating>();
        }
    }

    public void RateMovie(User user, int movieId, bool rate)
    {
        try
        {
            var existingRating = _ratings.FirstOrDefault(r => r.MovieId == movieId && r.User.Username == user.Username);

            if (existingRating != null)
                existingRating.Rate = rate;
            else
            {
                var movieRating = new MovieRating
                {
                    User = user,
                    MovieId = movieId,
                    Rate = rate
                };
                _ratings.Add(movieRating);
            }

            SaveRatings();

            _ratingsUpdated?.Invoke();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while rating the movie: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public List<int> GetPositiveRatedMovies (User user)
    {
        try
        {
            return _ratings
                .Where(r => r.User.Username == user.Username && r.Rate)
                .Select(r => r.MovieId)
                .ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while fetching positive rated movies: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return new List<int>();
        }
    }

    public List<MovieRating> LoadRatings()
    {
        try
        {
            if (!File.Exists(_ratingsFilePath))
            {
                return new List<MovieRating>();
            }

            var json = File.ReadAllText(_ratingsFilePath);
            return JsonConvert.DeserializeObject<List<MovieRating>>(json) ?? new List<MovieRating>();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while loading ratings from file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return new List<MovieRating>();
        }
    }

    public List<MovieRating> GetRatedMovies(User user)
    {
        try
        {
            return _ratings.Where(r => r.User.Username == user.Username).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while fetching rated movies: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return new List<MovieRating>();
        }
    }

    private void SaveRatings()
    {
        try
        {
            var json = JsonConvert.SerializeObject(_ratings, Formatting.Indented);
            File.WriteAllText(_ratingsFilePath, json);
            MessageBox.Show("Your rating has been recorded.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while saving ratings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
