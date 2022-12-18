using BlogReader.Stores;
using BlogReader.ViewModels;
using BlogReader.Views;
using System.Windows;

namespace BlogReader.Commands.Blogs
{
    public class ShowBlogSourcesViewCommand : BaseCommand
    {
        private Window _blogPostSourcesWindow;
        private readonly NotificationsStore _notificationsStore;
        private readonly BlogPostItemsStore _blogPostItemsStore;

        public ShowBlogSourcesViewCommand(NotificationsStore notificationsStore, 
            BlogPostItemsStore blogPostItemsStore)
        {
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

                var contentViewModel = new BlogSourcesViewModel(_notificationsStore, _blogPostItemsStore);
                contentViewModel.ItemRemoved += () =>
                {
                    _blogPostSourcesWindow.Topmost = true;
                    _blogPostSourcesWindow.Topmost = false;
                };

                content.DataContext = contentViewModel;

                _blogPostSourcesWindow.Height = (SystemParameters.PrimaryScreenHeight * 0.80);
                _blogPostSourcesWindow.Width = (SystemParameters.PrimaryScreenWidth * 0.80);
                _blogPostSourcesWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                _blogPostSourcesWindow.Title = "Blog Sources";
                _blogPostSourcesWindow.Show();
                _blogPostSourcesWindow.Content = content;

                _blogPostSourcesWindow.Closing += (sender, args) =>
                {
                    _blogPostSourcesWindow.Visibility = Visibility.Collapsed;
                };
            }
            else
            {
                _blogPostSourcesWindow.Focus();
            }
        }

        public override void Dispose()
        {
            _blogPostSourcesWindow?.Close();
            base.Dispose();
        }
    }
}