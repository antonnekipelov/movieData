using Prism.Mvvm;
using RecomendMovie.Models;

namespace RecomendMovie.ViewModels
{
    public class MovieDetailsViewModel : BindableBase
    {
        private readonly Movie _movie;

        public MovieDetailsViewModel(Movie movie)
        {
            _movie = movie;
        }

        public string Name => _movie.Name;

        public int Date => _movie.Date;

        public string Tagline => _movie.Tagline;

        public string Description => _movie.Description;

        public int Minute => _movie.Minute;

        public double Rating => _movie.Rating;

        // Здесь предполагается, что вы храните путь к постеру как часть модели Movie
        //public string Poster => _movie.PosterPath;
    }
}
