using BlogReader.Commands;
using BlogReader.Commands.Blogs.BlogSources;
using BlogReader.Models;
using BlogReader.Models.Enums;
using BlogReader.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;

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
        private BitmapImage _sourceImg;
        private Dictionary<string, string> _dataPropertyErrors;

        public readonly BitmapImage DefualtSourceImg = new BitmapImage(new Uri(@"\Assets\DefaultSourceImage.png", UriKind.Relative));

        public BitmapImage SourceImg
        {
            get { return _sourceImg; }
            set
            {
                _sourceImg = value;
                OnPropertyChanged(nameof(SourceImg));
            }
        }

        public Dictionary<string, string> DataPropertyErrors => _dataPropertyErrors;

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

        public event Action ItemRemoved;

        public BaseCommand AddNewSourceItemCommand { get; }
        public BaseCommand InsertOrUpdateBlogItemSourceCommand { get; }
        public BaseCommand CancelNewSourceItemCommand { get; }
        public BaseCommand EditBlogSourceItemCommand { get; }        
        public BaseCommand RemoveBlogSourceItemCommand { get; }
        public BaseCommand UploadImageCommand { get; }

        public BlogSourcesViewModel(NotificationsStore notificationsStore, 
            BlogPostItemsStore blogPostItemsStore)
        {
            _dataPropertyErrors = new Dictionary<string, string>
            {
                { nameof(BlogPostItemSource.SourceName), string.Empty },
                { nameof(BlogPostItemSource.SourceUrl), string.Empty }
            };

            _blogPostItemsStore = blogPostItemsStore;
            _notificationsStore = notificationsStore;

            AddNewSourceItemCommand = new CreateNewBlogSourceItemCommand(this);
            CancelNewSourceItemCommand = new CancelNewSourceItemCommand(this);
            InsertOrUpdateBlogItemSourceCommand = new InsertOrUpdateBlogItemSourceCommand(this, blogPostItemsStore, notificationsStore);
            EditBlogSourceItemCommand = new EditBlogSourceItemCommand(this, blogPostItemsStore, notificationsStore);
            RemoveBlogSourceItemCommand = new RemoveBlogSourceItemCommand(this, blogPostItemsStore, notificationsStore);
            UploadImageCommand = new UploadImageCommand(this, notificationsStore);

            InsertOrUpdateBlogItemSourceCommand.OnExecuted += InvokeItemRemovedEvent;
            RemoveBlogSourceItemCommand.OnExecuted += InvokeItemRemovedEvent;

            LoadSources();
            _blogPostItemsStore.BlogPostItemSourcesChanged += OnSourcesUpdated;
        }

        public void AddDataPropertyError(string propertyName, string errorMessage) 
        {
            if (_dataPropertyErrors.ContainsKey(propertyName) == false)
            {
                _dataPropertyErrors.Add(propertyName, string.Empty);
            }

            _dataPropertyErrors[propertyName] = errorMessage;
            OnPropertyChanged(nameof(DataPropertyErrors));
        }

        public void ClearDataPropertyErrors()
        {
            _dataPropertyErrors.Clear();
            OnPropertyChanged(nameof(DataPropertyErrors));
        }

        public bool HasDataPropertyErrors()
        {
            return _dataPropertyErrors.Any(dpi => string.IsNullOrEmpty(dpi.Value) == false);
        }

        private void LoadSources()
        {
            IsItemsGridLoading = true;

            try
            {
                _blogPostItemsSources.Clear();
                foreach (BlogPostItemSource source in _blogPostItemsStore.GetAllBlogPostItemSources())
                {
                    _blogPostItemsSources.Add(BlogPostItemSource.CreateNewCopy(source));
                }
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to load blog sources", ex.ToString());
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
    
        private void InvokeItemRemovedEvent(object sender, EventArgs args)
        {
            ItemRemoved?.Invoke();
        }
    }
}