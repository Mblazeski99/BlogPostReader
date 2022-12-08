using BlogReader.Stores;

namespace BlogReader.ViewModels
{
    public class BlogsViewModel : BaseViewModel
    {
        private readonly NotificationsStore _notificationsStore;
        private readonly BlogPostItemsStore _blogPostItemsStore;

        public BlogsViewModel(NotificationsStore notificationsStore, BlogPostItemsStore blogPostItemsStore)
        {
            _notificationsStore = notificationsStore;
            _blogPostItemsStore = blogPostItemsStore;
        }
    }
}
