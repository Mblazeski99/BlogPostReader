using BlogReader.Commands;
using BlogReader.Commands.Home;
using BlogReader.DataModels;
using BlogReader.DataModels.Enums;
using BlogReader.Stores;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BlogReader.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;
        private readonly ObservableCollection<BlogPostItem> _allBlogPostItems = new ObservableCollection<BlogPostItem>();
        private readonly ObservableCollection<BlogPostItem> _shownBlogPostItems = new ObservableCollection<BlogPostItem>();

        private BlogPostItemPreviewViewModel _selectedBlogPostItemDataContext;
        private bool _isLoading;
        private bool _isExpanded;
        private int _numberOfItemsShown;
        private Visibility _showMoreItemsButtonVisibility = Visibility.Collapsed;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
                OnPropertyChanged(nameof(EnableInput));
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));
            }
        }

        public int NumberOfItemsShown
        {
            get { return _numberOfItemsShown; }
            set
            {
                _numberOfItemsShown = value;
                SetItemsToShow();
                OnPropertyChanged(nameof(NumberOfItemsShown));
            }
        }

        public Visibility ShowMoreItemsButtonVisibility
        {
            get { return _showMoreItemsButtonVisibility; }
            set
            {
                _showMoreItemsButtonVisibility = value;
                OnPropertyChanged(nameof(ShowMoreItemsButtonVisibility));
            }
        }

        public BlogPostItemPreviewViewModel SelectedBlogPostItemDataContext
        {
            get { return _selectedBlogPostItemDataContext; }
            set
            {
                _selectedBlogPostItemDataContext = value;
                OnPropertyChanged(nameof(SelectedBlogPostItemDataContext));
            }
        }

        public bool EnableInput => !IsLoading;

        public ObservableCollection<BlogPostItem> ShownBlogPostItems => _shownBlogPostItems;
        public ObservableCollection<BlogPostItem> AllBlogPostItems => _allBlogPostItems;

        public BaseCommand SelectBlogPostItemCommand { get; }
        public BaseCommand DeSelectBlogPostItemCommand { get; }
        public BaseCommand ToggleIsExpandedCommand { get; }
        public BaseCommand IncreaseNumberOfItemsShownCommand { get; }

        public HomeViewModel(BlogPostItemsStore blogPostItemsStore, NotificationsStore notificationsStore)
        {
            _blogPostItemsStore = blogPostItemsStore;
            _notificationsStore = notificationsStore;

            SelectBlogPostItemCommand = new SelectBlogPostItemCommand(this);
            DeSelectBlogPostItemCommand = new DeSelectBlogPostItemCommand(this);
            ToggleIsExpandedCommand = new ToggleIsExpandedCommand(this);
            IncreaseNumberOfItemsShownCommand = new IncreaseNumberOfItemsShownCommand(this);

            SelectedBlogPostItemDataContext = new BlogPostItemPreviewViewModel();

            LoadBlogPostItems();
            _blogPostItemsStore.BlogPostItemsChanged += OnBlogPostItemsChanged;

            NumberOfItemsShown = 20;
        }

        public override void Dispose()
        {
            AppData.App.MainWindow.Title = "Blog Reader";
            base.Dispose();
        }

        public void SetItemsToShow()
        {
            IsLoading = true;
            _shownBlogPostItems.Clear();
            _shownBlogPostItems.AddRange(_allBlogPostItems.Take(NumberOfItemsShown));

            OnPropertyChanged(nameof(ShownBlogPostItems));

            ShowMoreItemsButtonVisibility = (_shownBlogPostItems.Count == _allBlogPostItems.Count) ? Visibility.Collapsed : Visibility.Visible;
            IsLoading = false;
        }

        private void LoadBlogPostItems()
        {
            IsLoading = true;

            try
            {
                _allBlogPostItems.Clear();
                foreach (BlogPostItem model in _blogPostItemsStore.GetAllBlogPostItems().OrderByDescending(b => b.Date))
                {
                    _allBlogPostItems.Add(BlogPostItem.CreateNewCopy(model));
                }

                SetItemsToShow();
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to load blog posts", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OnBlogPostItemsChanged(object sender, EventArgs args)
        {
            LoadBlogPostItems();
        }
    }
}
