using BlogReader.ViewModels;
using System;

namespace BlogReader.Commands.Notifications
{
    public class ClearFilterClickedCommand : BaseCommand
    {
        private readonly NotificationsLogViewModel _viewModel;

        public ClearFilterClickedCommand(NotificationsLogViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            try
            {
                _viewModel.LoadNotifications();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
