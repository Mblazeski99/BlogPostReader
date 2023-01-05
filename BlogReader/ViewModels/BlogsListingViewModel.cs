using BlogReader.Commands;
using BlogReader.Models.Enums;
using BlogReader.Models;
using BlogReader.Stores;
using System.Collections.ObjectModel;
using System;

namespace BlogReader.ViewModels
{
    public class BlogsListingViewModel : BaseViewModel
    {
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;
        private readonly ObservableCollection<BlogPostItem> _blogPostItems = new ObservableCollection<BlogPostItem>();

        private BlogPostItem _selectedBlogPostItem;
        private bool _isItemsGridLoading;
        private bool _isLoading;

        public bool IsItemsGridLoading
        {
            get { return _isItemsGridLoading; }
            set
            {
                _isItemsGridLoading = value;
                OnPropertyChanged(nameof(IsItemsGridLoading));
            }
        }

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

        public BlogPostItem SelectedBlogPostItem
        {
            get { return _selectedBlogPostItem; }
            set
            {
                _selectedBlogPostItem = value;
                OnPropertyChanged(nameof(SelectedBlogPostItem));
            }
        }

        public bool EnableInput => !IsLoading;

        public ObservableCollection<BlogPostItem> BlogPostItems => _blogPostItems;

        public BaseCommand ClearAllBlogPostItemsCommand { get; }
        public BaseCommand PreviewBlogPostItemCommand { get; }
        public BaseCommand RemoveBlogPostItemCommand { get; }
        public BaseCommand CancelPreviewBlogPostItemCommand { get; }

        public BlogsListingViewModel(NotificationsStore notificationsStore,
            BlogPostItemsStore blogPostItemsStore)
        {
            _blogPostItemsStore = blogPostItemsStore;
            _notificationsStore = notificationsStore;

            //ClearAllBlogPostItemsCommand = new ClearAllBlogPostItemsCommand(this, blogPostItemsStore, notificationsStore);
            //PreviewBlogPostItemCommand = new PreviewBlogPostItemCommand(this, blogPostItemsStore, notificationsStore);
            //RemoveBlogPostItemCommand = new RemoveBlogPostItemCommand(this, blogPostItemsStore, notificationsStore);
            //CancelPreviewBlogPostItemCommand = new CancelPreviewBlogPostItemCommand(this);

            LoadBlogPostItems();
            _blogPostItemsStore.BlogPostItemsChanged += OnBlogPostItemsChanged;
        }

        private void LoadBlogPostItems()
        {
            IsItemsGridLoading = true;

            try
            {
                _blogPostItems.Clear();
                foreach (BlogPostItem model in _blogPostItemsStore.GetAllBlogPostItems())
                {
                    _blogPostItems.Add(BlogPostItem.CreateNewCopy(model));
                }
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to load blog posts", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                IsItemsGridLoading = false;
            }
        }

        private void OnBlogPostItemsChanged(object sender, EventArgs args)
        {
            LoadBlogPostItems();
        }
    }
}
