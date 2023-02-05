using BlogReader.Commands;
using BlogReader.Commands.Notifications;
using BlogReader.DataModels;
using BlogReader.DataModels.Enums;
using BlogReader.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BlogReader.ViewModels
{
    public class NotificationsLogViewModel : BaseViewModel
    {
        private readonly NotificationsStore _notificationsStore;
        private readonly ObservableCollection<Notification> _notifications = new ObservableCollection<Notification>();
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

        public bool EnableInput => !IsLoading;

        public bool HasItems => _notifications.Any();

        public IEnumerable<Notification> Notifications => _notifications;

        public BaseCommand ClearNotificationsCommand { get; }

        public NotificationsLogViewModel(NotificationsStore notificationsStore)
        {
            _notificationsStore = notificationsStore;
            ClearNotificationsCommand = new ClearAllNotificationsCommand(notificationsStore, this);

            LoadNotifications();
            _notificationsStore.NotificationsChanged += OnNotificationsUpdated;
        }

        private void LoadNotifications()
        {
            IsItemsGridLoading = true;

            try
            {
                _notifications.Clear();
                foreach (Notification notification in _notificationsStore.Notifications)
                {
                    _notifications.Add(notification);
                }

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
