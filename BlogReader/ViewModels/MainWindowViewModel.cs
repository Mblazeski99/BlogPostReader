﻿using BlogReader.Commands;
using BlogReader.Stores;
using System.Windows.Input;

namespace BlogReader.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand NavigateCommand { get; }

        public MainWindowViewModel(NavigationStore navigationStore, NotificationsStore notificationsStore, BlogPostItemsStore blogPostItemsStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            NavigateCommand = new NavigateCommand(navigationStore: navigationStore, notificationsStore: notificationsStore, blogPostItemsStore);
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}