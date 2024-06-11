using AYAOPv1.Source.Class.Repositories;
using AYAOPv1.Source.Interfaces;
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
        private readonly IShortCutRepository shortCutRepository;
        public App()
        {
            navigationStore = new NavigationStore();
            shortCutRepository = new ShortCutRepository();
            //ConfigureServices.Configure(services);
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //navigationStore.CurrentView = new HomeViewModel(navigationStore);
            navigationStore.CurrentView = new HomeViewModel(shortCutRepository);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };
            MainWindow.Show();
        }
    }
}
