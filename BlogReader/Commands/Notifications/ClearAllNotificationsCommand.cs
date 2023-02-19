using BlogReader.DataModels.Enums;
using BlogReader.DataModels;
using BlogReader.Stores;
using System;
using BlogReader.ViewModels;
using BlogReader.CustomControls;
using BlogReader.Enums;
using System.Windows;

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
                ExtendedMessageBoxResult answer = ExtendedMessageBox.Show("Are you sure you want to clear the log?", "Clear Notifications", ExtendedMessageBoxButton.YesNo, ExtendedMessageBoxImage.Question);

                if (answer == ExtendedMessageBoxResult.Yes)
                {
                    _notificationsLogViewModel.IsLoading = true;
                    _notificationsStore.ClearNotifications();
                }
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to load notifications", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                _notificationsLogViewModel.IsLoading = false;
            }
        }
    }
}
