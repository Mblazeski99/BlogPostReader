using BlogReader.Models;
using BlogReader.ViewModels;

namespace BlogReader.Commands.Blogs.BlogSources
{
    public class CreateNewBlogSourceItemCommand : BaseCommand
    {
        private readonly BlogSourcesViewModel _viewModel;

        public CreateNewBlogSourceItemCommand(BlogSourcesViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            // TODO: Add default image
            _viewModel.SelectedSourceItem = new BlogPostItemSource()
            {
                Active = true,
                SourceName = "My Blog Source"
            };
        }
    }
}
