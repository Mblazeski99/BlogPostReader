using BlogReader.DataModels.Enums;
using BlogReader.DataModels;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System;

namespace BlogReader.Commands.Blogs.ContentModels
{
    public class InsertOrUpdateContentModelCommand : BaseCommand
    {
        private readonly ContentModelsViewModel _viewModel;
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;

        public InsertOrUpdateContentModelCommand(ContentModelsViewModel viewModel, 
            BlogPostItemsStore blogPostItemsStore, 
            NotificationsStore notificationsStore)
        {
            _viewModel = viewModel;
            _blogPostItemsStore = blogPostItemsStore;
            _notificationsStore = notificationsStore;
        }

        public override void Execute(object parameter)
        {
            if (_viewModel.SelectedContentModel == null) return;

            try
            {
                _viewModel.ClearDataPropertyErrors();
                if (string.IsNullOrEmpty(_viewModel.SelectedContentModel.ModelName))
                {
                    string errorMsg = "This field is required!";
                    _viewModel.AddDataPropertyError(nameof(_viewModel.SelectedContentModel.ModelName), errorMsg);
                }

                if (string.IsNullOrEmpty(_viewModel.SelectedContentModel.ItemContainerTag))
                {
                    string errorMsg = "This field is required!";
                    _viewModel.AddDataPropertyError(nameof(_viewModel.SelectedContentModel.ItemContainerTag), errorMsg);
                }

                if (string.IsNullOrEmpty(_viewModel.SelectedContentModel.TitleTag))
                {
                    string errorMsg = "This field is required!";
                    _viewModel.AddDataPropertyError(nameof(_viewModel.SelectedContentModel.TitleTag), errorMsg);
                }

                if (string.IsNullOrEmpty(_viewModel.SelectedContentModel.DateTag))
                {
                    string errorMsg = "This field is required!";
                    _viewModel.AddDataPropertyError(nameof(_viewModel.SelectedContentModel.DateTag), errorMsg);
                }

                if (string.IsNullOrEmpty(_viewModel.SelectedContentModel.ContentTag))
                {
                    string errorMsg = "This field is required!";
                    _viewModel.AddDataPropertyError(nameof(_viewModel.SelectedContentModel.ContentTag), errorMsg);
                }

                if (_viewModel.HasDataPropertyErrors()) return;

                _blogPostItemsStore.AddOrUpdateRssContentModel(_viewModel.SelectedContentModel);

                var notification = new Notification(MessageType.Success, "Successfully added/edited content model");
                _notificationsStore.AddNotification(notification);

                base.Execute(parameter);
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to add/update content model", ex.ToString());
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
