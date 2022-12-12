using BlogReader.Commands;
using BlogReader.Commands.Blogs.BlogSources;
using BlogReader.Models;
using BlogReader.Models.Enums;
using BlogReader.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlogReader.ViewModels
{
    public class BlogSourcesViewModel : BaseViewModel
    {
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;
        private readonly ObservableCollection<BlogPostItemSource> _blogPostItemsSources = new ObservableCollection<BlogPostItemSource>();

        private BlogPostItemSource _selectedSourceItem;
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

        public BlogPostItemSource SelectedSourceItem
        {
            get { return _selectedSourceItem; }
            set
            {
                _selectedSourceItem = value;
                OnPropertyChanged(nameof(SelectedSourceItem));
            }
        }

        public bool EnableInput => !IsLoading;

        public IEnumerable<BlogPostItemSource> BlogPostItemSources => _blogPostItemsSources;

        public BaseCommand AddOrEditBlogSourceItemCommand { get; }
        public BaseCommand InsertOrUpdateBlogItemSourceCommand { get; }
        public BaseCommand RemoveBlogItemSourceCommand { get; }

        public BlogSourcesViewModel(NotificationsStore notificationsStore, BlogPostItemsStore blogPostItemsStore)
        {
            _blogPostItemsStore = blogPostItemsStore;
            _notificationsStore = notificationsStore;

            AddOrEditBlogSourceItemCommand = new AddOrEditBlogSourceItemCommand(this);

            LoadSources();
            _blogPostItemsStore.BlogPostItemSourcesChanged += OnSourcesUpdated;
        }

        private void LoadSources()
        {
            IsItemsGridLoading = true;

            try
            {
                _blogPostItemsSources.Clear();
                foreach (BlogPostItemSource source in _blogPostItemsStore.BlogPostItemSources)
                {
                    _blogPostItemsSources.Add(source);
                }
            }
            catch (Exception ex)
            {
                var error = new Notification($"Failed to load blog sources: {ex}", MessageType.Error);
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                IsItemsGridLoading = false;
            }
        }

        private void OnSourcesUpdated(object sender, EventArgs args)
        {
            LoadSources();
        }
    }
}