using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using RecomendMovie.Services;
using System.Windows;

namespace RecomendMovie.ViewModels
{
    public class MovieDetailsViewModel : BindableBase
    {
        private readonly MovieService _movieService;
        private readonly ServiceMovieRating _serviceMovieRating;
        private readonly UserService _userService;
        private readonly string _postersDirectory;
        private int _movieID;

        public MovieDetailsViewModel(int id, ServiceMovieRating serviceMovieRating, UserService userService, string postersDirectory)
        {
            _movieID = id;
            _serviceMovieRating = serviceMovieRating;
            _userService = userService;
            _movieService = new MovieService();
            _postersDirectory = postersDirectory;

            try
            {
                Poster = LoadPoster();
                Countries = LoadData(() => _movieService.GetCountriesByMovieId(_movieID));
                Directors = LoadData(() => _movieService.GetDirectorsByMovieId(_movieID));
                Genres = LoadData(() => _movieService.GetGenresByMovieId(_movieID));
                Languages = LoadData(() => _movieService.GetLanguagesByMovieId(_movieID));
            }
            catch (Exception ex)
            {
                HandleError("Error loading movie details", ex);
            }

            LikeCommand = new DelegateCommand(LikeMovie, CanRateMovie);
            DisLikeCommand = new DelegateCommand(DisLikeMovie, CanRateMovie);
        }

        private BitmapImage LoadPoster()
        {
            try
            {
                var movie = _movieService.GetMovieById(_movieID);
                var posterPath = Path.Combine(_postersDirectory, $"{movie.Id}.jpg");
                if (File.Exists(posterPath))
                    return new BitmapImage(new Uri(posterPath, UriKind.Absolute));
                else
                    throw new FileNotFoundException("Poster not found.", posterPath);
            }
            catch (Exception ex)
            {
                HandleError("Error loading poster", ex);
                return null;
            }
        }

        private List<string> LoadData(Func<List<string>> dataLoader)
        {
            try
            {
                return dataLoader();
            }
            catch (Exception ex)
            {
                HandleError("Error loading movie data", ex);
                return new List<string>();
            }
        }

        private void LikeMovie()
        {
            try
            {
                _serviceMovieRating.RateMovie(_userService.GetCurrentUser(), _movieID, true);
            }
            catch (Exception ex)
            {
                HandleError("Error while liking the movie", ex);
            }
        }

        private void DisLikeMovie()
        {
            try
            {
                _serviceMovieRating.RateMovie(_userService.GetCurrentUser(), _movieID, false);
            }
            catch (Exception ex)
            {
                HandleError("Error while disliking the movie", ex);
            }
        }

        private bool CanRateMovie()
        {
            return _userService.GetCurrentUser() != null;
        }

        private void HandleError(string message, Exception ex)
        {
            MessageBox.Show($"{message}: {ex.Message}");
        }

        public string Name => SafeGetMovieData(() => _movieService.GetMovieById(_movieID).Name);
        public int Date => SafeGetMovieData(() => _movieService.GetMovieById(_movieID).Date, 0);
        public string Tagline => SafeGetMovieData(() => _movieService.GetMovieById(_movieID).Tagline);
        public string Description => SafeGetMovieData(() => _movieService.GetMovieById(_movieID).Description);
        public int Minute => SafeGetMovieData(() => _movieService.GetMovieById(_movieID).Minute, 0);
        public double Rating => SafeGetMovieData(() => _movieService.GetMovieById(_movieID).Rating, 0.0);
        public BitmapImage Poster { get; }
        public List<string> Countries { get; }
        public List<string> Directors { get; }
        public List<string> Genres { get; }
        public List<string> Languages { get; }
        public ICommand LikeCommand { get; }
        public ICommand DisLikeCommand { get; }

        private T SafeGetMovieData<T>(Func<T> dataFetcher, T defaultValue = default)
        {
            try
            {
                return dataFetcher();
            }
            catch (Exception ex)
            {
                HandleError("Error fetching movie data", ex);
                return defaultValue;
            }
        }
    }
}
