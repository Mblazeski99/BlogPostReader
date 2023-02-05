using System.Windows;

namespace BlogReader.Commands.App
{
    public class ExitAppCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            MessageBoxResult answer = MessageBox.Show("Are you sure you want to exit this application?", "Exit Blog Reader?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.Yes) Application.Current.Shutdown();
        }
    }
}
