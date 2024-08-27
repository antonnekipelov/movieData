using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using RecomendMovie.Models;
using RecomendMovie.Services;
using System.Collections.ObjectModel;

namespace RecomendMovie.ViewModels
{
    public class MovieDetailsViewModel : BindableBase
    {
        private readonly MovieService _movieService;
        private readonly ServiceMovieRating _serviceMovieRating;
        private readonly Movie _movie;
        private readonly User _user;

        public MovieDetailsViewModel(Movie movie, User user, string _postersDirectory)
        {
            _movie = movie;
            _user = user;
            _movieService = new MovieService();
            _serviceMovieRating = new ServiceMovieRating();
            Poster = new BitmapImage(new Uri(Path.Combine(_postersDirectory, $"{_movie.Id}.jpg"), UriKind.Absolute));
            Countries = new ObservableCollection<Country>(_movieService.GetCountriesByMovieId(movie.Id));
            Directors = new ObservableCollection<Director>(_movieService.GetDirectorsByMovieId(movie.Id));
            Genres = new ObservableCollection<Genre>(_movieService.GetGenresByMovieId(movie.Id));
            Languages = new ObservableCollection<Language>(_movieService.GetLanguagesByMovieId(movie.Id));
            LikeCommand = new DelegateCommand(LikeMovie);
            DisLikeCommand = new DelegateCommand(DisLikeMovie);
        }
        private void LikeMovie()
        {
            _serviceMovieRating.RateMovie(_user, _movie.Id, true);
        }
        private void DisLikeMovie()
        {
            _serviceMovieRating.RateMovie(_user, _movie.Id, false);
        }
        public string Name => _movie.Name;
        public int Date => _movie.Date;
        public string Tagline => _movie.Tagline;
        public string Description => _movie.Description;
        public int Minute => _movie.Minute;
        public double Rating => _movie.Rating;
        public BitmapImage Poster { get;}
        public ObservableCollection<Country> Countries { get; }
        public ObservableCollection<Director> Directors { get; }
        public ObservableCollection<Genre> Genres { get; }
        public ObservableCollection<Language> Languages { get; }
        public ICommand LikeCommand { get; }
        public ICommand DisLikeCommand { get; }
    }
}
