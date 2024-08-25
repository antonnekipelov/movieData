using Prism.Ioc;
using Prism.Unity;
using RecomendMovie.Services;
using RecomendMovie.ViewModels;
using RecomendMovie.Views;
using System.Windows;

namespace RecomendMovie
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<UserService, UserService>();
            containerRegistry.RegisterSingleton<MovieService, MovieService>();
            containerRegistry.RegisterForNavigation<RecommendationsView, RecommendationsViewModel>();
            containerRegistry.RegisterForNavigation<RegistrationView, RegistrationViewModel>();
            containerRegistry.RegisterForNavigation<MovieDetailsView, MovieDetailsViewModel>();
        }

        protected override Window CreateShell()
        {
            var registrationWindow = Container.Resolve<RegistrationView>();
            registrationWindow.DataContext = Container.Resolve<RegistrationViewModel>();
            return registrationWindow;
        }

       /*public void ShowRecommendationsView()
        {
            var recommendationsWindow = Container.Resolve<RecommendationsView>();
            recommendationsWindow.DataContext = Container.Resolve<RecommendationsViewModel>();
            recommendationsWindow.Show();
            var registrationWindow = Current.MainWindow;
            Current.MainWindow = recommendationsWindow;
            // Закрываем предыдущее главное окно (окно регистрации)
            registrationWindow.Close();
        }*/
    }
}
