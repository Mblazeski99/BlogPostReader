using BlogReader.DataModels;
using BlogReader.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BlogReader.CustomControls.GridFilterPopup
{
    public partial class GridFilterPopupButton : UserControl
    {
        private bool _useSecondCondition = false;
        private ObservableCollection<BaseEntity> _allItems { get; set; } = new ObservableCollection<BaseEntity>();

        public GridFilterDescriptor FilterDescriptor
        {
            get { return (GridFilterDescriptor)GetValue(FilterDescriptorProperty); }
            set { SetValue(FilterDescriptorProperty, value); }
        }

        public static readonly DependencyProperty FilterDescriptorProperty =
            DependencyProperty.Register(nameof(FilterDescriptor),
                typeof(GridFilterDescriptor),
                typeof(GridFilterPopupButton),
                new PropertyMetadata(default(GridFilterPopupButton), OnFilterDescriptorPropertyChanged));

        public event EventHandler OnFilterClicked;
        public event EventHandler OnClearFilterClicked;

        public GridFilterPopupButton()
        {
            InitializeComponent();
        }

        #region Events

        private async static void OnFilterDescriptorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var popupBtn = d as GridFilterPopupButton;
                var filterDescriptor = e.NewValue as GridFilterDescriptor;

                popupBtn._allItems = filterDescriptor?.ItemsSource.ToObservableCollection();

                if (filterDescriptor?.ItemsSource == null || filterDescriptor?.ItemsSource.Count == 0)
                {
                    popupBtn.IsEnabled = false;
                }
                else
                {
                    popupBtn.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void FilterPopupButton_Click(object sender, RoutedEventArgs e)
        {
            FilterPopup.IsOpen = !FilterPopup.IsOpen;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GridFilterPopupConditionResult filteredResult = FirstCondition.Filter(this._allItems);
                ObservableCollection<BaseEntity> filteredItems = filteredResult.FilteredItems;

                if (filteredResult.IsSuccessful)
                {
                    if (_useSecondCondition) 
                    {
                        if ((int)FilterConditionTypeComboBox.SelectedValue == (int)GridFilterConditionType.AND)
                        {
                            filteredResult = SecondCondition.Filter(filteredResult.FilteredItems.ToObservableCollection());

                            if (filteredResult.IsSuccessful == false)
                            {
                                // TODO: Log error
                                return;
                            }

                            filteredItems = filteredResult.FilteredItems.ToObservableCollection();
                        }
                        else if ((int)FilterConditionTypeComboBox.SelectedValue == (int)GridFilterConditionType.OR)
                        {
                            GridFilterPopupConditionResult secondConditionFilteredResult = SecondCondition.Filter(this._allItems);

                            if (secondConditionFilteredResult.IsSuccessful == false)
                            {
                                // TODO: Log error
                                return;
                            }

                            filteredItems.AddRange(secondConditionFilteredResult.FilteredItems);
                        }

                        filteredItems = filteredItems.Distinct().ToObservableCollection();
                    }

                    OnFilterClicked?.Invoke(this, new GridFilterPopupButtonEventArgs() { FilteredItems = filteredItems });

                    FilterPopup.IsOpen = false;
                    FilterPopupButton.Tag = true; // Is Filter Active
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FirstCondition.Clear();
                SecondCondition.Clear();

                OnClearFilterClicked?.Invoke(this, EventArgs.Empty);

                FilterPopup.IsOpen = false;
                FilterPopupButton.Tag = false; // Is Filter Active
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        private void AddConditionButton_Click(object sender, RoutedEventArgs e)
        {
            _useSecondCondition = true;
            SecondConditionContainerGrid.Visibility = Visibility.Visible;
            AddConditionButton.Visibility = Visibility.Collapsed;
        }

        private void RemoveConditionButton_Click(object sender, RoutedEventArgs e)
        {
            _useSecondCondition = false;
            SecondConditionContainerGrid.Visibility = Visibility.Collapsed;
            AddConditionButton.Visibility = Visibility.Visible;
        }
    }

    public class GridFilterPopupButtonEventArgs : EventArgs
    {
        public ObservableCollection<BaseEntity> FilteredItems { get; set; }
    }
}
