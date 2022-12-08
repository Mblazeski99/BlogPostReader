using BlogReader.Models;
using BlogReader.Services;
using BlogReader.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace BlogReader.Stores
{
    public class BlogPostItemsStore : BaseStore
    {
        private readonly IBlogPostItemService _blogPostItemService;
        private readonly string _blogPostItemsFilePath = String.Empty;
        private readonly ObservableCollection<BlogPostItem> _blogPostItems;

        public ObservableCollection<BlogPostItem> BlogPostItems => _blogPostItems;
        public event EventHandler BlogPostItemsChanged;

        public BlogPostItemsStore(IBlogPostItemService blogPostItemService) : base()
        {
            _blogPostItemService = blogPostItemService;

            _blogPostItemsFilePath = DataItemsFolderPath + @"\BlogPostItems.txt";
            _blogPostItems = new ObservableCollection<BlogPostItem>();

            // Checks if BlogPostItems.txt exists
            if (File.Exists(_blogPostItemsFilePath))
            {
                using (StreamReader sr = new StreamReader(_blogPostItemsFilePath))
                {
                    string blogPostItemsJson = sr.ReadToEnd();
                    if (!String.IsNullOrEmpty(blogPostItemsJson))
                    {
                        _blogPostItems = JsonConvert.DeserializeObject<ObservableCollection<BlogPostItem>>(blogPostItemsJson);
                    }
                }
            }
            else
            {
                // if not then create one
                using (FileStream fs = File.Create(_blogPostItemsFilePath)) { }
            }
        }

        public void AddBlogPostItem(BlogPostItem blogPostItem)
        {
            blogPostItem.DateCreated = DateTime.Now;
            _blogPostItems.Add(blogPostItem);
            BlogPostItemsChanged?.Invoke(_blogPostItems, EventArgs.Empty);
        }

        public void DeleteBlogPostItem(BlogPostItem blogPostItem)
        {
            _blogPostItems.Remove(blogPostItem);
            BlogPostItemsChanged?.Invoke(_blogPostItems, EventArgs.Empty);
        }

        public void ClearBlogPostItems()
        {
            _blogPostItems.Clear();
            BlogPostItemsChanged?.Invoke(_blogPostItems, EventArgs.Empty);
        }

        public override void Dispose()
        {
            SaveItemsToFile(_blogPostItemsFilePath, _blogPostItems.ToList());
            base.Dispose();
        }
    }
}
