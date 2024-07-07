using BlogReader.Commands;
using BlogReader.Commands.Blogs.BlogSources;
using BlogReader.DataModels;
using BlogReader.DataModels.Enums;
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
        private readonly ObservableCollection<RssContentModel> _rssContentModels = new ObservableCollection<RssContentModel>();

        private BlogPostItemSource _selectedSourceItem;
        private RssContentModel _selectedRssContentModel;
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

        public RssContentModel SelectedRssContentModel
        {
            get { return _selectedRssContentModel; }
            set
            {
                _selectedRssContentModel = value;

                if (value is not null && SelectedSourceItem is not null)
                {
                    SelectedSourceItem.ContentModelId = value.Id;
                }

                OnPropertyChanged(nameof(SelectedRssContentModel));
            }
        }

        public bool EnableInput => !IsLoading;

        public int SelectedRssContentModelIndex
        {
            get 
            {
                int index = SelectedSourceItem != null 
                    ? RssContentModels.IndexOf(RssContentModels.SingleOrDefault(cm => cm.Id == SelectedSourceItem.ContentModelId)) 
                    : -1;

                return index;
            }
        }

        public ObservableCollection<BlogPostItemSource> BlogPostItemSources => _blogPostItemsSources;
        public ObservableCollection<RssContentModel> RssContentModels => _rssContentModels;

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
                { nameof(BlogPostItemSource.SourceUrl), string.Empty },
                { nameof(BlogPostItemSource.ContentModelId), string.Empty }
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
            EditBlogSourceItemCommand.OnExecuted += InvokeItemSelectedEvent;

            LoadData();
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

        private void LoadData()
        {
            IsItemsGridLoading = true;

            try
            {
                _blogPostItemsSources.Clear();
                foreach (BlogPostItemSource source in _blogPostItemsStore.GetAllBlogPostItemSources())
                {
                    _blogPostItemsSources.Add(BlogPostItemSource.CreateNewCopy(source));
                }

                string selectedRssContentModelId = SelectedRssContentModel?.Id;

                _rssContentModels.Clear();
                foreach (RssContentModel model in _blogPostItemsStore.GetAllRssContentModels())
                {
                    _rssContentModels.Add(RssContentModel.CreateNewCopy(model));
                }

                // re-select the SelectedRssContentModel value
                if (string.IsNullOrEmpty(selectedRssContentModelId) == false)
                {
                    SelectedRssContentModel = RssContentModels.SingleOrDefault(cm => cm.Id == selectedRssContentModelId);
                }
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to load blog sources data", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                IsItemsGridLoading = false;
            }
        }

        private void OnSourcesUpdated(object sender, EventArgs args)
        {
            LoadData();
        }
    
        private void InvokeItemRemovedEvent(object sender, EventArgs args)
        {
            ItemRemoved?.Invoke();
        }

        private void InvokeItemSelectedEvent(object sender, EventArgs args) 
        {
            OnPropertyChanged(nameof(SelectedRssContentModelIndex));
        }
    }
}