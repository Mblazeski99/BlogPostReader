using BlogReader.DataModels.Enums;
using BlogReader.DataModels;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System;
using BlogReader.CustomControls;
using BlogReader.Enums;

namespace BlogReader.Commands.Blogs.ContentModels
{
    public class RemoveContentModelCommand : BaseCommand
    {
        private readonly ContentModelsViewModel _viewModel;
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;

        public RemoveContentModelCommand(ContentModelsViewModel viewModel, 
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

                // TODO: If model is being used by a source add warning.
                string msg = "Are you sure you want to delete this content model?";
                ExtendedMessageBoxResult answer = ExtendedMessageBox.Show(msg, "Delete Content Model", ExtendedMessageBoxButton.YesNo, ExtendedMessageBoxImage.Question);

                if (answer == ExtendedMessageBoxResult.Yes)
                {
                    _viewModel.IsItemsGridLoading = true;

                    if (_viewModel.SelectedContentModel?.Id == itemToRemoveId)
                    {
                        _viewModel.SelectedContentModel = null;
                    }

                    _blogPostItemsStore.RemoveRssContentModel(itemToRemoveId);

                    var notification = new Notification(MessageType.Success, "Successfully deleted content model");
                    _notificationsStore.AddNotification(notification);

                    base.Execute(parameter);
                }
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to delete content model", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                _viewModel.IsItemsGridLoading = false;
            }
        }
    }
}
