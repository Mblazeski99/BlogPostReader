using BlogReader.Models.Enums;
using BlogReader.Models;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System.Windows;
using System;

namespace BlogReader.Commands.Blogs.BlogsListing
{
    public class RemoveBlogPostItemCommand : BaseCommand
    {
        private readonly NotificationsStore _notificationsStore;
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly BlogsListingViewModel _viewModel;

        public RemoveBlogPostItemCommand(BlogsListingViewModel viewModel,
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
                if (parameter == null || parameter.ToString() == string.Empty)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                MessageBoxResult answer = MessageBox.Show("Are you sure you want to delete this blog post?", "Delete Blog Post", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (answer == MessageBoxResult.Yes)
                {
                    _viewModel.IsLoading = true;
                    _blogPostItemsStore.RemoveBlogPostItem(parameter.ToString());
                }
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to delete blog post", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                _viewModel.IsLoading = false;
            }
        }
    }
}
