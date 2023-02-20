using BlogReader.ViewModels;

namespace BlogReader.Commands.Home
{
    public class ToggleIsExpandedCommand : BaseCommand
    {
        private readonly HomeViewModel _viewModel;

        public ToggleIsExpandedCommand(HomeViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.IsExpanded = !_viewModel.IsExpanded;
        }
    }
}
