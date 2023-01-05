using BlogReader.ViewModels;

namespace BlogReader.Commands.Blogs.ContentModels
{
    public class CancelNewContentModelCommand : BaseCommand
    {
        private readonly ContentModelsViewModel _viewModel;

        public CancelNewContentModelCommand(ContentModelsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.ClearDataPropertyErrors();
            _viewModel.SelectedContentModel = null;
        }
    }
}
