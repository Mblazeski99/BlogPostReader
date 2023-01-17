using Microsoft.Web.WebView2.Wpf;
using System.Windows;

namespace BlogReader.CustomControls
{
    public class ExtendedWebView : WebView2
    {
        public string HTMLContent
        {
            get 
            {
                string value = string.Empty;

                if (string.IsNullOrEmpty((string)GetValue(HTMLContentProperty)) == false)
                {
                    value = (string)GetValue(HTMLContentProperty);
                }

                return value;
            }
            set
            {
                SetValue(HTMLContentProperty, value);
            }
        }

        public static readonly DependencyProperty HTMLContentProperty =
            DependencyProperty.Register(nameof(HTMLContent), 
                typeof(string), 
                typeof(ExtendedWebView), 
                new PropertyMetadata(default(ExtendedWebView), OnHTMLContentPropertyChanged));

        private async static void OnHTMLContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webView = d as ExtendedWebView;
            string htmlContent = e.NewValue?.ToString() != null ? e.NewValue?.ToString() : "";

            await webView.EnsureCoreWebView2Async();
            webView.NavigateToString(htmlContent);
        }

        public ExtendedWebView() : base()
        {
            EnsureCreated();
        }

        private async void EnsureCreated()
        {
            await EnsureCoreWebView2Async();
            NavigateToString(HTMLContent);
        }
    }
}
