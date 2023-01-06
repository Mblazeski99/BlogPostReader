using BlogReader.Commands;
using BlogReader.Commands.Blogs.BlogPostItemPreview;
using BlogReader.Models;
using BlogReader.Stores;

namespace BlogReader.ViewModels
{
    public class BlogPostItemPreviewViewModel : BaseViewModel
    {
        private readonly BlogPostItem _blogPostItem;

        public BlogPostItem BlogPostItem => _blogPostItem;

        public BaseCommand NavigateToContentCommand { get; }

        public BlogPostItemPreviewViewModel() { }

        public BlogPostItemPreviewViewModel(BlogPostItem blogPostItem,
            NotificationsStore notificationsStore)
        {
            _blogPostItem = blogPostItem;

            NavigateToContentCommand = new NavigateToContentCommand(this, notificationsStore);
        }
    }
}
