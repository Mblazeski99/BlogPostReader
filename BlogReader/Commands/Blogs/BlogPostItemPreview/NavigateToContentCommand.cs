using BlogReader.Models.Enums;
using BlogReader.Models;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System;
using Microsoft.Web.WebView2.Wpf;

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

                string head = "<head> \n" + DisableScriptError + "<meta charset = \"UTF-8\"> \n" + "</head> \n";

                string backgroundColor = AppData.PrimaryBackgroundColorBrush.ToString();
                string fontColor = AppData.PrimaryFontColorBrush.ToString().Replace("FF", "");
                string primaryBtnColor = AppData.PrimaryButtonColorBrush.ToString().Replace("FF", "");

                string css = @"<style> 

                                body { 
                                    font: 400 20px/1.25 Metric,Arial,Gadget,sans-serif;
                                    color: " + fontColor + @";
                                    background-color: " + backgroundColor + @";
                                    padding: 10px;
                                }

                                div, p, a {
                                    font: 400 20px/1.25 Metric,Arial,Gadget,sans-serif;
                                    color: " + fontColor + @";
                                }
                                
                                a { 
                                    color: #0066cc; 
                                    text-decoration: none;
                                }

                                a:hover { 
                                    text-decoration: underline;
                                    color: #ff0000; 
                                }

                                img { max-width: 90vw; }

                                .blog-post-title { 
                                    font-size: 52px; 
                                    margin-bottom: 0;
                                }

                                .blog-post-info { 
                                    font-weight: 300;
                                    margin-top: 10px;
                                    margin-bottom: 50px;
                                }

                                .blog-post-author { color: " + primaryBtnColor + @"; }
                                
                                .blog-post-info-seperator {  
                                    color: #d9d9d9;
                                    margin: 0 0.75em;
                                }

                               </style>";

                string titleHtml = $@"<h1 class=""blog-post-title"">{_viewModel.BlogPostItem.Title}</h1>";

                string postInfoHtml = string.IsNullOrEmpty(_viewModel.BlogPostItem.Author)
                    ? "<h4>"
                    : $@"<h4 class=""blog-post-info"">
                            by: <span class=""blog-post-author"">{_viewModel.BlogPostItem.Author} </span> 
                            <span class=""blog-post-info-seperator"">|</span>";

                postInfoHtml += "<span>January 06, 2023</span> </h4>";

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
