using BlogReader.CustomControls.GridFilterPopup;
using BlogReader.DataModels;
using BlogReader.ViewModels;
using System;

namespace BlogReader.Commands.Notifications
{
    public class FilterClickedCommand : BaseCommand
    {
        private readonly NotificationsLogViewModel _viewModel;

        public FilterClickedCommand(NotificationsLogViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            try
            {
                var eventArgs = (parameter as GridFilterPopupButtonEventArgs);

                _viewModel.Notifications.Clear();
                foreach (var item in eventArgs.FilteredItems)
                {
                    _viewModel.Notifications.Add(item as Notification);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
