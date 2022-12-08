using BlogReader.Commands.Notifications;
using BlogReader.Models;
using BlogReader.Models.Enums;
using BlogReader.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BlogReader.ViewModels
{
    public class NotificationsLogViewModel : BaseViewModel
    {
        private readonly NotificationsStore _notificationsStore;
        private readonly ObservableCollection<Notification> _notifications;
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

        public IEnumerable<Notification> Notifications => _notifications;

        public ICommand ClearNotificationsCommand { get; }

        public NotificationsLogViewModel(NotificationsStore notificationsStore)
        {
            _notificationsStore = notificationsStore;
            _notifications = new ObservableCollection<Notification>();
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
            }
            catch (Exception ex)
            {
                var error = new Notification($"Failed to load notifications: {ex}", MessageType.Error);
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
