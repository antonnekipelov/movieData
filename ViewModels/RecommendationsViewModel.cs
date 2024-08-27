using Prism.Mvvm;
using Prism.Commands;
using RecomendMovie.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RecomendMovie.Views;
using System.Windows;

namespace RecomendMovie.ViewModels
{
    public class RecommendationsViewModel : BindableBase
    {
        
        private const int PostersPerPage = 20;
        private int _currentPage = 1;
        private string _projectDirectory;
        private ObservableCollection<BitmapImage> _posters;
        private readonly UserService _userService;
        private readonly MovieService _movieService;
        string _postersDirectory = "";
        public RecommendationsViewModel(UserService userService)
        {
            _userService = userService;
            _movieService = new MovieService();
            _projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            _postersDirectory = Path.Combine(_projectDirectory, "Data", "posters");
            LoadPostersCommand = new DelegateCommand(LoadPosters);
            PosterClickCommand = new DelegateCommand<BitmapImage>(OnPosterClick);
            NextPageCommand = new DelegateCommand(OnNextPage, () => CanGoNext);
            PreviousPageCommand = new DelegateCommand(OnPreviousPage, () => CanGoPrevious);
            // Вызов команды при инициализации
            LoadPostersCommand.Execute(null);
        }
        private void LoadPosters()
        {
            var allPosters = new List<BitmapImage>();
            string posterPath = "";
            for (int i = 1000001; i <= 1000100; i++)
            {
                posterPath = Path.Combine(_postersDirectory, $"{i}.jpg");
                if (File.Exists(posterPath))
                {
                    try
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(posterPath, UriKind.Absolute);
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                        allPosters.Add(image);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image {i}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                /*else
                    // Файл не найден
                    MessageBox.Show($"File not found: {posterPath}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Warning);*/
            }

            // Обновляем CurrentPosters только если есть изображения
            if (allPosters.Any())
            {
                TotalPages = (allPosters.Count + PostersPerPage - 1) / PostersPerPage;

                CurrentPosters = new ObservableCollection<BitmapImage>(
                    allPosters.Skip((_currentPage - 1) * PostersPerPage).Take(PostersPerPage)
                );

                RaisePropertyChanged(nameof(CurrentPosters));
                RaisePropertyChanged(nameof(CurrentPageDisplay));
                RaisePropertyChanged(nameof(CanGoNext));
                RaisePropertyChanged(nameof(CanGoPrevious));
            }
            else
                MessageBox.Show("No posters loaded!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void OnPosterClick(BitmapImage poster)
        {
            // Получаем ID фильма из имени файла постера
            var fileName = System.IO.Path.GetFileNameWithoutExtension(poster.UriSource.LocalPath);
            if (int.TryParse(fileName, out int movieId))
                OpenMovieDetails(movieId);
        }
        public ObservableCollection<Movie> Movies { get; private set; }

        public ObservableCollection<BitmapImage> CurrentPosters
        {
            get => _posters;
            set => SetProperty(ref _posters, value);
        }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand LoadPostersCommand { get; }
        public ICommand OpenPosterCommand { get; }
        public ICommand PosterClickCommand { get; }
        public string CurrentPageDisplay => $"Page {_currentPage} of {TotalPages}";

        private int TotalPages { get; set; }

        public bool CanGoNext
        {
            get { return _currentPage < TotalPages; }
        }

        public bool CanGoPrevious
        {
            get { return _currentPage > 1; }
        }

        private void OnNextPage()
        {
            if (CanGoNext)
            {
                _currentPage++;
                LoadPosters();
                (NextPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (PreviousPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }
        private void OnPreviousPage()
        {
            if (CanGoPrevious)
            {
                _currentPage--;
                LoadPosters();
                (NextPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (PreviousPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }
        private void OpenMovieDetails(int movieId)
        {
            var movieDetailsViewModel = new MovieDetailsViewModel(_movieService.GetMovieById(movieId), _userService.GetCurrentUser(), _postersDirectory);
            var movieDetailsView = new MovieDetailsView
            {
                DataContext = movieDetailsViewModel
            };
            movieDetailsView.ShowDialog();
        }
    }
}