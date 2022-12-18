using BlogReader.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace BlogReader.Stores
{
    public class BlogPostItemsStore : BaseStore
    {
        private readonly string _blogPostItemsFilePath = String.Empty;
        private readonly string _blogPostItemSourcesFilePath = String.Empty;
        private readonly ObservableCollection<BlogPostItemSource> _blogPostItemSources = new ObservableCollection<BlogPostItemSource>();
        private readonly ObservableCollection<BlogPostItem> _blogPostItems = new ObservableCollection<BlogPostItem>();

        public ObservableCollection<BlogPostItem> BlogPostItems => _blogPostItems;
        public ObservableCollection<BlogPostItemSource> BlogPostItemSources => _blogPostItemSources;

        public event EventHandler BlogPostItemSourcesChanged;
        public event EventHandler BlogPostItemsChanged;

        public event EventHandler OnException;

        public BlogPostItemsStore() : base()
        {
            _blogPostItemsFilePath = DataItemsFolderPath + @"\BlogPostItems.txt";
            _blogPostItemSourcesFilePath = DataItemsFolderPath + @"\BlogPostItemSources.txt";

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

            // Checks if BlogPostItemSources.txt exists
            if (File.Exists(_blogPostItemSourcesFilePath))
            {
                using (StreamReader sr = new StreamReader(_blogPostItemSourcesFilePath))
                {
                    string blogPostItemSourcesJson = sr.ReadToEnd();
                    if (!String.IsNullOrEmpty(blogPostItemSourcesJson))
                    {
                        _blogPostItemSources = JsonConvert.DeserializeObject<ObservableCollection<BlogPostItemSource>>(blogPostItemSourcesJson);
                    }
                }
            }
            else
            {
                // if not then create one
                using (FileStream fs = File.Create(_blogPostItemSourcesFilePath)) { }
            }

            FetchAllBlogPostSourceData();
        }

        public async void FetchAllBlogPostSourceData()
        {
            try
            {
                var client = new HttpClient();

                foreach (BlogPostItemSource source in _blogPostItemSources.Where(s => s.Active))
                {
                    var response = await client.GetAsync(source.SourceUrl);
                    var strResponse = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                string msg = "Failed to get blogs for one or all of the sources";
                OnException?.Invoke(msg, new ErrorEventArgs(ex));
            }
        }

        public void AddOrUpdateBlogPostItem(BlogPostItem blogPostItem)
        {
            var existingBlogPostItem = _blogPostItems.SingleOrDefault(b => b.Id == blogPostItem.Id);
            if (existingBlogPostItem == null)
            {
                blogPostItem.DateCreated = DateTime.Now;
                _blogPostItems.Add(blogPostItem);
            }
            else
            {
                BlogPostItem.Copy(blogPostItem, existingBlogPostItem);
                existingBlogPostItem.DateModified = DateTime.Now;
            }

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

        public BlogPostItemSource GetBlogItemSourceById(string id)
        {
            return _blogPostItemSources.SingleOrDefault(s => s.Id == id);
        }

        public void AddOrUpdateBlogItemSource(BlogPostItemSource itemSource)
        {
            var existingItemSource = _blogPostItemSources.SingleOrDefault(bs => bs.Id == itemSource.Id);
            if (existingItemSource == null)
            {
                itemSource.DateCreated = DateTime.Now;
                _blogPostItemSources.Add(itemSource);
            }
            else
            {
                BlogPostItemSource.Copy(itemSource, existingItemSource);
                existingItemSource.DateModified = DateTime.Now;
            }

            BlogPostItemSourcesChanged?.Invoke(itemSource, EventArgs.Empty);
        }

        public void RemoveBlogItemSource(string sourceId)
        {
            var itemSourceToRemove = _blogPostItemSources.SingleOrDefault(bs => bs.Id == sourceId);
            _blogPostItemSources.Remove(itemSourceToRemove);
            BlogPostItemSourcesChanged?.Invoke(itemSourceToRemove, EventArgs.Empty);
        }

        public override void Dispose()
        {
            SaveItemsToFile(_blogPostItemsFilePath, _blogPostItems.ToList());
            SaveItemsToFile(_blogPostItemSourcesFilePath, _blogPostItemSources.ToList());
            base.Dispose();
        }
    }
}