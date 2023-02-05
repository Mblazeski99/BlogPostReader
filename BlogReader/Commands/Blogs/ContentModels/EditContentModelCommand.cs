using BlogReader.DataModels.Enums;
using BlogReader.DataModels;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System;

namespace BlogReader.Commands.Blogs.ContentModels
{
    public class EditContentModelCommand : BaseCommand
    {
        private readonly ContentModelsViewModel _viewModel;
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;

        public EditContentModelCommand(ContentModelsViewModel viewModel, 
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
                _viewModel.IsLoading = true;
                _viewModel.IsItemsGridLoading = true;

                string itemToEditId = parameter.ToString();
                var itemToEdit = _blogPostItemsStore.GetRssContentModelById(itemToEditId);

                if (itemToEdit == null)
                {
                    throw new Exception($"Could not find RssContentModel with id: {itemToEditId}");
                }

                _viewModel.SelectedContentModel = RssContentModel.CreateNewCopy(itemToEdit);
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Could not find content model", ex.ToString());
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
