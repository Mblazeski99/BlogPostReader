using BlogReader.DataModels;
using BlogReader.ViewModels;

namespace BlogReader.Commands.Blogs.ContentModels
{
    public class CreateNewContentModelCommand : BaseCommand
    {
        private readonly ContentModelsViewModel _viewModel;

        public CreateNewContentModelCommand(ContentModelsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.SelectedContentModel = new RssContentModel()
            {
                ModelName = "My Content Model"
            };
        }
    }
}
