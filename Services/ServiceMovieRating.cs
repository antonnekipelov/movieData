using RecomendMovie.Models;

public class ServiceMovieRating
{
    private readonly List<MovieRating> _movieRatings;

    public ServiceMovieRating()
    {
        _movieRatings = new List<MovieRating>();
    }

    public void RateMovie(Movie movie, bool rate)
    {
        // Проверяем, есть ли уже оценка для данного фильма
        var existingRating = _movieRatings.FirstOrDefault(r => r.Movie == movie);
        if (existingRating != null)
        {
            // Обновляем оценку, если фильм уже был оценен
            existingRating.Rate = rate;
        }
        else
        {
            // Добавляем новую оценку
            var newRating = new MovieRating
            {
                Movie = movie,
                Rate = rate
            };
            _movieRatings.Add(newRating);
        }
    }

    public IEnumerable<MovieRating> GetRatings()
    {
        return _movieRatings;
    }
}
