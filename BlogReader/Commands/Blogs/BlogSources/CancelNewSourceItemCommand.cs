using BlogReader.ViewModels;

namespace BlogReader.Commands.Blogs.BlogSources
{
    public class CancelNewSourceItemCommand : BaseCommand
    {
        private readonly BlogSourcesViewModel _viewModel;

        public CancelNewSourceItemCommand(BlogSourcesViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.ClearDataPropertyErrors();
            _viewModel.SelectedSourceItem = null;
        }
    }
}
