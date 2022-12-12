using BlogReader.Helpers;

namespace BlogReader.Models
{
    public class BlogPostItemSource : BaseEntity
    {
        private string _sourceName;
        private string _sourceUrl;
        private string _imageUrl;
        private bool _active;

        public string SourceName 
        {
            get { return _sourceName; }
            set 
            { 
                _sourceName = value; 
                OnPropertyChanged(nameof(SourceName));
            }
        }

        public string SourceUrl
        {
            get { return _sourceUrl; }
            set 
            {
                _sourceUrl = value; 
                OnPropertyChanged(nameof(SourceUrl));
            }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set 
            {
                _imageUrl = value; 
                OnPropertyChanged(nameof(ImageUrl));
            }
        }

        public bool Active
        {
            get { return _active; }
            set 
            {
                _active = value; 
                OnPropertyChanged(nameof(Active));
            }
        }

        public static void Copy(BlogPostItemSource copyFrom, BlogPostItemSource copyTo)
        {
            PropertyCopier<BlogPostItemSource, BlogPostItemSource>.Copy(copyFrom, copyTo);
        }
    }
}
