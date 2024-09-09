using Prism.Commands;
using RecomendMovie.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RecomendMovie.Views;
using Prism.Mvvm;
using System.Windows;

namespace RecomendMovie.ViewModels
{
    public class RecommendationsViewModel : BindableBase
    {
        private const int ItemsPerPage = 12;
        private int _currentPostersPage;
        private int _currentRecomendationsPage;
        private readonly UserService _userService;
        private readonly MovieService _movieService;
        private List<BitmapImage> _allPosters;
        private List<BitmapImage> _allRecomendations;
        private readonly ServiceMovieRating _serviceMovieRating;
        private string _postersDirectory = "";
        private string _selectedGenre;
        private string _selectedDirector;
        private int? _selectedYear;
        public string SelectedGenre
        {
            get => _selectedGenre;
            set => SetProperty(ref _selectedGenre, value, () => ApplyFilters());
        }
        public string SelectedDirector
        {
            get => _selectedDirector;
            set => SetProperty(ref _selectedDirector, value, () => ApplyFilters());
        }
        public int? SelectedYear
        {
            get => _selectedYear;
            set => SetProperty(ref _selectedYear, value, () => ApplyFilters());
        }
        public ObservableCollection<BitmapImage> Posters { get; private set; } = new ObservableCollection<BitmapImage>();
        public ObservableCollection<string> AvailableGenres { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableDirectors { get; private set; } = new ObservableCollection<string>();
        public ObservableCollection<int> AvailableYears { get; private set; } = new ObservableCollection<int>();
        public ObservableCollection<BitmapImage> Recomendations { get; private set; } = new ObservableCollection<BitmapImage>();
        public RecommendationsViewModel(UserService userService)
        {
            _userService = userService;
            _movieService = new MovieService();
            _serviceMovieRating = new ServiceMovieRating();
            try
            {
                var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                _postersDirectory = Path.Combine(projectDirectory, "Data", "posters");
            }
            catch (Exception ex)
            {
                HandleError("Error determining posters directory", ex);
            }
            _currentPostersPage = 1;
            _currentRecomendationsPage = 1;
            _serviceMovieRating._ratingsUpdated += OnRatingsUpdated;
            LoadPostersCommand = new DelegateCommand(LoadPosters);
            LoadRecomendationsCommand = new DelegateCommand(LoadRecomendations);
            FiltersCommand = new DelegateCommand(FillAvailableFilters);
            ResetFiltersCommand = new DelegateCommand(ResetFilters);
            PosterClickCommand = new DelegateCommand<BitmapImage>(OnPosterClick);
            NextPostersPageCommand = new DelegateCommand(OnNextPostersPage, () => CanGoNextPosters);
            PreviousPostersPageCommand = new DelegateCommand(OnPreviousPostersPage, () => CanGoPreviousPosters);
            NextRecomendationsPageCommand = new DelegateCommand(OnNextRecomendationsPage, () => CanGoNextRecomendations);
            PreviousRecomendationsPageCommand = new DelegateCommand(OnPreviousRecomendationsPage, () => CanGoPreviousRecomendations);
            try
            {
                LoadPostersCommand.Execute(null);
                LoadRecomendationsCommand.Execute(null);
                FiltersCommand.Execute(null);
            }
            catch (Exception ex)
            {
                HandleError("Error initializing view model", ex);
            }
        }
        public ICommand NextPostersPageCommand { get; }
        public ICommand PreviousPostersPageCommand { get; }
        public ICommand LoadPostersCommand { get; }
        public ICommand FiltersCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand PosterClickCommand { get; }
        public ICommand NextRecomendationsPageCommand { get; }
        public ICommand PreviousRecomendationsPageCommand { get; }
        public ICommand LoadRecomendationsCommand { get; }
        private void LoadPosters()
        {
            try
            {
                _allPosters = new List<BitmapImage>();
                foreach (var movie in _movieService.GetMovies())
                {
                    if (ApplyMovieFilters(movie.Id))
                    {
                        var posterPath = Path.Combine(_postersDirectory, $"{movie.Id}.jpg");
                        if (File.Exists(posterPath))
                        {
                            var image = new BitmapImage(new Uri(posterPath, UriKind.Absolute));
                            _allPosters.Add(image);
                        }
                    }
                }
                ApplyFilters();
            }
            catch (Exception ex)
            {
                HandleError("Error loading posters", ex);
            }
        }
        private List<int> GetUnratedMovieIds()
        {
            try
            {
                var ratedMovieIds = _serviceMovieRating.GetRatedMovies(_userService.GetCurrentUser()).Select(r => r.MovieId).ToList();
                var allMovieIds = _movieService.GetMovies().Select(m => m.Id).ToList();
                return allMovieIds.Except(ratedMovieIds).ToList();
            }
            catch (Exception ex)
            {
                HandleError("Error fetching unrated movie IDs", ex);
                return new List<int>();
            }
        }
        private void LoadRecomendations()
        {
            try
            {
                _allRecomendations = new List<BitmapImage>();
                var positiveRatedMovies = _serviceMovieRating.GetPositiveRatedMovies(_userService.GetCurrentUser());
                var unratedMovieIds = GetUnratedMovieIds();

                List<string> userLikedDirectors = new List<string>();
                List<string> userLikedGenres = new List<string>();

                foreach (var movieId in positiveRatedMovies)
                {
                    userLikedDirectors.AddRange(_movieService.GetDirectorsByMovieId(movieId));
                    userLikedGenres.AddRange(_movieService.GetGenresByMovieId(movieId));
                }

                foreach (var movieId in unratedMovieIds)
                {
                    var movieDirectors = _movieService.GetDirectorsByMovieId(movieId);
                    var movieGenres = _movieService.GetGenresByMovieId(movieId);

                    bool isMatching = movieDirectors.Intersect(userLikedDirectors).Any() ||
                                        movieGenres.Intersect(userLikedGenres).Any();

                    if (isMatching)
                    {
                        var movie = _movieService.GetMovieById(movieId);
                        _allRecomendations.Add(new BitmapImage(new Uri(Path.Combine(_postersDirectory, $"{movie.Id}.jpg"), UriKind.Absolute)));
                    }
                }

                UpdateRecomendationsPage();
            }
            catch (Exception ex)
            {
                HandleError("Error loading recommendations", ex);
            }
        }
        private void ApplyFilters()
        {
            try
            {
                var filteredPosters = _allPosters.AsEnumerable();
                if (!string.IsNullOrEmpty(_selectedGenre))
                    filteredPosters = filteredPosters
                        .Where(p => _movieService.GetGenresByMovieId(GetMovieIdFromPoster(p)).Contains(_selectedGenre));
                if (!string.IsNullOrEmpty(_selectedDirector))
                    filteredPosters = filteredPosters
                        .Where(p => _movieService.GetDirectorsByMovieId(GetMovieIdFromPoster(p)).Contains(_selectedDirector));
                if (_selectedYear.HasValue)
                    filteredPosters = filteredPosters
                        .Where(p => _movieService.GetMovieById(GetMovieIdFromPoster(p)).Date == _selectedYear.Value);
                Posters.Clear();
                foreach (var poster in filteredPosters.Take(ItemsPerPage))
                    Posters.Add(poster);
                LoadRecomendations();
            }
            catch (Exception ex)
            {
                HandleError("Error applying filters", ex);
            }
        }
        private bool ApplyMovieFilters(int movieID)
        {
            try
            {
                if (!string.IsNullOrEmpty(_selectedGenre) && !_movieService.GetGenresByMovieId(movieID).Contains(_selectedGenre))
                    return false;

                if (!string.IsNullOrEmpty(_selectedDirector) && !_movieService.GetDirectorsByMovieId(movieID).Contains(_selectedDirector))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                HandleError($"Error applying movie filters for movie ID {movieID}", ex);
                return false;
            }
        }
        private void ResetFilters()
        {
            SelectedGenre = null;
            SelectedDirector = null;
            SelectedYear = null;
            ApplyFilters();
        }
        private int GetMovieIdFromPoster(BitmapImage poster)
        {
            var fileName = Path.GetFileNameWithoutExtension(poster.UriSource.LocalPath);
            return int.TryParse(fileName, out int movieId) ? movieId : 0;
        }
        private void FillAvailableFilters()
        {
            try
            {
                var allMovies = _movieService.GetMovies();
                AvailableGenres.Clear();
                AvailableDirectors.Clear();
                AvailableYears.Clear();
                foreach (var genre in allMovies.SelectMany(m => _movieService.GetGenresByMovieId(m.Id)).Distinct())
                    AvailableGenres.Add(genre);
                foreach (var director in allMovies.SelectMany(m => _movieService.GetDirectorsByMovieId(m.Id)).Distinct())
                    AvailableDirectors.Add(director);
                foreach (var year in allMovies.Select(m => m.Date).Distinct())
                    AvailableYears.Add(year);
            }
            catch (Exception ex)
            {
                HandleError("Error filling available filters", ex);
            }
        }

        private void UpdatePostersPage()
        {
            try
            {
                Posters.Clear();
                var itemsForPage = _allPosters.Skip((_currentPostersPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
                foreach (var item in itemsForPage)
                    Posters.Add(item);
                (NextPostersPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (PreviousPostersPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                HandleError("Error updating posters page", ex);
            }
        }
        private void UpdateRecomendationsPage()
        {
            try
            {
                Recomendations.Clear();
                var itemsForPage = _allRecomendations.Skip((_currentRecomendationsPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
                foreach (var item in itemsForPage)
                    Recomendations.Add(item);
                (NextRecomendationsPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (PreviousRecomendationsPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                HandleError("Error updating recommendations page", ex);
            }
        }
        private void OnRatingsUpdated()
        {
            try
            {
                LoadRecomendationsCommand.Execute(null);
                (NextRecomendationsPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (PreviousRecomendationsPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                HandleError("Error handling ratings update", ex);
            }
        }
        private void OnPosterClick(BitmapImage poster)
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(poster.UriSource.LocalPath);
                if (int.TryParse(fileName, out int movieId))
                {
                    OpenMovieDetails(movieId);
                }
            }
            catch (Exception ex)
            {
                HandleError("Error handling poster click", ex);
            }
        }
        private void OpenMovieDetails(int movieId)
        {
            try
            {
                var movieDetailsViewModel = new MovieDetailsViewModel(movieId, _serviceMovieRating, _userService, _postersDirectory);
                var movieDetailsView = new MovieDetailsView
                {
                    DataContext = movieDetailsViewModel
                };
                movieDetailsView.ShowDialog();
            }
            catch (Exception ex)
            {
                HandleError($"Error opening movie details for movie ID {movieId}", ex);
            }
        }
        private void OnNextPostersPage()
        {
            try
            {
                if (CanGoNextPosters)
                {
                    _currentPostersPage++;
                    UpdatePostersPage();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error navigating to the next posters page", ex);
            }
        }
        private void OnPreviousPostersPage()
        {
            try
            {
                if (CanGoPreviousPosters)
                {
                    _currentPostersPage--;
                    UpdatePostersPage();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error navigating to the previous posters page", ex);
            }
        }
        private void OnNextRecomendationsPage()
        {
            try
            {
                if (CanGoNextRecomendations)
                {
                    _currentRecomendationsPage++;
                    UpdateRecomendationsPage();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error navigating to the next recommendations page", ex);
            }
        }
        private void OnPreviousRecomendationsPage()
        {
            try
            {
                if (CanGoPreviousRecomendations)
                {
                    _currentRecomendationsPage--;
                    UpdateRecomendationsPage();
                }
            }
            catch (Exception ex)
            {
                HandleError("Error navigating to the previous recommendations page", ex);
            }
        }
        private bool CanGoNextPosters => _currentPostersPage * ItemsPerPage < _allPosters.Count;
        private bool CanGoNextRecomendations => _currentRecomendationsPage * ItemsPerPage < _allRecomendations.Count;
        private bool CanGoPreviousPosters => _currentPostersPage > 1;
        private bool CanGoPreviousRecomendations => _currentRecomendationsPage > 1;
        private void HandleError(string message, Exception ex)
        {
            MessageBox.Show($"{message}: {ex.Message}");
        }
    }
}
