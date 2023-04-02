using BlogReader.DataModels.Interfaces;
using BlogReader.Helpers;
using System;

namespace BlogReader.DataModels
{
    public class BlogPostItem : BaseEntity, IBaseEntity
    {
        private string _title;
        private string _summary;
        private string _content;
        private DateTime? _date;
        private string _imageLink;
        private string _link;
        private string _sourceId;
        private string _sourceName;
        private string _author;
        private bool _markedAsWatchLater;
        private bool _isVisited;
        private bool _markedAsIrrelevant;
        private bool _isDeleted;

        public string Title 
        { 
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Summary
        { 
            get { return _summary; }
            set
            {
                _summary = value;
                OnPropertyChanged(nameof(Summary));
            }
        }

        public string Content
        { 
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public DateTime? Date
        { 
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public string ImageLink
        { 
            get { return _imageLink; }
            set
            {
                _imageLink = value;
                OnPropertyChanged(nameof(ImageLink));
            }
        }

        public string Link
        { 
            get { return _link; }
            set
            {
                _link = value;
                OnPropertyChanged(nameof(Link));
            }
        }

        public string SourceId
        { 
            get { return _sourceId; }
            set
            {
                _sourceId = value;
                OnPropertyChanged(nameof(SourceId));
            }
        }

        public string SourceName
        { 
            get { return _sourceName; }
            set
            {
                _sourceName = value;
                OnPropertyChanged(nameof(SourceName));
            }
        }

        public string Author
        { 
            get { return _author; }
            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        public bool MarkedAsWatchLater
        { 
            get { return _markedAsWatchLater; }
            set
            {
                _markedAsWatchLater = value;
                OnPropertyChanged(nameof(MarkedAsWatchLater));
            }
        }

        public bool IsVisited
        { 
            get { return _isVisited; }
            set
            {
                _isVisited = value;
                OnPropertyChanged(nameof(IsVisited));
            }
        }

        public bool MarkedAsIrrelevant
        { 
            get { return _markedAsIrrelevant; }
            set
            {
                _markedAsIrrelevant = value;
                OnPropertyChanged(nameof(MarkedAsIrrelevant));
            }
        }

        public bool IsDeleted
        { 
            get { return _isDeleted; }
            set
            {
                _isDeleted = value;
                OnPropertyChanged(nameof(IsDeleted));
            }
        }

        public static void Copy(BlogPostItem copyFrom, BlogPostItem copyTo)
        {
            PropertyCopier<BlogPostItem, BlogPostItem>.Copy(copyFrom, copyTo);
        }

        public static BlogPostItem CreateNewCopy(BlogPostItem item)
        {
            var newItem = new BlogPostItem();
            Copy(item, newItem);
            return newItem;
        }
    }
}
