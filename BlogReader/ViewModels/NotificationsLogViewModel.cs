using BlogReader.Commands;
using BlogReader.Commands.Notifications;
using BlogReader.CustomControls.GridFilterPopup;
using BlogReader.DataModels;
using BlogReader.DataModels.Enums;
using BlogReader.Helpers;
using BlogReader.Stores;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BlogReader.ViewModels
{
    public class NotificationsLogViewModel : BaseViewModel
    {
        private readonly NotificationsStore _notificationsStore;
        private readonly ObservableCollection<Notification> _allNotifications = new ObservableCollection<Notification>();
        private readonly ObservableCollection<Notification> _notifications = new ObservableCollection<Notification>();
        
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

        public bool HasItems => _notifications.Any();

        public ObservableCollection<Notification> Notifications => _notifications;
        public GridFilterDescriptor FilterDescriptor
        {
            get { return _filterDescriptor; }
            private set
            {
                _filterDescriptor = value;
                OnPropertyChanged(nameof(FilterDescriptor));
            }
        }

        public BaseCommand ClearNotificationsCommand { get; }
        public BaseCommand FilterClickedCommand { get; }
        public BaseCommand ClearFilterClickedCommand { get; }

        public NotificationsLogViewModel(NotificationsStore notificationsStore)
        {
            _notificationsStore = notificationsStore;
            ClearNotificationsCommand = new ClearAllNotificationsCommand(notificationsStore, this);
            FilterClickedCommand = new FilterClickedCommand(this);
            ClearFilterClickedCommand = new ClearFilterClickedCommand(this);

            LoadNotifications();
            _notificationsStore.NotificationsChanged += OnNotificationsUpdated;
        }

        public void LoadNotifications()
        {
            IsItemsGridLoading = true;

            try
            {
                _notifications.Clear();
                _allNotifications.Clear();
                foreach (Notification notification in _notificationsStore.GetAllNotifications())
                {
                    var modelCopy = Notification.CreateNewCopy(notification);
                    _notifications.Add(modelCopy);
                    _allNotifications.Add(modelCopy);
                }

                var propertyNames = new ObservableCollection<string>
                {
                    nameof(Notification.Message),
                    nameof(Notification.DateCreated),
                    nameof(Notification.MessageType)
                };

                var itemsSource = new ObservableCollection<BaseEntity>();
                _allNotifications.ForEach(n => itemsSource.Add(n));

                FilterDescriptor = new GridFilterDescriptor(itemsSource, propertyNames);

                OnPropertyChanged(nameof(HasItems));
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to load notifications", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                IsItemsGridLoading = false;
            }
        }

        private void OnNotificationsUpdated()
        {
            LoadNotifications();
        }

        public override void Dispose()
        {
            _notificationsStore.NotificationsChanged -= OnNotificationsUpdated;
            base.Dispose();
        }
    }
}
