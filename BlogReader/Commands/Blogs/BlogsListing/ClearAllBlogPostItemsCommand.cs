using BlogReader.DataModels.Enums;
using BlogReader.DataModels;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System.Windows;
using System;

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
                MessageBoxResult answer = MessageBox.Show("Are you sure you want to clear these blogs?", "Clear Blogs", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (answer == MessageBoxResult.Yes)
                {
                    _viewModel.IsLoading = true;
                    _blogPostItemsStore.ClearBlogPostItems();
                }
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to load blog posts", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                _viewModel.IsLoading = false;
            }
        }
    }
}
