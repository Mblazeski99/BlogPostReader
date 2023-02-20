using BlogReader.ViewModels;

namespace BlogReader.Commands.Home
{
    public class DeSelectBlogPostItemCommand : BaseCommand
    {
        private readonly HomeViewModel _viewModel;

        public DeSelectBlogPostItemCommand(HomeViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.SelectedBlogPostItemDataContext.BlogPostItem = null;
        }
    }
}
