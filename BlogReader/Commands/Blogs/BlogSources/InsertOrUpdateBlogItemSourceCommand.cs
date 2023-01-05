using BlogReader.Models.Enums;
using BlogReader.Models;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System;

namespace BlogReader.Commands.Blogs.BlogSources
{
    public class InsertOrUpdateBlogItemSourceCommand : BaseCommand
    {
        private readonly BlogSourcesViewModel _viewModel;
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;

        public InsertOrUpdateBlogItemSourceCommand(BlogSourcesViewModel viewModel,
            BlogPostItemsStore blogPostItemsStore,
            NotificationsStore notificationsStore)
        {
            _viewModel = viewModel;
            _blogPostItemsStore = blogPostItemsStore;
            _notificationsStore = notificationsStore;
        }

        public override void Execute(object parameter)
        {
            if (_viewModel.SelectedSourceItem == null) return;

            try
            {
                _viewModel.ClearDataPropertyErrors();
                if (string.IsNullOrEmpty(_viewModel.SelectedSourceItem.SourceName))
                {
                    string errorMsg = "This field is required!";
                    _viewModel.AddDataPropertyError(nameof(_viewModel.SelectedSourceItem.SourceName), errorMsg);
                }

                if (string.IsNullOrEmpty(_viewModel.SelectedSourceItem.SourceUrl))
                {
                    string errorMsg = "This field is required!";
                    _viewModel.AddDataPropertyError(nameof(_viewModel.SelectedSourceItem.SourceUrl), errorMsg);
                }
                else if (_viewModel.SelectedSourceItem.SourceUrl.IsValidUri() == false)
                {
                    string errorMsg = "Not a valid url!";
                    _viewModel.AddDataPropertyError(nameof(_viewModel.SelectedSourceItem.SourceUrl), errorMsg);
                }

                if (_viewModel.SelectedSourceItem.ContentModel == null)
                {
                    string errorMsg = "This field is required!";
                    _viewModel.AddDataPropertyError(nameof(_viewModel.SelectedSourceItem.ContentModel), errorMsg);
                }

                if (_viewModel.HasDataPropertyErrors()) return;

                string imgName = $"BlogSource_{_viewModel.SelectedSourceItem.Id}";
                _viewModel.SelectedSourceItem.ImageName = imgName;
                _viewModel.SelectedSourceItem.ImageSource = _viewModel.SourceImg;

                _blogPostItemsStore.AddOrUpdateBlogItemSource(_viewModel.SelectedSourceItem);

                var notification = new Notification(MessageType.Success, "Successfully added/edited blog source");
                _notificationsStore.AddNotification(notification);

                base.Execute(parameter);
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to add/update blog source", ex.ToString());
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
