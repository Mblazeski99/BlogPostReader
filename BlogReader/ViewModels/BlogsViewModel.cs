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
            BlogsListingViewModel = new BlogsListingViewModel(notificationsStore, blogPostItemsStore);
            BlogSourcesViewModel = new BlogSourcesViewModel(notificationsStore, blogPostItemsStore);
            ContentModelsViewModel = new ContentModelsViewModel(notificationsStore, blogPostItemsStore);
        }

        public override void Dispose()
        {
            BlogsListingViewModel.Dispose();
            BlogSourcesViewModel.Dispose();
            ContentModelsViewModel.Dispose();
            base.Dispose();
        }
    }
}
