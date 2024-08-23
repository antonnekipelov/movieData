using Prism.Mvvm;
using RecomendMovie.Models;
using RecomendMovie.Services;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace RecomendMovie.ViewModels
{
    public class MovieDetailsViewModel : INotifyPropertyChanged
    {
        private readonly Movie _movie;

        public MovieDetailsViewModel(int movieId, IMovieService movieService, string csvFilePath)
        {
            _movie = movieService.GetMovieById(movieId, csvFilePath);
            Poster = new BitmapImage(new Uri("Data/posters/" + $"{movieId}.jpg", UriKind.RelativeOrAbsolute));
        }

        public string Name => _movie.Name;

        public int Date => _movie.Date;

        public string Tagline => _movie.Tagline;

        public string Description => _movie.Description;

        public int Minute => _movie.Minute;

        public double Rating => _movie.Rating;
        private BitmapImage _poster;

        public BitmapImage Poster
        {
            get => _poster;
            set
            {
                _poster = value;
                OnPropertyChanged(nameof(Poster));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
