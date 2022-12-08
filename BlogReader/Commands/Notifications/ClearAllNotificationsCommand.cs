using BlogReader.Models.Enums;
using BlogReader.Models;
using BlogReader.Stores;
using System;
using System.Windows;
using BlogReader.ViewModels;

namespace BlogReader.Commands.Notifications
{
    public class ClearAllNotificationsCommand : BaseCommand
    {
        private readonly NotificationsStore _notificationsStore;
        private readonly NotificationsLogViewModel _notificationsLogViewModel;

        public ClearAllNotificationsCommand(NotificationsStore notificationsStore, NotificationsLogViewModel notificationsLogViewModel)
        {
            _notificationsStore = notificationsStore;
            _notificationsLogViewModel = notificationsLogViewModel;
        }

        public override async void Execute(object parameter)
        {
            try
            {
                MessageBoxResult answer = MessageBox.Show("Are you sure you want to clear the log?", "Clear Notifications", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (answer == MessageBoxResult.Yes)
                {
                    _notificationsLogViewModel.IsLoading = true;
                    _notificationsStore.ClearNotifications();
                }
            }
            catch (Exception ex)
            {
                var error = new Notification($"Failed to notifications: {ex}", MessageType.Error);
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                _notificationsLogViewModel.IsLoading = false;
            }
        }
    }
}
