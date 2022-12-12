using System.Diagnostics;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BlogReader
{
    public class AppData
    {
        #region EventHandlers
        public void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            (sender as TextBox)?.SelectAll();
        }

        public void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                if (parent is TextBox)
                {
                    var textBox = (TextBox)parent;
                    if (!textBox.IsKeyboardFocusWithin)
                    {
                        textBox.Focus();
                        e.Handled = true;
                    }
                }
            }
        }

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
