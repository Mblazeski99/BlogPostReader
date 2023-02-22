using BlogReader.DataModels.Enums;
using BlogReader.DataModels;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System;
using BlogReader.CustomControls;
using BlogReader.Enums;
using System.Linq;
using System.Runtime.CompilerServices;

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

                string msg = "Are you sure you want to delete this content model?";
                ExtendedMessageBoxResult answer = ExtendedMessageBox.Show(msg, "Delete Content Model", ExtendedMessageBoxButton.YesNo, ExtendedMessageBoxImage.Question);

                if (answer == ExtendedMessageBoxResult.Yes)
                {
                    _viewModel.IsItemsGridLoading = true;

                    var sources = _blogPostItemsStore.GetAllBlogPostItemSources().Where(s => s.ContentModel.Id == itemToRemoveId);
                    if (sources.Any())
                    {
                        msg = $"This content model is currently being used by the sources: {string.Join(",", sources.Select(s => s.SourceName))}";
                        answer = ExtendedMessageBox.Show(msg, "Content Model Currently Used", ExtendedMessageBoxButton.OKCancel, ExtendedMessageBoxImage.Warning);
                        if (answer == ExtendedMessageBoxResult.Cancel) return;
                    }

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
