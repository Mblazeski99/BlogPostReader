using System.Diagnostics;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ToastNotifications.Utilities;
using System;

namespace BlogReader
{
    public static class AppData
    {
        public static readonly App App = Application.Current as App;

        #region EventHandlers
        public static void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            (sender as TextBox)?.SelectAll();
        }

        public static void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        public static void Hyperlink_RequestNavigate(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(new ProcessStartInfo()
            {
                FileName = link.NavigateUri.AbsoluteUri,
                UseShellExecute = true
            });
        }

        public static void HyperLinkTextBlock_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var textBlock = (sender as TextBlock);
            var parent = textBlock?.Parent as Control;

            if (textBlock?.Tag?.ToString() == "HyperLinkTextBlock"
                || parent?.Tag?.ToString() == "HyperLinkTextBlock")
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = textBlock.Text,
                    UseShellExecute = true
                });
            }
        }

        public static void DatePickerClearSelectionButton_Click(object sender, RoutedEventArgs e) 
        {
            var button = (sender as Button);

            if (button?.Tag?.ToString() == "DatePickerClearValueButton")
            {
                var datePicker = (button.TemplatedParent as DatePicker);
                datePicker?.ClearValue(DatePicker.SelectedDateProperty);
                button.Visibility = Visibility.Collapsed;
            }
        }

        public static void DatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DatePicker datePicker = (sender as DatePicker);
                Button clearValueButton = datePicker.FindChild<Button>("DatePickerClearValueButton");
                clearValueButton.Visibility = datePicker.SelectedDate.HasValue ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Colors
        public static Brush PrimaryBackgroundColorBrush => App.Resources[nameof(PrimaryBackgroundColorBrush)] as Brush;
        public static Brush SecondaryBackgroundColorBrush => App.Resources[nameof(SecondaryBackgroundColorBrush)] as Brush;
        public static Brush PrimaryFontColorBrush => App.Resources[nameof(PrimaryFontColorBrush)] as Brush;
        public static Brush PrimaryButtonColorBrush => App.Resources[nameof(PrimaryButtonColorBrush)] as Brush;
        public static Brush PrimaryAccentBtnColorBrush => App.Resources[nameof(PrimaryAccentBtnColorBrush)] as Brush;
        public static Brush InformationColorBrush => App.Resources[nameof(InformationColorBrush)] as Brush;
        public static Brush BackgrounHighlightColorBrush => App.Resources[nameof(BackgrounHighlightColorBrush)] as Brush;
        public static Brush CodeBackgrounColorBrush => App.Resources[nameof(CodeBackgrounColorBrush)] as Brush;
        #endregion

        public static string SecondaryDateFormatString => App.Resources[nameof(SecondaryDateFormatString)].ToString();
    }
}
