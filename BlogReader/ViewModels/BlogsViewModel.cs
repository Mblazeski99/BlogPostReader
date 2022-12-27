using BlogReader.Stores;

namespace BlogReader.ViewModels
{
    public class BlogsViewModel : BaseViewModel
    {
        public BlogsListingViewModel BlogsListingViewModel { get; }
        public BlogSourcesViewModel BlogSourcesViewModel { get; }
        public ContentModelsViewModel ContentModelsViewModel { get; }

        public BlogsViewModel(NotificationsStore notificationsStore, BlogPostItemsStore blogPostItemsStore)
        {
            BlogsListingViewModel = new BlogsListingViewModel();
            BlogSourcesViewModel = new BlogSourcesViewModel(notificationsStore, blogPostItemsStore);
            ContentModelsViewModel = new ContentModelsViewModel();
        }
    }
}
