using BlogReader.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows.Media.Imaging;

namespace BlogReader.Stores
{
    public class BlogPostItemsStore : BaseStore
    {
        private readonly string _blogPostItemsFilePath = String.Empty;
        private readonly string _blogPostItemSourcesFilePath = String.Empty;
        private readonly string _blogPostItemSourceUploadsFolderPath = String.Empty;
        private readonly ObservableCollection<BlogPostItemSource> _blogPostItemSources = new ObservableCollection<BlogPostItemSource>();
        private readonly ObservableCollection<BlogPostItem> _blogPostItems = new ObservableCollection<BlogPostItem>();

        public ObservableCollection<BlogPostItem> BlogPostItems => _blogPostItems;

        public event EventHandler BlogPostItemSourcesChanged;
        public event EventHandler BlogPostItemsChanged;

        public event EventHandler OnException;

        public BlogPostItemsStore() : base()
        {
            _blogPostItemsFilePath = DataItemsFolderPath + @"\BlogPostItems.txt";
            _blogPostItemSourcesFilePath = DataItemsFolderPath + @"\BlogPostItemSources.txt";
            _blogPostItemSourceUploadsFolderPath = DataItemsFolderPath + @"\BlogPostItemSourceUploads";

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
                using (FileStream fs = File.Create(_blogPostItemsFilePath)) { }
            }

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
                using (FileStream fs = File.Create(_blogPostItemSourcesFilePath)) { }
            }

            if (Directory.Exists(_blogPostItemSourceUploadsFolderPath) == false)
            {
                Directory.CreateDirectory(_blogPostItemSourceUploadsFolderPath);
            }

            FetchAllBlogPostSourceData();
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
            string sourceImagePath = new Uri($@"{_blogPostItemSourceUploadsFolderPath}\{itemSource.ImageName}_{DateTime.Now.Ticks}.png")
                .ToString()
                .Replace("file:///", String.Empty)
                .Replace(@"//", @"/");

            var existingItemSource = _blogPostItemSources.SingleOrDefault(bs => bs.Id == itemSource.Id);
            if (existingItemSource == null)
            {
                itemSource.DateCreated = DateTime.Now;
                itemSource.ImagePath = sourceImagePath;

                _blogPostItemSources.Add(itemSource);
            }
            else
            {
                File.Delete(existingItemSource.ImagePath);

                BlogPostItemSource.Copy(itemSource, existingItemSource);
                existingItemSource.ImagePath = sourceImagePath;
                existingItemSource.DateModified = DateTime.Now;
            }

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(itemSource.ImageSource));
            
            using (var fileStream = new FileStream(sourceImagePath, FileMode.Create, FileAccess.Write))
            {
                encoder.Save(fileStream);
                fileStream.Close();
            }

            SaveBlogSourcesToFile();
            BlogPostItemSourcesChanged?.Invoke(itemSource, EventArgs.Empty);
        }

        public void RemoveBlogItemSource(string sourceId)
        {
            var itemSourceToRemove = _blogPostItemSources.SingleOrDefault(bs => bs.Id == sourceId);

            _blogPostItemSources.Remove(itemSourceToRemove);
            BlogPostItemSourcesChanged?.Invoke(itemSourceToRemove, EventArgs.Empty);

            File.Delete(itemSourceToRemove.ImagePath);
        }

        public ObservableCollection<BlogPostItemSource> GetAllBlogPostItemSources()
        {
            foreach (var sourceItem in _blogPostItemSources)
            {
                var sourceImage = new BitmapImage();
                sourceImage.BeginInit();
                sourceImage.CacheOption = BitmapCacheOption.OnLoad;
                sourceImage.UriSource = new Uri(sourceItem.ImagePath);
                sourceImage.EndInit();

                sourceItem.ImageSource = sourceImage;
            }

            return _blogPostItemSources;
        }

        public override void Dispose()
        {
            SaveItemsToFile(_blogPostItemsFilePath, _blogPostItems.ToList());
            SaveBlogSourcesToFile();
            base.Dispose();
        }

        private void FetchAllBlogPostSourceData()
        {
            var client = new HttpClient();

            foreach (BlogPostItemSource source in _blogPostItemSources.Where(s => s.Active))
            {
                FetchBlogPostSourceData(source);
            }
        }

        private async void FetchBlogPostSourceData(BlogPostItemSource source)
        {
            try
            {
                var client = new HttpClient();

                var response = await client.GetAsync(source.SourceUrl);
                var strResponse = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                string msg = $"Failed to get blogs for: '{source.SourceName}'";
                OnException?.Invoke(msg, new ErrorEventArgs(ex));
            }
        }

        private void SaveBlogSourcesToFile()
        {
            foreach (var sourceItem in _blogPostItemSources)
            {
                sourceItem.ImageSource = null;
            }

            SaveItemsToFile(_blogPostItemSourcesFilePath, _blogPostItemSources.ToList());
        }
    }
}