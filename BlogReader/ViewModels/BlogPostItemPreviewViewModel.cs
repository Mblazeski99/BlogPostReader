using BlogReader.Models;
using BlogReader.Models.Enums;
using BlogReader.Stores;
using System;
using System.IO;

namespace BlogReader.ViewModels
{
    public class BlogPostItemPreviewViewModel : BaseViewModel
    {
        private readonly NotificationsStore _notificationsStore;

        private BlogPostItem _blogPostItem;
        private string _htmlContent = string.Empty;

        public string HtmlContent => _htmlContent;

        public BlogPostItemPreviewViewModel() { }

        public BlogPostItemPreviewViewModel(BlogPostItem blogPostItem,
            NotificationsStore notificationsStore)
        {
            _notificationsStore = notificationsStore;
            RenderBlogPostItem(blogPostItem);
        }

        public void RenderBlogPostItem(BlogPostItem blogPostItem) 
        {
            try
            {
                _blogPostItem = blogPostItem;

                string html = string.Empty;

                string DisableScriptError = @"<script> 
                                                function noError() { return true; }
                                                window.onerror = noError;
                                              </script>";

                string head = $@"<head>
                                     {DisableScriptError}
                                     <meta charset = ""UTF-8"">
                                 </head>";

                string backgroundColor = AppData.PrimaryBackgroundColorBrush.ToString().Replace("#FF", "#");
                string secondaryBackgroundColor = AppData.SecondaryBackgroundColorBrush.ToString().Replace("#FF", "#");
                string fontColor = AppData.PrimaryFontColorBrush.ToString().Replace("#FF", "#");
                string primaryBtnColor = AppData.PrimaryButtonColorBrush.ToString().Replace("#FF", "#");
                string infoColor = AppData.InformationColorBrush.ToString().Replace("#FF", "#");
                string accentColor = AppData.PrimaryAccentBtnColorBrush.ToString().Replace("#FF", "#");
                string backgrounHighlightColor = AppData.BackgrounHighlightColorBrush.ToString().Replace("#FF", "#");

                string codeStylesCssFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Assets\Css\preview-blog.css";
                string codeStylesCss = File.Exists(codeStylesCssFilePath) ? File.ReadAllText(codeStylesCssFilePath) : string.Empty;

                codeStylesCss = codeStylesCss.Replace(nameof(fontColor), fontColor)
                                                .Replace(nameof(backgroundColor), backgroundColor)
                                                .Replace(nameof(backgrounHighlightColor), backgrounHighlightColor)
                                                .Replace(nameof(primaryBtnColor), primaryBtnColor)
                                                .Replace(nameof(accentColor), accentColor)
                                                .Replace(nameof(secondaryBackgroundColor), secondaryBackgroundColor)
                                                .Replace(nameof(infoColor), infoColor);

                string css = @"<style>" + codeStylesCss + @"</style>";

                string titleHtml = $@"<h1 class=""blog-post-title"">{_blogPostItem.Title}</h1>";

                string postInfoHtml = string.IsNullOrEmpty(_blogPostItem.Author)
                    ? "<h4>"
                    : $@"<h4 class=""blog-post-info"">
                            by: <span class=""blog-post-author"">{_blogPostItem.Author} </span> 
                            <span class=""blog-post-info-seperator"">|</span>";

                postInfoHtml += $"<span>{_blogPostItem.Date.Value.ToString("MMMM dd, yyyy")}</span> </h4>";

                string mainContent = $@"<body>
                                            {titleHtml}
                                            {postInfoHtml}
                                            {_blogPostItem.Content}
                                        </body>";

                html = head + css + mainContent;

                _htmlContent = html;
                OnPropertyChanged(nameof(HtmlContent));
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to load blog post", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
        }
    }
}