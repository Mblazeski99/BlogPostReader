using BlogReader.DataModels;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace BlogReader.CustomControls.GridFilterPopup
{
    public partial class GridFilterPopupButton : UserControl
    {
        private Type _selectedFilterPropertyType;

        private ObservableCollection<BlogPostItem> _allItems { get; set; } = new ObservableCollection<BlogPostItem>();
        private ObservableCollection<BlogPostItem> _items { get; set; } = new ObservableCollection<BlogPostItem>();

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

        private async static void OnFilterDescriptorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var popupBtn = d as GridFilterPopupButton;
            var filterDescriptor = e.NewValue as GridFilterDescriptor;

            popupBtn.PropertiesComboBox.ItemsSource = filterDescriptor?.PropertyNames;
        }

        public GridFilterPopupButton()
        {
            InitializeComponent();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterPopup.IsOpen = !FilterPopup.IsOpen;
        }

        private void PropertiesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterDescriptor?.ItemsSource?.Count > 0)
            {
                FilterValueSection.Visibility = Visibility.Visible;

                string selectedPropertyName = e.AddedItems[0].ToString();
                PropertyInfo selectedPropertyInfo = FilterDescriptor.ItemsSource[0].GetType().GetProperty(selectedPropertyName);
                _selectedFilterPropertyType = selectedPropertyInfo.PropertyType;

                if (_selectedFilterPropertyType != null)
                {
                    DisablePropertyTypeFields();

                    if (_selectedFilterPropertyType == typeof(DateTime)
                        || _selectedFilterPropertyType == typeof(DateTime?))
                    {
                        DateTimePropertyFilterSection.Visibility = Visibility.Visible;
                    }
                    else if (_selectedFilterPropertyType == typeof(bool)
                            || _selectedFilterPropertyType == typeof(bool?))
                    {
                        BooleanPropertyFilterSection.Visibility = Visibility.Visible;
                        FilterValueTypesComboBox.Visibility = Visibility.Collapsed;
                    }
                    else if (_selectedFilterPropertyType.IsEnum)
                    {
                        EnumPropertyFilterSection.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        DefaultPropertyFilterSection.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void DisablePropertyTypeFields()
        {
            DefaultPropertyFilterSection.Visibility = Visibility.Collapsed;
            DateTimePropertyFilterSection.Visibility = Visibility.Collapsed;
            BooleanPropertyFilterSection.Visibility = Visibility.Collapsed;
            EnumPropertyFilterSection.Visibility = Visibility.Collapsed;
            FilterValueTypesComboBox.Visibility = Visibility.Visible;
        }
    }
}
