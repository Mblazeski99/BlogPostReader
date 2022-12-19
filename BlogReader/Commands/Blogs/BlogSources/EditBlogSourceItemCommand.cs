using BlogReader.Models.Enums;
using BlogReader.Models;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System;
using System.Windows.Media.Imaging;

namespace BlogReader.Commands.Blogs.BlogSources
{
    public class EditBlogSourceItemCommand : BaseCommand
    {
        private readonly BlogSourcesViewModel _viewModel;
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;

        public EditBlogSourceItemCommand(BlogSourcesViewModel viewModel,
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
                var itemToEdit = _blogPostItemsStore.GetBlogItemSourceById(itemToEditId);

                if (itemToEdit == null) 
                {
                    throw new Exception($"Could not find BlogPostItemSource with id: {itemToEditId}");
                }

                _viewModel.SelectedSourceItem = BlogPostItemSource.CreateNewCopy(itemToEdit);

                var sourceImage = new BitmapImage();
                sourceImage.BeginInit();
                sourceImage.CacheOption = BitmapCacheOption.OnLoad;
                sourceImage.UriSource = new Uri(itemToEdit.ImagePath);
                sourceImage.EndInit();

                _viewModel.SourceImg = sourceImage;
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Could not find blog source", ex.ToString());
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
