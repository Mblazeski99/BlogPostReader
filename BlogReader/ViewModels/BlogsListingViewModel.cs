﻿using BlogReader.Commands;
using BlogReader.DataModels.Enums;
using BlogReader.DataModels;
using BlogReader.Stores;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using BlogReader.Commands.Blogs.BlogsListing;
using BlogReader.CustomControls.GridFilterPopup;
using BlogReader.Helpers;

namespace BlogReader.ViewModels
{
    public class BlogsListingViewModel : BaseViewModel
    {
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;
        private readonly ObservableCollection<BlogPostItem> _allBlogPostItems = new ObservableCollection<BlogPostItem>();
        private readonly ObservableCollection<BlogPostItem> _blogPostItems = new ObservableCollection<BlogPostItem>();

        private bool _isItemsGridLoading;
        private bool _isLoading;
        private GridFilterDescriptor _filterDescriptor;

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

        public bool EnableInput => !IsLoading;
        public bool HasItems => _blogPostItems.Any();

        public ObservableCollection<BlogPostItem> BlogPostItems => _blogPostItems;
        public GridFilterDescriptor FilterDescriptor 
        { 
            get { return _filterDescriptor; }
            private set
            {
                _filterDescriptor = value;
                OnPropertyChanged(nameof(FilterDescriptor));
            }
        }

        public BaseCommand ClearAllBlogPostItemsCommand { get; }
        public BaseCommand PreviewBlogPostItemCommand { get; }
        public BaseCommand RemoveBlogPostItemCommand { get; }
        public BaseCommand FilterClickedCommand { get; }
        public BaseCommand ClearFilterClickedCommand { get; }

        public BlogsListingViewModel(NotificationsStore notificationsStore,
            BlogPostItemsStore blogPostItemsStore)
        {
            _blogPostItemsStore = blogPostItemsStore;
            _notificationsStore = notificationsStore;

            ClearAllBlogPostItemsCommand = new ClearAllBlogPostItemsCommand(this, notificationsStore, blogPostItemsStore);
            PreviewBlogPostItemCommand = new PreviewBlogPostItemCommand(notificationsStore, blogPostItemsStore);
            RemoveBlogPostItemCommand = new RemoveBlogPostItemCommand(this, notificationsStore, blogPostItemsStore);
            FilterClickedCommand = new FilterClickedCommand(this);
            ClearFilterClickedCommand = new ClearFilterClickedCommand(this);

            LoadBlogPostItems();
            _blogPostItemsStore.BlogPostItemsChanged += OnBlogPostItemsChanged;
        }

        public override void Dispose()
        {
            PreviewBlogPostItemCommand.Dispose();
            base.Dispose();
        }

        public void LoadBlogPostItems()
        {
            IsItemsGridLoading = true;

            try
            {
                _blogPostItems.Clear();
                _allBlogPostItems.Clear();
                foreach (BlogPostItem model in _blogPostItemsStore.GetAllBlogPostItems().OrderByDescending(b => b.Date))
                {
                    var modelCopy = BlogPostItem.CreateNewCopy(model);
                    _blogPostItems.Add(modelCopy);
                    _allBlogPostItems.Add(modelCopy);
                }
                
                var propertyNames = new ObservableCollection<string>
                {
                    nameof(BlogPostItem.Title),
                    nameof(BlogPostItem.Summary),
                    nameof(BlogPostItem.Date),
                    nameof(BlogPostItem.SourceName),
                    nameof(BlogPostItem.Author),
                    nameof(BlogPostItem.MarkedAsWatchLater),
                    nameof(BlogPostItem.MarkedAsIrrelevant),
                    nameof(BlogPostItem.IsDeleted)
                };

                var itemSource = new ObservableCollection<BaseEntity>();
                _allBlogPostItems.ForEach(b => itemSource.Add(b));

                FilterDescriptor = new GridFilterDescriptor(itemSource, propertyNames);

                OnPropertyChanged(nameof(HasItems));
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