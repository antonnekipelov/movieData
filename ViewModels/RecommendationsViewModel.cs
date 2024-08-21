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
        private readonly IMovieService _movieService;
        private const int PostersPerPage = 20;
        private string _postersDirectory;
        private int _currentPage = 1;
        private ObservableCollection<BitmapImage> _currentPosters;

        public RecommendationsViewModel(IMovieService movieService)
        {
            _movieService = movieService;
            LoadPostersCommand = new DelegateCommand(LoadPosters);
            NextPageCommand = new DelegateCommand(OnNextPage, CanGoNext);
            PreviousPageCommand = new DelegateCommand(OnPreviousPage, CanGoPrevious);
            OpenPosterCommand = new DelegateCommand<BitmapImage>(OpenPoster);

            // Вызов команды при инициализации
            LoadPostersCommand.Execute(null);
        }

        /*private void LoadMovies()
        {
            Movies = new ObservableCollection<Movie>(_movieService.GetMovies());
        }*/

        private void LoadPosters()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            _postersDirectory = Path.Combine(projectDirectory, "Data", "posters");
            MessageBox.Show(_postersDirectory);
            var allPosters = new List<BitmapImage>();
            string posterPath = "";
            for (int i = 1000001; i <= 1000100; i++)
            {
                posterPath = Path.Combine(_postersDirectory, $"{i}.jpg");
                //MessageBox.Show(posterPath);
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
                else
                {
                    // Файл не найден
                    MessageBox.Show($"File not found: {posterPath}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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
            {
                MessageBox.Show("No posters loaded!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Movie> Movies { get; private set; }

        public ObservableCollection<BitmapImage> CurrentPosters
        {
            get => _currentPosters;
            set => SetProperty(ref _currentPosters, value);
        }

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand LoadPostersCommand { get; }
        public ICommand OpenPosterCommand { get; }
        public string CurrentPageDisplay => $"Page {_currentPage} of {TotalPages}";

        private int TotalPages { get; set; }

        public bool CanGoNext() => _currentPage < TotalPages;

        public bool CanGoPrevious() => _currentPage > 1;

        private void OnNextPage()
        {
            if (CanGoNext())
            {
                _currentPage++;
                LoadPosters();
                (NextPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (PreviousPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }
        private void OnPreviousPage()
        {
            if (CanGoPrevious())
            {
                _currentPage--;
                LoadPosters();
                (NextPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (PreviousPageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }
        private void OpenPoster(BitmapImage poster)
        {
            var modalWindow = new MovieDetailsView();  // Создаем экземпляр модального окна
            modalWindow.Owner = Application.Current.MainWindow; // Устанавливаем владельцем текущее главное окно
            modalWindow.ShowDialog();  // Открываем модальное окно
        }
    }
}