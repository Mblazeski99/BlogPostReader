using BlogReader.Commands.Blogs;
using BlogReader.Stores;
using System.Windows;
using System.Windows.Input;

namespace BlogReader.ViewModels
{
    public class BlogsViewModel : BaseViewModel
    {
        private readonly NotificationsStore _notificationsStore;
        private readonly BlogPostItemsStore _blogPostItemsStore;

        private Window _blogSourcesWindow;

        public ICommand ShowBlogSourcesViewCommand { get; }

        public BlogsViewModel(NotificationsStore notificationsStore, BlogPostItemsStore blogPostItemsStore)
        {
            _notificationsStore = notificationsStore;
            _blogPostItemsStore = blogPostItemsStore;

            ShowBlogSourcesViewCommand = new ShowBlogSourcesView(_blogSourcesWindow, notificationsStore, blogPostItemsStore);
        }
    }
}
