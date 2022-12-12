using BlogReader.Models;
using BlogReader.Stores;
using System.Windows.Input;

namespace BlogReader.ViewModels
{
    public class BlogSourcesViewModel : BaseViewModel
    {
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;

        private BlogPostItemSource _newSourceItem;

        public BlogPostItemSource NewSourceItem
        {
            get { return _newSourceItem; }
            set
            {
                _newSourceItem = value;
                OnPropertyChanged(nameof(NewSourceItem));
            }
        }

        public ICommand AddOrUpdateBlogItemSourceCommand { get; }
        public ICommand RemoveBlogItemSourceCommand { get; }

        public BlogSourcesViewModel(NotificationsStore notificationsStore, BlogPostItemsStore blogPostItemsStore)
        {
            _blogPostItemsStore = blogPostItemsStore;
            _notificationsStore = notificationsStore;
        }
    }
}