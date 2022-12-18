using BlogReader.Models;
using BlogReader.Models.Enums;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System;
using System.Windows;

namespace BlogReader.Commands.Blogs.BlogSources
{
    public class RemoveBlogSourceItemCommand : BaseCommand
    {
        private readonly BlogSourcesViewModel _viewModel;
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;

        public RemoveBlogSourceItemCommand(BlogSourcesViewModel viewModel,
            BlogPostItemsStore blogPostItemsStore,
            NotificationsStore notificationsStore)
        {
            _viewModel = viewModel;
            _blogPostItemsStore = blogPostItemsStore;
            _notificationsStore = notificationsStore;
        }

        public override void Execute(object parameter)
        {
            if (parameter == null || string.IsNullOrWhiteSpace(parameter.ToString())) return;

            try
            {
                string itemToRemoveId = parameter.ToString();
                MessageBoxResult answer = MessageBox.Show("Are you sure you want to delete this source?", "Delete Source", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (answer == MessageBoxResult.Yes)
                {
                    _viewModel.IsLoading = true;
                    _viewModel.IsItemsGridLoading = true;
                    _blogPostItemsStore.RemoveBlogItemSource(itemToRemoveId);
                    _viewModel.SelectedSourceItem = null;

                    var notification = new Notification(MessageType.Success, "Successfully deleted blog source");
                    _notificationsStore.AddNotification(notification);

                    base.Execute(parameter);
                }
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to delete blog source", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                _viewModel.IsLoading = false;
                _viewModel.IsItemsGridLoading = false;
            }
        }
    }
}
