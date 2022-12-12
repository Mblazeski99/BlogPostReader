using BlogReader.Helpers;
using System;

namespace BlogReader.Models
{
    public class BlogPostItem : BaseEntity
    {
        private string _title;
        private string _content;
        private DateTime _date;
        private string _imageLink;
        private string _link;
        private string _source;
        private string _author;
        private bool _markedAsWatchLater;
        private bool _isVisited;
        private bool _markedAsIrrelevant;

        public string Title 
        { 
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
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

        public DateTime Date
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

        public string Source
        { 
            get { return _source; }
            set
            {
                _source = value;
                OnPropertyChanged(nameof(Source));
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

        public static void Copy(BlogPostItem copyFrom, BlogPostItem copyTo)
        {
            PropertyCopier<BlogPostItem, BlogPostItem>.Copy(copyFrom, copyTo);
        }
    }
}
