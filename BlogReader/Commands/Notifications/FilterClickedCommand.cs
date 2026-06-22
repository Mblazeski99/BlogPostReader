using BlogReader.CustomControls.GridFilterPopup;
using BlogReader.DataModels;
using BlogReader.DataModels.Enums;
using BlogReader.Stores;
using BlogReader.ViewModels;
using Serilog;
using System;

namespace BlogReader.Commands.Notifications
{
    public class FilterClickedCommand : BaseCommand
    {
        private readonly NotificationsLogViewModel _viewModel;
        private readonly NotificationsStore _notificationsStore;

        public FilterClickedCommand(NotificationsLogViewModel viewModel, NotificationsStore notificationsStore)
        {
            _viewModel = viewModel;
            _notificationsStore = notificationsStore;
        }

        public override void Execute(object parameter)
        {
            try
            {
                var eventArgs = (parameter as GridFilterPopupButtonEventArgs);

                if (eventArgs != null && eventArgs.IsSuccessful)
                {
                    _viewModel.Notifications.Clear();
                    foreach (var item in eventArgs.FilteredItems)
                    {
                        _viewModel.Notifications.Add(item as Notification);
                    }
                }
                else
                {
                    var error = new Notification(MessageType.Error, "Filtering of notifications failed!");
                    _notificationsStore.AddNotification(error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to filter notifications!");
            }
        }
    }
}
