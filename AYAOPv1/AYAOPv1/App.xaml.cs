using AYAOPv1.Source.MVVM.ViewModel;
using AYAOPv1.Source.Services;
using System.Windows;

namespace AYAOPv1
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore navigationStore;

        public App()
        {
            navigationStore = new NavigationStore();

            //ConfigureServices.Configure(services);
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //navigationStore.CurrentView = new HomeViewModel(navigationStore);
            navigationStore.CurrentView = new HomeViewModel();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };
            MainWindow.Show();
        }
    }
}
