using Microsoft.Web.WebView2.Wpf;
using System.Windows;

namespace BlogReader.CustomControls
{
    public class ExtendedWebView : WebView2
    {
        public string HTMLContent
        {
            get { return (string)GetValue(HTMLContentProperty); }
            set
            {
                SetValue(HTMLContentProperty, value);
                NavigateToString(HTMLContent);
            }
        }

        public static readonly DependencyProperty HTMLContentProperty =
            DependencyProperty.Register(nameof(HTMLContent), typeof(string), typeof(ExtendedWebView), new PropertyMetadata(string.Empty));

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
