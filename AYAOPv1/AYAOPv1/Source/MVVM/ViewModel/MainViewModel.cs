using AYAOPv1.Source.Services;
using AYAOPv1.Source.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AYAOPv1.Source.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        //Alterar views atraves da navigationstore
        private readonly NavigationStore navigationStore;
        public ViewModelBase CurrentView { get { return navigationStore.CurrentView; } }

        //Commands
        public RelayCommand CloseAppCommand { get; }
        public RelayCommand MinimizeAppCommand { get; }

        public MainViewModel(NavigationStore navigationStore)
        {
            //Navigationstore
            this.navigationStore = navigationStore;
            this.navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            //Commands
            CloseAppCommand = new RelayCommand(x => CloseApp(), x => true);
            MinimizeAppCommand = new RelayCommand(x => MinimizeApp(), x => true);
        }
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentView));
        }

        public void CloseApp()
        {
            Application.Current.Shutdown();
        }

        public void MinimizeApp()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }
}
