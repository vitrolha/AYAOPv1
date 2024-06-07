using System;
using AYAOPv1.Source.Utils;

namespace AYAOPv1.Source.Services
{
    public class NavigationStore
    {
        private ViewModelBase currentView;

        public ViewModelBase CurrentView
        {
            get { return currentView; }
            set { currentView = value; OnViewModelChanged(); }
        }
        private void OnViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

        public event Action CurrentViewModelChanged;
    }
}
