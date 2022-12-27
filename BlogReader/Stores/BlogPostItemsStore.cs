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
        private readonly string _rssContentModelsFilePath = String.Empty;

        private readonly ObservableCollection<BlogPostItemSource> _blogPostItemSources = new ObservableCollection<BlogPostItemSource>();
        private readonly ObservableCollection<BlogPostItem> _blogPostItems = new ObservableCollection<BlogPostItem>();
        private readonly ObservableCollection<RssContentModel> _rssContentModels = new ObservableCollection<RssContentModel>();

        public ObservableCollection<BlogPostItem> BlogPostItems => _blogPostItems;

        public event EventHandler BlogPostItemSourcesChanged;
        public event EventHandler BlogPostItemsChanged;
        public event EventHandler RssContentModelsChanged;
        public event EventHandler OnException;

        public BlogPostItemsStore() : base()
        {
            _blogPostItemsFilePath = DataItemsFolderPath + @"\BlogPostItems.txt";
            _blogPostItemSourcesFilePath = DataItemsFolderPath + @"\BlogPostItemSources.txt";
            _blogPostItemSourceUploadsFolderPath = DataItemsFolderPath + @"\BlogPostItemSourceUploads";
            _rssContentModelsFilePath = DataItemsFolderPath + @"\RssContentModels.txt";

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

            if (File.Exists(_rssContentModelsFilePath))
            {
                using (StreamReader sr = new StreamReader(_rssContentModelsFilePath))
                {
                    string rssContentModelsJson = sr.ReadToEnd();
                    if (!String.IsNullOrEmpty(rssContentModelsJson))
                    {
                        _rssContentModels = JsonConvert.DeserializeObject<ObservableCollection<RssContentModel>>(rssContentModelsJson);
                    }
                }
            }
            else
            {
                using (FileStream fs = File.Create(_rssContentModelsFilePath)) { }
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

        public void RemoveBlogPostItem(string blogPostItemId)
        {
            var itemToRemove = _blogPostItems.SingleOrDefault(bpi => bpi.Id == blogPostItemId);
            _blogPostItems.Remove(itemToRemove);
            BlogPostItemsChanged?.Invoke(_blogPostItems, EventArgs.Empty);
        }

        public BlogPostItemSource GetBlogItemSourceById(string id)
        {
            return _blogPostItemSources.SingleOrDefault(s => s.Id == id);
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

        public void AddOrUpdateRssContentModel(RssContentModel model)
        {
            var existingModel = _rssContentModels.SingleOrDefault(m => m.Id == model.Id);
            if (existingModel == null)
            {
                model.DateCreated = DateTime.Now;
                _rssContentModels.Add(model);
            }
            else
            {
                RssContentModel.Copy(model, existingModel);
                existingModel.DateModified = DateTime.Now;

                // Update all sources which use the model
                foreach (var source in _blogPostItemSources)
                {
                    if (source.ContentModel != null && source.ContentModel.Id == existingModel.Id)
                    {
                        RssContentModel.Copy(existingModel, source.ContentModel);
                    }
                }

                SaveBlogSourcesToFile();
            }

            RssContentModelsChanged?.Invoke(_rssContentModels, EventArgs.Empty);
            BlogPostItemSourcesChanged?.Invoke(_blogPostItemSources, EventArgs.Empty);
        }

        public void RemoveRssContentModel(string modelId)
        {
            var modelToRemove = _rssContentModels.SingleOrDefault(m => m.Id == modelId);

            _rssContentModels.Remove(modelToRemove);

            // Remove the deleted models from sources that used it
            foreach (var source in _blogPostItemSources)
            {
                if (source.ContentModel != null && source.ContentModel.Id == modelToRemove.Id)
                {
                    source.ContentModel = null;
                }
            }

            SaveBlogSourcesToFile();

            RssContentModelsChanged?.Invoke(modelToRemove, EventArgs.Empty);
            BlogPostItemSourcesChanged?.Invoke(_blogPostItemSources, EventArgs.Empty);
        }

        public override void Dispose()
        {
            SaveItemsToFile(_blogPostItemsFilePath, _blogPostItems.ToList());
            SaveBlogSourcesToFile();
            SaveItemsToFile(_rssContentModelsFilePath, _rssContentModels.ToList());
            base.Dispose();
        }

        private void FetchAllBlogPostSourceData()
        {
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