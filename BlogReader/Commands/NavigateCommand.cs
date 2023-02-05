using BlogReader.DataModels.Enums;
using BlogReader.DataModels;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System;

namespace BlogReader.Commands
{
    public class NavigateCommand : BaseCommand
    {
        private readonly NavigationStore _navigationStore;
        private readonly NotificationsStore _notificationsStore;
        private readonly BlogPostItemsStore _blogPostItemsStore;

        public NavigateCommand(NavigationStore navigationStore, NotificationsStore notificationsStore, BlogPostItemsStore blogPostItemsStore)
        {
            _navigationStore = navigationStore;
            _notificationsStore = notificationsStore;
            _blogPostItemsStore = blogPostItemsStore;
        }

        public override void Execute(object parameter)
        {
            try
            {
                if (parameter == null) throw new ArgumentNullException(nameof(parameter), "Parameter Was Null");

                string viewModelType = parameter.ToString();
                BaseViewModel? viewModel = null;

                switch (viewModelType)
                {
                    case nameof(HomeViewModel):
                        if (_navigationStore.CurrentViewModel.GetType() != typeof(HomeViewModel))
                            viewModel = new HomeViewModel(_blogPostItemsStore, _notificationsStore);
                        break;

                    case nameof(MainWindowViewModel):
                        if (_navigationStore.CurrentViewModel.GetType() != typeof(MainWindowViewModel))
                            viewModel = new MainWindowViewModel(_navigationStore, _notificationsStore, _blogPostItemsStore);
                        break;

                    case nameof(BlogsViewModel):
                        if (_navigationStore.CurrentViewModel.GetType() != typeof(BlogsViewModel))
                            viewModel = new BlogsViewModel(_notificationsStore, _blogPostItemsStore);
                        break;

                    case nameof(BlogSourcesViewModel):
                        if (_navigationStore.CurrentViewModel?.GetType() != typeof(BlogSourcesViewModel))
                            viewModel = new BlogSourcesViewModel(_notificationsStore, _blogPostItemsStore);
                        break;

                    case nameof(NotificationsLogViewModel):
                        if (_navigationStore.CurrentViewModel.GetType() != typeof(NotificationsLogViewModel))
                            viewModel = new NotificationsLogViewModel(_notificationsStore);
                        break;

                    default:
                        throw new Exception("ViewModel type does not exist");
                }

                if (viewModel != null)
                {
                    _navigationStore.CurrentViewModel = viewModel;
                }
            }
            catch (Exception ex)
            {
                Notification error = new Notification(MessageType.Error, ex.ToString(), ex.ToString());
                _notificationsStore.AddNotification(error);
            }
        }
    }
}
