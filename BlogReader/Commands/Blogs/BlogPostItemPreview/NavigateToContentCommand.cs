using BlogReader.Models.Enums;
using BlogReader.Models;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System;
using Microsoft.Web.WebView2.Wpf;
using System.IO;

namespace BlogReader.Commands.Blogs.BlogPostItemPreview
{
    public class NavigateToContentCommand : BaseCommand
    {
        private readonly BlogPostItemPreviewViewModel _viewModel;
        private readonly NotificationsStore _notificationsStore;

        public NavigateToContentCommand(BlogPostItemPreviewViewModel viewModel, 
            NotificationsStore notificationsStore)
        {
            _viewModel = viewModel;
            _notificationsStore = notificationsStore;
        }

        public override async void Execute(object parameter)
        {
            try
            {
                if (parameter == null) throw new ArgumentNullException(nameof(parameter));

                WebView2 webViewControl = (parameter as WebView2);
                await webViewControl.EnsureCoreWebView2Async();

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
                                    font: 400 20px/1.25 Metric,Arial,Gadget,sans-serif;
                                    color: " + fontColor + @";
                                    background-color: " + backgroundColor + @";
                                    padding: 10px;
                                }

                                .blog-post-author { color: " + primaryBtnColor + @"; }

                               </style>";

                string titleHtml = $@"<h1 class=""blog-post-title"">{_viewModel.BlogPostItem.Title}</h1>";

                string postInfoHtml = string.IsNullOrEmpty(_viewModel.BlogPostItem.Author)
                    ? "<h4>"
                    : $@"<h4 class=""blog-post-info"">
                            by: <span class=""blog-post-author"">{_viewModel.BlogPostItem.Author} </span> 
                            <span class=""blog-post-info-seperator"">|</span>";

                postInfoHtml += $"<span>{_viewModel.BlogPostItem.Date.Value.ToString("MMMM dd, yyyy")}</span> </h4>";

                string mainContent = $@"<body>
                                            {titleHtml}
                                            {postInfoHtml}
                                            {_viewModel.BlogPostItem.Content}
                                        </body>";

                html = head + css + mainContent;

                webViewControl.NavigateToString(html);
            }
            catch (Exception ex)
            {   
                var error = new Notification(MessageType.Error, "Failed to load blog post", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
        }
    }
}
