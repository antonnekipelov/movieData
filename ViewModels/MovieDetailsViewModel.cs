using Prism.Commands;
using Prism.Mvvm;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using RecomendMovie.Models;

namespace RecomendMovie.ViewModels
{
    public class MovieDetailsViewModel : BindableBase
    {
        private readonly ServiceMovieRating _serviceMovieRating;
        private readonly Movie _movie;
        private readonly User _user;

        public MovieDetailsViewModel(Movie movie, User user, string _postersDirectory)
        {
            _movie = movie;
            _user = user;
            _serviceMovieRating = new ServiceMovieRating();

            Poster = new BitmapImage(new Uri("Data/posters/" + $"{movie.Id}.jpg", UriKind.RelativeOrAbsolute));
            Poster = new BitmapImage(new Uri(Path.Combine(_postersDirectory, $"{_movie.Id}.jpg"), UriKind.Absolute));
            LikeCommand = new DelegateCommand(LikeMovie);
            DisLikeCommand = new DelegateCommand(DisLikeMovie);
        }
        private void LikeMovie()
        {
            _serviceMovieRating.RateMovie(_user, _movie, true);
        }
        private void DisLikeMovie()
        {
            _serviceMovieRating.RateMovie(_user, _movie, false);
        }
        public string Name => _movie.Name;
        public int Date => _movie.Date;
        public string Tagline => _movie.Tagline;
        public string Description => _movie.Description;
        public int Minute => _movie.Minute;
        public double Rating => _movie.Rating;
        public BitmapImage Poster { get; set; }
        public ICommand LikeCommand { get; }
        public ICommand DisLikeCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
