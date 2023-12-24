using BlogReader.CustomControls.GridFilterPopup;
using BlogReader.DataModels;
using BlogReader.ViewModels;
using System;

namespace BlogReader.Commands.Blogs.BlogsListing
{
    public class FilterClickedCommand : BaseCommand
    {
        private readonly BlogsListingViewModel _viewModel;

        public FilterClickedCommand(BlogsListingViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            try
            {
                var eventArgs = (parameter as GridFilterPopupButtonEventArgs);
                
                _viewModel.BlogPostItems.Clear();
                foreach (var item in eventArgs.FilteredItems)
                {
                    _viewModel.BlogPostItems.Add(item as BlogPostItem);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
