using BlogReader.CustomControls;
using BlogReader.Enums;
using System.Windows;

namespace BlogReader.Commands.App
{
    public class ExitAppCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            ExtendedMessageBoxResult answer = ExtendedMessageBox.Show("Are you sure you want to exit this application?", "Exit Blog Reader?", ExtendedMessageBoxButton.YesNo, ExtendedMessageBoxImage.Question);
            if (answer == ExtendedMessageBoxResult.Yes) Application.Current.Shutdown();
        }
    }
}
