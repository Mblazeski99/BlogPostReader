using BlogReader.DataModels;
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

            var blogPostItem = parameter as BlogPostItem;
            _viewModel.SelectedBlogPostItemDataContext.RenderBlogPostItem(blogPostItem);
            AppData.App.MainWindow.Title = $"Blog Reader - {blogPostItem.Title}";
        }
    }
}
