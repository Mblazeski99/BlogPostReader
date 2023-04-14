using BlogReader.ViewModels;

namespace BlogReader.Commands.Home
{
    public class IncreaseNumberOfItemsShownCommand : BaseCommand
    {
        private readonly HomeViewModel _viewModel;

        public IncreaseNumberOfItemsShownCommand(HomeViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.NumberOfItemsShown += 20;
            _viewModel.SetItemsToShow();
        }
    }
}
