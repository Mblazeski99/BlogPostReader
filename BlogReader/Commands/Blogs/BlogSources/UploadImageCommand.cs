using BlogReader.Models.Enums;
using BlogReader.Models;
using BlogReader.Stores;
using BlogReader.ViewModels;
using Microsoft.Win32;
using System;
using System.Windows.Media.Imaging;

namespace BlogReader.Commands.Blogs.BlogSources
{
    public class UploadImageCommand : BaseCommand
    {
        private readonly BlogSourcesViewModel _viewModel;
        private readonly NotificationsStore _notificationsStore;

        public UploadImageCommand(BlogSourcesViewModel viewModel,
            NotificationsStore notificationsStore)
        {
            _viewModel = viewModel;
            _notificationsStore = notificationsStore;
        }

        public override void Execute(object parameter)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Select a picture";
                op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png";

                if (op.ShowDialog() == true) 
                {
                    _viewModel.IsLoading = true;

                    var bitmap = new BitmapImage();

                    bitmap.BeginInit();
                    bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(op.FileName);
                    bitmap.EndInit();

                    _viewModel.SourceImg = bitmap;
                }
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to upload image", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                _viewModel.IsLoading = false;
            }
        }
    }
}
