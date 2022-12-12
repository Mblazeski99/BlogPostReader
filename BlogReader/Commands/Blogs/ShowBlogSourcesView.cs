using BlogReader.Stores;
using BlogReader.ViewModels;
using BlogReader.Views;
using System.Windows;

namespace BlogReader.Commands.Blogs
{
    public class ShowBlogSourcesView : BaseCommand
    {
        private Window _blogPostSourcesWindow;
        private readonly NotificationsStore _notificationsStore;
        private readonly BlogPostItemsStore _blogPostItemsStore;

        public ShowBlogSourcesView(Window blogPostSourcesWindow, 
            NotificationsStore notificationsStore, 
            BlogPostItemsStore blogPostItemsStore)
        {
            _blogPostSourcesWindow = blogPostSourcesWindow;
            _notificationsStore = notificationsStore;
            _blogPostItemsStore = blogPostItemsStore;
        }

        public override void Execute(object parameter)
        {
            // checks if blog sources page is already opened
            if (_blogPostSourcesWindow == null || _blogPostSourcesWindow.Visibility == Visibility.Collapsed) 
            {
                _blogPostSourcesWindow = new Window();
                var content = new BlogSourcesView();
                content.DataContext = new BlogSourcesViewModel(_notificationsStore, _blogPostItemsStore);

                _blogPostSourcesWindow.Height = 450;
                _blogPostSourcesWindow.Width = 800;
                _blogPostSourcesWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                _blogPostSourcesWindow.Title = "Blog Sources";
                _blogPostSourcesWindow.Show();
                _blogPostSourcesWindow.Content = content;

                _blogPostSourcesWindow.Closing += (sender, args) =>
                {
                    _blogPostSourcesWindow.Visibility = Visibility.Collapsed;
                };
            }
        }
    }
}