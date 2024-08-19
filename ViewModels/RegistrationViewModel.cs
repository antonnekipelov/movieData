using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using RecomendMovie.Services;
using RecomendMovie.Views;
using System.Windows;
using System.Windows.Input;

namespace RecomendMovie.ViewModels
{
    public class RegistrationViewModel : BindableBase
    {
        private readonly IUserService _userService;

        public RegistrationViewModel(IUserService userService)
        {
            _userService = userService;
            RegisterCommand = new DelegateCommand(OnRegister);
            LoginCommand = new DelegateCommand(OnLogin);
        }

        private void OnRegister()
        {
            if (_userService.Authenticate(Username, Password) != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = _userService.Register(Username, Password);
            if (user != null)
                OpenRecommendationsView();
            else
                MessageBox.Show("Не удалось зарегистрировать пользователя.", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnLogin()
        {
            var user = _userService.Authenticate(Username, Password);
            if (user != null)
                OpenRecommendationsView();
            else
                MessageBox.Show("Неверное имя пользователя или пароль.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void OpenRecommendationsView()
        {
            // Открываем окно рекомендаций и передаем логику закрытия окна регистрации в него
            var app = (App)Application.Current;
            var recommendationsWindow = app.Container.Resolve<RecommendationsView>();
            recommendationsWindow.DataContext = app.Container.Resolve<RecommendationsViewModel>();
            recommendationsWindow.Show();

            // Закрываем окно регистрации
            var registrationWindow = Application.Current.MainWindow;
            Application.Current.MainWindow = recommendationsWindow;
            registrationWindow.Close();
        }
        private string _username = "Herina";
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password = "a-I3fAa6";
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }
    }
}
