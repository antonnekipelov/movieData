using Prism.Commands;
using Prism.Mvvm;
using System.IO;
using RecomendMovie.Services;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RecomendMovie.ViewModels
{
    public class MovieDetailsViewModel : BindableBase
    {
        private readonly ServiceMovieRating _serviceMovieRating;
        private readonly MovieService _movieService;
        private readonly Movie _movie;
        private string _moviesCsvPath;

        public MovieDetailsViewModel(int movieId,  string _projectDirectory, string _postersDirectory)
        {
            _movieService = new MovieService();
            _serviceMovieRating = new ServiceMovieRating();
            _moviesCsvPath = Path.Combine(_projectDirectory, "Data", "movies.csv");
            _movie = _movieService.GetMovieById(movieId, _moviesCsvPath);
            _moviesCsvPath = Path.Combine(_projectDirectory, "Data", "movies.csv");
            Poster = new BitmapImage(new Uri(Path.Combine(_postersDirectory, $"{movieId}.jpg"), UriKind.Absolute));
            LikeCommand = new DelegateCommand(LikeMovie);
            DisLikeCommand = new DelegateCommand(DisLikeMovie);
        }
        private void LikeMovie()
        {
            _serviceMovieRating.RateMovie(_movie, true);
        }
        private void DisLikeMovie()
        {
            _serviceMovieRating.RateMovie(_movie, false);
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
        public ICommand LikeCommand { get; }
        public ICommand DisLikeCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
