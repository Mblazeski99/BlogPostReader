using System.Diagnostics;
using System.Windows.Documents;
using System.Windows;

namespace BlogReader
{
    public class AppData
    {
        #region EventHandlers
        public void Hyperlink_RequestNavigate(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(new ProcessStartInfo()
            {
                FileName = link.NavigateUri.AbsoluteUri,
                UseShellExecute = true
            });
        }
        #endregion
    }
}
