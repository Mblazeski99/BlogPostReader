﻿using BlogReader.CustomControls;
using BlogReader.DataModels;
using BlogReader.DataModels.Enums;
using BlogReader.Enums;
using BlogReader.Stores;
using BlogReader.ViewModels;
using System;

namespace BlogReader.Commands.Blogs.BlogSources
{
    public class RemoveBlogSourceItemCommand : BaseCommand
    {
        private readonly BlogSourcesViewModel _viewModel;
        private readonly BlogPostItemsStore _blogPostItemsStore;
        private readonly NotificationsStore _notificationsStore;

        public RemoveBlogSourceItemCommand(BlogSourcesViewModel viewModel,
            BlogPostItemsStore blogPostItemsStore,
            NotificationsStore notificationsStore)
        {
            _viewModel = viewModel;
            _blogPostItemsStore = blogPostItemsStore;
            _notificationsStore = notificationsStore;
        }

        public override void Execute(object parameter)
        {
            if (parameter == null || string.IsNullOrWhiteSpace(parameter.ToString())) return;

            try
            {
                string itemToRemoveId = parameter.ToString();
                ExtendedMessageBoxResult answer = ExtendedMessageBox.Show("Are you sure you want to delete this source?", "Delete Source", ExtendedMessageBoxButton.YesNo, ExtendedMessageBoxImage.Question);

                if (answer == ExtendedMessageBoxResult.Yes)
                {
                    _viewModel.IsItemsGridLoading = true;

                    if (_viewModel.SelectedSourceItem?.Id == itemToRemoveId)
                    {
                        _viewModel.SelectedSourceItem = null;
                    }

                    _blogPostItemsStore.RemoveBlogItemSource(itemToRemoveId);

                    var notification = new Notification(MessageType.Success, "Successfully deleted blog source");
                    _notificationsStore.AddNotification(notification);

                    base.Execute(parameter);
                }
            }
            catch (Exception ex)
            {
                var error = new Notification(MessageType.Error, "Failed to delete blog source", ex.ToString());
                _notificationsStore.AddNotification(error);
            }
            finally
            {
                _viewModel.IsItemsGridLoading = false;
            }
        }
    }
}
