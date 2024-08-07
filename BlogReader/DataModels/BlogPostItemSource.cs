﻿using BlogReader.Helpers;
using System.Windows.Media.Imaging;

namespace BlogReader.DataModels
{
    public class BlogPostItemSource : BaseEntity
    {
        private string _sourceName;
        private string _sourceUrl;
        private string _imageName;
        private string _imagePath;
        private BitmapSource _imageSource;
        private bool _active;
        private string _contentModelId;

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

        public string ImageName
        {
            get { return _imageName; }
            set 
            {
                _imageName = value; 
                OnPropertyChanged(nameof(ImageName));
            }
        }

        public string ImagePath
        {
            get { return _imagePath; }
            set 
            {
                _imagePath = value; 
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        public BitmapSource ImageSource
        {
            get { return _imageSource; }
            set 
            {
                _imageSource = value; 
                OnPropertyChanged(nameof(ImageSource));
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

        public string ContentModelId
        {
            get { return _contentModelId; }
            set 
            {
                _contentModelId = value; 
                OnPropertyChanged(nameof(ContentModelId));
            }
        }

        public static void Copy(BlogPostItemSource copyFrom, BlogPostItemSource copyTo)
        {
            PropertyCopier<BlogPostItemSource, BlogPostItemSource>.Copy(copyFrom, copyTo);
        }

        public static BlogPostItemSource CreateNewCopy(BlogPostItemSource source)
        {
            var newSource = new BlogPostItemSource();
            Copy(source, newSource);
            return newSource;
        }
    }
}
