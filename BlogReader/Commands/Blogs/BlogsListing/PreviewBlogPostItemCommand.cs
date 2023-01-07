using BlogReader.Models;
using BlogReader.Models.Enums;
using BlogReader.Stores;
using BlogReader.ViewModels;
using BlogReader.Views;
using System;
using System.Windows;

namespace BlogReader.Commands.Blogs.BlogsListing
{
    public class PreviewBlogPostItemCommand : BaseCommand
    {
        private Window _previewBlogPostWindow;
        private readonly NotificationsStore _notificationsStore;
        private readonly BlogPostItemsStore _blogPostItemsStore;

        public PreviewBlogPostItemCommand(NotificationsStore notificationsStore,
            BlogPostItemsStore blogPostItemsStore)
        {
            _notificationsStore = notificationsStore;
            _blogPostItemsStore = blogPostItemsStore;
        }

        public override async void Execute(object parameter)
        {
            try
            {
                if (parameter == null || parameter.ToString() == string.Empty) 
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                if (_previewBlogPostWindow != null) _previewBlogPostWindow.Close();

                var selectedBlogPostItem = _blogPostItemsStore.GetBlogPostItemById(parameter.ToString());
                var viewModel = new BlogPostItemPreviewViewModel(selectedBlogPostItem, _notificationsStore);
                var view = new BlogPostItemPreviewView();
                view.DataContext = viewModel;

                _previewBlogPostWindow = new Window();
                _previewBlogPostWindow.WindowState = WindowState.Maximized;
                _previewBlogPostWindow.Title = selectedBlogPostItem.Title;

                var itemSource = _blogPostItemsStore.GetBlogItemSourceById(selectedBlogPostItem.SourceId);
                _previewBlogPostWindow.Icon = itemSource?.ImageSource;

                _previewBlogPostWindow.Content = view;

                _previewBlogPostWindow.Show();
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to get blog post", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
        }
    }
}
