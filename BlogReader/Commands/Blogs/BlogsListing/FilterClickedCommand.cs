using BlogReader.CustomControls.GridFilterPopup;
using BlogReader.DataModels;
using BlogReader.DataModels.Enums;
using BlogReader.Stores;
using BlogReader.ViewModels;
using Serilog;
using System;

namespace BlogReader.Commands.Blogs.BlogsListing
{
    public class FilterClickedCommand : BaseCommand
    {
        private readonly BlogsListingViewModel _viewModel;
        private readonly NotificationsStore _notificationsStore;

        public FilterClickedCommand(BlogsListingViewModel viewModel, NotificationsStore notificationsStore)
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
                    _viewModel.BlogPostItems.Clear();
                    foreach (var item in eventArgs.FilteredItems)
                    {
                        _viewModel.BlogPostItems.Add(item as BlogPostItem);
                    }
                }
                else
                {
                    var error = new Notification(MessageType.Error, "Filtering of blog items failed!");
                    _notificationsStore.AddNotification(error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to filter blog items!");
            }
        }
    }
}
