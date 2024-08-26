using Newtonsoft.Json;
using RecomendMovie.Models;
using System.Collections.Generic;
using System.IO;


public class ServiceMovieRating
{
    private readonly string _ratingsFilePath = "ratings.json";
    private List<MovieRating> _ratings;

    public ServiceMovieRating()
    {
        _ratings = LoadRatings();
    }

    public void RateMovie(User user, Movie movie, bool rate)
    {
        _ratings = LoadRatings();
        var existingRating = _ratings.FirstOrDefault(r => r.Movie.Id == movie.Id && r.User.Username == user.Username);
        if (existingRating != null)
            // Обновляем оценку, если фильм уже был оценен
            existingRating.Rate = rate;
        else
        {
            var movieRating = new MovieRating
            {
                User = user,
                Movie = movie,
                Rate = rate
            };

            _ratings.Add(movieRating);
        }
        SaveRatings();
    }
    public List<MovieRating> LoadRatings()
    {
        if (!File.Exists(_ratingsFilePath))
            return new List<MovieRating>();
        var json = File.ReadAllText(_ratingsFilePath);
        return JsonConvert.DeserializeObject<List<MovieRating>>(json);
    }
    private void SaveRatings()
    {
        var json = JsonConvert.SerializeObject(_ratings, Formatting.Indented);
        File.WriteAllText(_ratingsFilePath, json);
    }

    public void SaveRatingsOnExit()
    {
        SaveRatings();
    }

}
