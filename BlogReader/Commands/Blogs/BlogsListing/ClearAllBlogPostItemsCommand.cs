using BlogReader.CustomControls;
using BlogReader.DataModels;
using BlogReader.DataModels.Enums;
using BlogReader.Enums;
using BlogReader.Stores;
using BlogReader.ViewModels;
using Serilog;
using System;
using System.Windows.Interop;

namespace BlogReader.Commands.Blogs.BlogsListing
{
    public class ClearAllBlogPostItemsCommand : BaseCommand
    {
        private readonly NotificationsStore _notificationsStore;
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly BlogsListingViewModel _viewModel;

        public ClearAllBlogPostItemsCommand(BlogsListingViewModel viewModel,
            NotificationsStore notificationsStore,
            BlogPostItemsStore blogPostItemsStore)
        {
            _notificationsStore = notificationsStore;
            _blogPostItemsStore = blogPostItemsStore;
            _viewModel = viewModel;
        }

        public override async void Execute(object parameter)
        {
            try
            {
                ExtendedMessageBoxResult answer = ExtendedMessageBox.Show("Are you sure you want to clear these blogs?", "Clear Blogs", ExtendedMessageBoxButton.YesNo, ExtendedMessageBoxImage.Question);

                if (answer == ExtendedMessageBoxResult.Yes)
                {
                    _viewModel.IsLoading = true;
                    _blogPostItemsStore.ClearBlogPostItems();
                }
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to load blog posts", ex.ToString());
                _notificationsStore.AddNotification(error);
                Log.Error(ex, "Failed to load blog posts");
            }
            finally
            {
                _viewModel.IsLoading = false;
            }
        }
    }
}
