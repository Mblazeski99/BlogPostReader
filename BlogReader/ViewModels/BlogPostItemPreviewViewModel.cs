using BlogReader.Models;
using BlogReader.Models.Enums;
using BlogReader.Stores;
using System;
using System.IO;

namespace BlogReader.ViewModels
{
    public class BlogPostItemPreviewViewModel : BaseViewModel
    {
        private readonly BlogPostItem _blogPostItem;
        private readonly string _htmlContent = string.Empty;

        public string HtmlContent => _htmlContent;

        public BlogPostItemPreviewViewModel() { }

        public BlogPostItemPreviewViewModel(BlogPostItem blogPostItem,
            NotificationsStore notificationsStore)
        {
            _blogPostItem = blogPostItem;
            try
            {
                string html = string.Empty;

                string DisableScriptError = @"<script> 
                                                function noError() { return true; }
                                                window.onerror = noError;
                                              </script>";

                string head = $@"<head>
                                     {DisableScriptError}
                                     <meta charset = ""UTF-8"">
                                 </head>";

                string backgroundColor = AppData.PrimaryBackgroundColorBrush.ToString();
                string fontColor = AppData.PrimaryFontColorBrush.ToString().Replace("FF", "");
                string primaryBtnColor = AppData.PrimaryButtonColorBrush.ToString().Replace("FF", "");

                string codeStylesCssFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Assets\Css\preview-blog.css";
                string codeStylesCss = File.Exists(codeStylesCssFilePath) ? File.ReadAllText(codeStylesCssFilePath) : string.Empty;

                string css = @"<style>

                                " + codeStylesCss + @"

                                body { 
                                    font: 400 20px/1.25 Arial,Gadget,sans-serif;
                                    color: " + fontColor + @";
                                    background-color: " + backgroundColor + @";
                                    padding: 10px;
                                }

                                .blog-post-author { color: " + primaryBtnColor + @"; }

                               </style>";

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
                notificationsStore.AddNotification(error);
            }
        }
    }
}