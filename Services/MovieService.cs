using RecomendMovie.Models;
namespace RecomendMovie.Services
{
    public class MovieService : IMovieService
    {
        public IEnumerable<Movie> GetMovies()
        {
            // Здесь можно заменить на реальное получение данных
            return new List<Movie>
            {
                new Movie { Id = 1, Title = "Inception", Genre = "Sci-Fi", Rating = 8.8 },
                new Movie { Id = 2, Title = "The Matrix", Genre = "Action", Rating = 8.7 }
                // Добавьте больше фильмов
            };
        }

        public IEnumerable<Recommendation> GetRecommendations(Movie movie)
        {
            // Простая логика для генерации рекомендаций
            return new List<Recommendation>
            {
                new Recommendation { Movie = new Movie { Title = "Interstellar", Genre = "Sci-Fi", Rating = 8.6 }, Reason = "Similar genre" },
                new Recommendation { Movie = new Movie { Title = "The Dark Knight", Genre = "Action", Rating = 9.0 }, Reason = "Similar genre" }
                // Добавьте больше рекомендаций
            };
        }
    }
}