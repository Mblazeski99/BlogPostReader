using BlogReader.Models.Enums;
using BlogReader.Models;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System;
using Microsoft.Web.WebView2.Wpf;
using System.Threading.Tasks;

namespace BlogReader.Commands.Blogs.BlogPostItemPreview
{
    public class NavigateToContentCommand : BaseCommand
    {
        private readonly BlogPostItemPreviewViewModel _viewModel;
        private readonly NotificationsStore _notificationsStore;

        public NavigateToContentCommand(BlogPostItemPreviewViewModel viewModel, 
            NotificationsStore notificationsStore)
        {
            _viewModel = viewModel;
            _notificationsStore = notificationsStore;
        }

        public override async void Execute(object parameter)
        {
            try
            {
                if (parameter == null) throw new ArgumentNullException(nameof(parameter));

                WebView2 webViewControl = (parameter as WebView2);
                await webViewControl.EnsureCoreWebView2Async();
                webViewControl.NavigateToString(_viewModel.BlogPostItem.Content);
            }
            catch (Exception ex)
            {   
                var error = new Notification(MessageType.Error, "Failed to load blog post", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
        }
    }
}
