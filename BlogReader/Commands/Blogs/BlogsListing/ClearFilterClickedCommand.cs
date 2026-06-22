using BlogReader.ViewModels;
using Serilog;
using System;

namespace BlogReader.Commands.Blogs.BlogsListing
{
    public class ClearFilterClickedCommand : BaseCommand
    {
        private readonly BlogsListingViewModel _viewModel;

        public ClearFilterClickedCommand(BlogsListingViewModel viewModel) 
        { 
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            try
            {
                _viewModel.LoadBlogPostItems();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to clear blogs listing filter!");
            }
        }
    }
}
