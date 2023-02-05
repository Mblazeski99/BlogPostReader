using BlogReader.Commands;
using BlogReader.Commands.App;
using BlogReader.Models;
using BlogReader.Stores;
using System.Collections.ObjectModel;
using System.Linq;

namespace BlogReader.ViewModels
{
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
        public BaseCommand ExitAppCommand { get; }

        public ObservableCollection<NavigationMenuItem> TopMenuItems { get; set; }
        public ObservableCollection<NavigationMenuItem> BottomMenuItems { get; set; }

        public MainWindowViewModel(NavigationStore navigationStore, NotificationsStore notificationsStore, BlogPostItemsStore blogPostItemsStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            NavigateCommand = new NavigateCommand(navigationStore: navigationStore, notificationsStore: notificationsStore, blogPostItemsStore);
            ExitAppCommand = new ExitAppCommand();

            TopMenuItems = new ObservableCollection<NavigationMenuItem>()
            {
                new NavigationMenuItem()
                {
                    Title = "Home",
                    Icon = "Home",
                    Command = NavigateCommand,
                    CommandParameter = nameof(HomeViewModel),
                    IsSelected = true,
                },
                new NavigationMenuItem()
                {
                    Title = "Blogs",
                    Icon = "NewspaperOutline",
                    Command = NavigateCommand,
                    CommandParameter = nameof(BlogsViewModel)
                },
                new NavigationMenuItem()
                {
                    Title = "Notifications",
                    Icon = "BellOutline",
                    Command = NavigateCommand,
                    CommandParameter = nameof(NotificationsLogViewModel)
                },
                new NavigationMenuItem()
                {
                    Title = "Settings",
                    Icon = "Cog",
                    Command = NavigateCommand,
                    CommandParameter = nameof(NotificationsLogViewModel)
                }
            };

            BottomMenuItems = new ObservableCollection<NavigationMenuItem>()
            {
                new NavigationMenuItem()
                {
                    Title = "Exit",
                    Icon = "PowerOff",
                    Command = ExitAppCommand,
                    IsExitItem = true
                }
            };
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));

            TopMenuItems.ForEach(mi => mi.IsSelected = false);

            var currentViewModelType = CurrentViewModel.GetType().Name;

            // TODO: Change to SingleOrDefault
            var menuItem = TopMenuItems.FirstOrDefault(mi => mi.CommandParameter == currentViewModelType);
            if (menuItem == null) menuItem = BottomMenuItems.FirstOrDefault(mi => mi.CommandParameter == currentViewModelType);

            menuItem.IsSelected = true;
        }
    }
}
