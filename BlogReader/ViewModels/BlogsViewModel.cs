using BlogReader.Commands;
using BlogReader.Commands.Blogs;
using BlogReader.Stores;

namespace BlogReader.ViewModels
{
    public class BlogsViewModel : BaseViewModel
    {
        private readonly NotificationsStore _notificationsStore;
        private readonly BlogPostItemsStore _blogPostItemsStore;

        public BaseCommand ShowBlogSourcesViewCommand { get; }

        public BlogsViewModel(NotificationsStore notificationsStore, BlogPostItemsStore blogPostItemsStore)
        {
            _notificationsStore = notificationsStore;
            _blogPostItemsStore = blogPostItemsStore;

            ShowBlogSourcesViewCommand = new ShowBlogSourcesViewCommand(notificationsStore, blogPostItemsStore);
        }

        public override void Dispose()
        {
            ShowBlogSourcesViewCommand.Dispose();
            base.Dispose();
        }
    }
}
