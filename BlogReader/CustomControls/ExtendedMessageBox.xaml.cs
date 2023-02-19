using BlogReader.Enums;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BlogReader.CustomControls
{
    public partial class ExtendedMessageBox : Window
    {
        public static readonly DependencyProperty ButtonsProperty =
            DependencyProperty.RegisterAttached(nameof(Buttons), 
                typeof(ExtendedMessageBoxButton), typeof(ExtendedMessageBox), 
                new PropertyMetadata(ExtendedMessageBoxButton.OK));

        public ExtendedMessageBoxButton Buttons
        {
            get { return ((ExtendedMessageBoxButton)GetValue(ButtonsProperty)); }
            set
            {
                SetValue(ButtonsProperty, value);
                OnPropertyChanged(nameof(Buttons));
            }
        }

        public static readonly DependencyProperty DialogImageProperty =
            DependencyProperty.RegisterAttached(nameof(DialogImage),
                typeof(ExtendedMessageBoxImage), 
                typeof(ExtendedMessageBox), 
                new PropertyMetadata(ExtendedMessageBoxImage.Warning));

        public ExtendedMessageBoxImage DialogImage
        {
            get { return ((ExtendedMessageBoxImage)GetValue(DialogImageProperty)); }
            set
            {
                SetValue(DialogImageProperty, value);
                OnPropertyChanged(nameof(DialogImage));
            }
        }

        public ExtendedMessageBoxResult Result { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ExtendedMessageBox()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static ExtendedMessageBoxResult Show(ContentControl ctrlOwner, string strMessage, string strCaption = null, ExtendedMessageBoxButton button = ExtendedMessageBoxButton.OK, ExtendedMessageBoxImage image = ExtendedMessageBoxImage.Warning)
        {
            try
            {
                ExtendedMessageBox dlg = new ExtendedMessageBox();
                dlg.DialogImage = image;
                dlg.Buttons = button;

                if (strCaption != null) dlg.Title = $"Blog Reader - {strCaption}";

                dlg.txtMessage.Text = strMessage;

                dlg.ShowDialog();

                ExtendedMessageBoxResult res = dlg.Result;
                return (res);
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError(err.ToString());
                return (ExtendedMessageBoxResult.None);
            }
        }

        public static ExtendedMessageBoxResult Show(string strMessage, string strCaption = null)
        {
            return Show(null, strMessage, strCaption);
        }

        public static ExtendedMessageBoxResult Show(string strMessage, string strCaption, ExtendedMessageBoxButton button)
        {
            return Show(null, strMessage, strCaption, button);
        }

        public static ExtendedMessageBoxResult Show(string strMessage, string strCaption, ExtendedMessageBoxButton button, ExtendedMessageBoxImage image)
        {
            return Show(null, strMessage, strCaption, button, image);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Result = ExtendedMessageBoxResult.OK;
            DialogResult = true;
            Close();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Result = ExtendedMessageBoxResult.Yes;
            DialogResult = true;
            Close();
        }

        private void BtnApprove_Click(object sender, RoutedEventArgs e)
        {
            Result = ExtendedMessageBoxResult.Approve;
            DialogResult = true;
            Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Result = ExtendedMessageBoxResult.No;
            DialogResult = false;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = ExtendedMessageBoxResult.Cancel;
            DialogResult = false;
            Close();
        }

        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            Result = ExtendedMessageBoxResult.Reject;
            DialogResult = false;
            Close();
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}