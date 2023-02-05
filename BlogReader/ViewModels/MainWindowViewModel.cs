using BlogReader.Commands;
using BlogReader.Stores;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace BlogReader.ViewModels
{
    public class NavigationMenuItem : INotifyPropertyChanged
    {
        private bool _isSelected = false;

        public string Title { get; set; }
        public string Icon { get; set; }
        public string ViewModelType { get; set; }
        public bool IsExitItem { get; set; }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class MainWindowViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        
        private bool _isMenuExpanded = false;

        public bool IsMenuExpanded
        {
            get { return _isMenuExpanded; }
            set
            {
                _isMenuExpanded = value;
                OnPropertyChanged(nameof(IsMenuExpanded));
            }
        }

        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        public BaseCommand NavigateCommand { get; }

        public ObservableCollection<NavigationMenuItem> MenuItems { get; set; }

        public MainWindowViewModel(NavigationStore navigationStore, NotificationsStore notificationsStore, BlogPostItemsStore blogPostItemsStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            NavigateCommand = new NavigateCommand(navigationStore: navigationStore, notificationsStore: notificationsStore, blogPostItemsStore);

            MenuItems = new ObservableCollection<NavigationMenuItem>()
            {
                new NavigationMenuItem()
                {
                    Title = "Home",
                    Icon = "Home",
                    ViewModelType = nameof(HomeViewModel),
                    IsSelected = true,
                },
                new NavigationMenuItem()
                {
                    Title = "Blogs",
                    Icon = "NewspaperOutline",
                    ViewModelType = nameof(BlogsViewModel)
                },
                new NavigationMenuItem()
                {
                    Title = "Notifications",
                    Icon = "BellOutline",
                    ViewModelType = nameof(NotificationsLogViewModel)
                },
                new NavigationMenuItem()
                {
                    Title = "Settings",
                    Icon = "Cog",
                    ViewModelType = nameof(NotificationsLogViewModel)
                },
                new NavigationMenuItem()
                {
                    Title = "Exit",
                    Icon = "PowerOff",
                    ViewModelType = nameof(NotificationsLogViewModel),
                    IsExitItem = true
                }
            };
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));

            MenuItems.ForEach(mi => mi.IsSelected = false);

            var currentViewModelType = CurrentViewModel.GetType().Name;

            // TODO: Change to SingleOrDefault
            var menuItem = MenuItems.FirstOrDefault(mi => mi.ViewModelType == currentViewModelType);
            menuItem.IsSelected = true;
        }
    }
}
