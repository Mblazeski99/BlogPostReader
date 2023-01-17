using BlogReader.Commands;
using BlogReader.Commands.Home;
using BlogReader.Models;
using BlogReader.Models.Enums;
using BlogReader.Stores;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BlogReader.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;
        private readonly ObservableCollection<BlogPostItem> _blogPostItems = new ObservableCollection<BlogPostItem>();

        private BlogPostItemPreviewViewModel _selectedBlogPostItemDataContext;
        private bool _isLoading;

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
        public bool HasItems => _blogPostItems.Any();

        public ObservableCollection<BlogPostItem> BlogPostItems => _blogPostItems;

        public BaseCommand SelectBlogPostItemCommand { get; }

        public HomeViewModel(BlogPostItemsStore blogPostItemsStore, 
            NotificationsStore notificationsStore)
        {
            _blogPostItemsStore = blogPostItemsStore;
            _notificationsStore = notificationsStore;

            SelectBlogPostItemCommand = new SelectBlogPostItemCommand(this);

            SelectedBlogPostItemDataContext = new BlogPostItemPreviewViewModel();

            LoadBlogPostItems();
            _blogPostItemsStore.BlogPostItemsChanged += OnBlogPostItemsChanged;
        }

        private void LoadBlogPostItems()
        {
            IsLoading = true;

            try
            {
                _blogPostItems.Clear();
                foreach (BlogPostItem model in _blogPostItemsStore.GetAllBlogPostItems().OrderByDescending(b => b.Date))
                {
                    _blogPostItems.Add(BlogPostItem.CreateNewCopy(model));
                }

                OnPropertyChanged(nameof(HasItems));
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
