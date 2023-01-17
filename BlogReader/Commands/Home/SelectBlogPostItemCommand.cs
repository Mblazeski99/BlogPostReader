using BlogReader.Models;
using BlogReader.ViewModels;

namespace BlogReader.Commands.Home
{
    public class SelectBlogPostItemCommand : BaseCommand
    {
        private readonly HomeViewModel _viewModel;

        public SelectBlogPostItemCommand(HomeViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if (parameter == null || (parameter is BlogPostItem) == false) return;
            _viewModel.SelectedBlogPostItemDataContext.RenderBlogPostItem(parameter as BlogPostItem);
        }
    }
}
