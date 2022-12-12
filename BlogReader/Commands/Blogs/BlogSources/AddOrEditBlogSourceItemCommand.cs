using BlogReader.Models;
using BlogReader.ViewModels;

namespace BlogReader.Commands.Blogs.BlogSources
{
    public class AddOrEditBlogSourceItemCommand : BaseCommand
    {
        private readonly BlogSourcesViewModel _viewModel;

        public AddOrEditBlogSourceItemCommand(BlogSourcesViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if (parameter == null) 
            {
                // TODO: Add default image
                _viewModel.SelectedSourceItem = new BlogPostItemSource()
                {
                    Active = true,
                    SourceName = "My Blog Source"
                };
            }
            else if (parameter is BlogPostItemSource)
            {
                var selectedSourceItem = (parameter as BlogPostItemSource);
                BlogPostItemSource.Copy(selectedSourceItem, _viewModel.SelectedSourceItem);
            }
        }
    }
}
