using BlogReader.Commands;
using BlogReader.Models.Enums;
using BlogReader.Models;
using BlogReader.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using BlogReader.Commands.Blogs.ContentModels;

namespace BlogReader.ViewModels
{
    public class ContentModelsViewModel : BaseViewModel
    {
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;
        private readonly ObservableCollection<RssContentModel> _rssContentModels = new ObservableCollection<RssContentModel>();

        private RssContentModel _selectedContentModel;
        private bool _isItemsGridLoading;
        private bool _isLoading;
        private Dictionary<string, string> _dataPropertyErrors;

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

        public RssContentModel SelectedContentModel
        {
            get { return _selectedContentModel; }
            set
            {
                _selectedContentModel = value;
                OnPropertyChanged(nameof(SelectedContentModel));
            }
        }

        public bool EnableInput => !IsLoading;

        public ObservableCollection<RssContentModel> RssContentModels => _rssContentModels;

        public BaseCommand AddNewContentModelCommand { get; }
        public BaseCommand InsertOrUpdateContentModelCommand { get; }
        public BaseCommand RemoveContentModelCommand { get; }
        public BaseCommand EditContentModelCommand { get; }
        public BaseCommand CancelNewContentModelCommand { get; }

        public ContentModelsViewModel(NotificationsStore notificationsStore,
            BlogPostItemsStore blogPostItemsStore)
        {
            _dataPropertyErrors = new Dictionary<string, string>
            {
                { nameof(RssContentModel.ModelName), string.Empty },
                { nameof(RssContentModel.ItemContainerTag), string.Empty },
                { nameof(RssContentModel.TitleTag), string.Empty },
                { nameof(RssContentModel.DateTag), string.Empty },
                { nameof(RssContentModel.ContentTag), string.Empty }
            };

            _blogPostItemsStore = blogPostItemsStore;
            _notificationsStore = notificationsStore;

            AddNewContentModelCommand = new CreateNewContentModelCommand(this);
            InsertOrUpdateContentModelCommand = new InsertOrUpdateContentModelCommand(this, blogPostItemsStore, notificationsStore);
            RemoveContentModelCommand = new RemoveContentModelCommand(this, blogPostItemsStore, notificationsStore);
            EditContentModelCommand = new EditContentModelCommand(this, blogPostItemsStore, notificationsStore);
            CancelNewContentModelCommand = new CancelNewContentModelCommand(this);

            LoadContentModels();
            _blogPostItemsStore.RssContentModelsChanged += OnContentModelsUpdated;
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

        private void LoadContentModels()
        {
            IsItemsGridLoading = true;

            try
            {
                _rssContentModels.Clear();
                foreach (RssContentModel model in _blogPostItemsStore.RssContentModels)
                {
                    _rssContentModels.Add(RssContentModel.CreateNewCopy(model));
                }
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to load content models", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                IsItemsGridLoading = false;
            }
        }

        private void OnContentModelsUpdated(object sender, EventArgs args)
        {
            LoadContentModels();
        }
    }
}
