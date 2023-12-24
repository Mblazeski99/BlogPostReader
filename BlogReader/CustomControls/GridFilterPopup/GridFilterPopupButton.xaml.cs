using BlogReader.DataModels;
using BlogReader.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace BlogReader.CustomControls.GridFilterPopup
{
    public partial class GridFilterPopupButton : UserControl
    {
        private Type _itemSourceType;
        private List<GridFilterRuleType> _genericGridFilterRuleTypes = new List<GridFilterRuleType>();
        private List<GridFilterRuleType> _stringGridFilterRuleTypes = new List<GridFilterRuleType>();
        private List<GridFilterRuleType> _numberGridFilterRuleTypes = new List<GridFilterRuleType>();
        private List<GridFilterRuleType> _dateGridFilterRuleTypes = new List<GridFilterRuleType>();

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
            SetGridFilterRuleTypes();
        }

        #region Events

        private async static void OnFilterDescriptorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var popupBtn = d as GridFilterPopupButton;
                var filterDescriptor = e.NewValue as GridFilterDescriptor;

                popupBtn.PropertiesComboBox.ItemsSource = filterDescriptor?.PropertyNames;
                popupBtn._allItems = filterDescriptor?.ItemsSource.ToObservableCollection();

                if (filterDescriptor?.ItemsSource == null || filterDescriptor?.ItemsSource.Count == 0)
                {
                    popupBtn.IsEnabled = false;
                }
                else
                {
                    popupBtn._itemSourceType = popupBtn._allItems.FirstOrDefault().GetType();
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
                RemoveRequiredFieldErrors();

                if (CheckRequiredFieldsForErrors() && _allItems != null && _allItems.Count > 0)
                {
                    ObservableCollection<BaseEntity> filteredItems = new ObservableCollection<BaseEntity>();
                    string selectedProperty = PropertiesComboBox.SelectedValue.ToString();
                    GridFilterRuleType? selectedCondition = null;

                    if (FilterValueTypesComboBox.SelectedValue is not null)
                    {
                        selectedCondition = (GridFilterRuleType)FilterValueTypesComboBox.SelectedValue;
                    }

                    if (StringPropertyFilterSection.Visibility == Visibility.Visible)
                    {
                        filteredItems = FilterItemsByStringProperty(selectedProperty, selectedCondition.Value, StringPropertyInputField.Text);
                    }
                    else if (NumberPropertyFilterSection.Visibility == Visibility.Visible)
                    {
                        filteredItems = FilterItemsByNumberProperty(selectedProperty, selectedCondition.Value, Convert.ToInt64(NumberPropertyInputField.Text));
                    }
                    else if (DateTimePropertyFilterSection.Visibility == Visibility.Visible)
                    {
                        filteredItems = FilterItemsByDateTimeProperty(selectedProperty, selectedCondition.Value, DateTimePropertyInputField.SelectedDate.Value);
                    }
                    else if (BooleanPropertyFilterSection.Visibility == Visibility.Visible)
                    {
                        filteredItems = FilterItemsByBooleanProperty(selectedProperty, BoolPropertyInputField.IsChecked.Value);
                    }
                    else if (EnumPropertyFilterSection.Visibility == Visibility.Visible)
                    {
                        filteredItems = FilterItemsByEnumProperty(selectedProperty, selectedCondition.Value, EnumValueTypesComboBox.SelectedValue as Enum);
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
                RemoveRequiredFieldErrors();

                PropertiesComboBox.SelectedValue = null;
                FilterValueTypesComboBox.SelectedValue = null;
                StringPropertyInputField.Text = string.Empty;
                NumberPropertyInputField.Text = string.Empty;
                DateTimePropertyInputField?.ClearValue(DatePicker.SelectedDateProperty);
                BoolPropertyInputField.IsChecked = false;
                EnumValueTypesComboBox.SelectedValue = null;

                OnClearFilterClicked?.Invoke(this, EventArgs.Empty);

                FilterValueSection.Visibility = Visibility.Collapsed;

                FilterPopup.IsOpen = false;
                FilterPopupButton.Tag = false; // Is Filter Active
            }
            catch (Exception ex)
            {
            }
        }

        private void PropertiesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (FilterDescriptor?.ItemsSource?.Count > 0 && e.AddedItems != null && e.AddedItems.Count > 0)
                {
                    FilterValueSection.Visibility = Visibility.Visible;

                    string selectedPropertyName = e.AddedItems[0].ToString();
                    PropertyInfo selectedPropertyInfo = FilterDescriptor.ItemsSource[0].GetType().GetProperty(selectedPropertyName);
                    Type selectedFilterPropertyType = selectedPropertyInfo.PropertyType;

                    if (selectedFilterPropertyType != null)
                    {
                        DisablePropertyTypeFields();

                        bool isEnumProperty = selectedFilterPropertyType.IsEnum;
                        bool isDateTimeProperty = selectedFilterPropertyType == typeof(DateTime) || selectedFilterPropertyType == typeof(DateTime?);
                        bool isBoolProperty = selectedFilterPropertyType == typeof(bool) || selectedFilterPropertyType == typeof(bool?);
                        bool isStringProperty = selectedFilterPropertyType == typeof(string);
                        bool isNumberProperty = selectedFilterPropertyType == typeof(int) 
                                                || selectedFilterPropertyType == typeof(int?)
                                                || selectedFilterPropertyType == typeof(double)
                                                || selectedFilterPropertyType == typeof(double?)
                                                || selectedFilterPropertyType == typeof(decimal)
                                                || selectedFilterPropertyType == typeof(decimal?)
                                                || selectedFilterPropertyType == typeof(float)
                                                || selectedFilterPropertyType == typeof(float?);

                        FilterValueTypesSection.Visibility = isBoolProperty ? Visibility.Collapsed : Visibility.Visible;

                        if (isDateTimeProperty)
                        {
                            DateTimePropertyFilterSection.Visibility = Visibility.Visible;
                            FilterValueTypesComboBox.ItemsSource = _dateGridFilterRuleTypes;
                        }
                        else if (isBoolProperty)
                        {
                            BooleanPropertyFilterSection.Visibility = Visibility.Visible;
                        }
                        else if (isStringProperty)
                        {
                            StringPropertyFilterSection.Visibility = Visibility.Visible;
                            FilterValueTypesComboBox.ItemsSource = _stringGridFilterRuleTypes;
                        }
                        else if (isNumberProperty)
                        {
                            NumberPropertyFilterSection.Visibility = Visibility.Visible;
                            FilterValueTypesComboBox.ItemsSource = _numberGridFilterRuleTypes;
                        }
                        else if (isEnumProperty)
                        {
                            EnumPropertyFilterSection.Visibility = Visibility.Visible;
                            FilterValueTypesComboBox.ItemsSource = _genericGridFilterRuleTypes;
                            EnumValueTypesComboBox.ItemsSource = selectedFilterPropertyType.GetEnumValues();
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
            }
        }

        #endregion

        #region Methods

        private void DisablePropertyTypeFields()
        {
            try
            {
                StringPropertyFilterSection.Visibility = Visibility.Collapsed;
                NumberPropertyFilterSection.Visibility = Visibility.Collapsed;
                DateTimePropertyFilterSection.Visibility = Visibility.Collapsed;
                BooleanPropertyFilterSection.Visibility = Visibility.Collapsed;
                EnumPropertyFilterSection.Visibility = Visibility.Collapsed;
                FilterValueTypesComboBox.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
            }
        }

        private void SetGridFilterRuleTypes()
        {
            try
            {
                _genericGridFilterRuleTypes = new List<GridFilterRuleType>();
                _stringGridFilterRuleTypes = new List<GridFilterRuleType>();
                _numberGridFilterRuleTypes = new List<GridFilterRuleType>();
                _dateGridFilterRuleTypes = new List<GridFilterRuleType>();

                _genericGridFilterRuleTypes.Add(GridFilterRuleType.IsEqualTo);
                _genericGridFilterRuleTypes.Add(GridFilterRuleType.IsNotEqualTo);
                _stringGridFilterRuleTypes.Add(GridFilterRuleType.IsEqualTo);
                _stringGridFilterRuleTypes.Add(GridFilterRuleType.IsNotEqualTo);
                _numberGridFilterRuleTypes.Add(GridFilterRuleType.IsEqualTo);
                _numberGridFilterRuleTypes.Add(GridFilterRuleType.IsNotEqualTo);
                _dateGridFilterRuleTypes.Add(GridFilterRuleType.IsEqualTo);
                _dateGridFilterRuleTypes.Add(GridFilterRuleType.IsNotEqualTo);

                _stringGridFilterRuleTypes.Add(GridFilterRuleType.StartsWith);
                _stringGridFilterRuleTypes.Add(GridFilterRuleType.EndsWith);
                _stringGridFilterRuleTypes.Add(GridFilterRuleType.Contains);
                _stringGridFilterRuleTypes.Add(GridFilterRuleType.DoesNotContain);

                _numberGridFilterRuleTypes.Add(GridFilterRuleType.IsGreaterThanOrEqualTo);
                _numberGridFilterRuleTypes.Add(GridFilterRuleType.IsGreaterThan);
                _numberGridFilterRuleTypes.Add(GridFilterRuleType.IsLessThan);
                _numberGridFilterRuleTypes.Add(GridFilterRuleType.IsLessThanOrEqualTo);

                _dateGridFilterRuleTypes.Add(GridFilterRuleType.IsGreaterThanOrEqualTo);
                _dateGridFilterRuleTypes.Add(GridFilterRuleType.IsGreaterThan);
                _dateGridFilterRuleTypes.Add(GridFilterRuleType.IsLessThanOrEqualTo);
                _dateGridFilterRuleTypes.Add(GridFilterRuleType.IsLessThan);
            }
            catch (Exception ex)
            {
            }
        }

        private bool CheckRequiredFieldsForErrors()
        {
            bool result = true;

            try
            {
                if (PropertiesComboBox.SelectedValue is null)
                {
                    PropertyErrorTextBlock.Visibility = Visibility.Visible;
                    result = false;
                }

                if (FilterValueTypesComboBox.Visibility == Visibility.Visible
                    && FilterValueTypesComboBox.SelectedValue == null
                    && BooleanPropertyFilterSection.Visibility == Visibility.Collapsed)
                {
                    ConditionErrorTextBlock.Visibility = Visibility.Visible;
                    result = false;
                }

                if (StringPropertyFilterSection.Visibility == Visibility.Visible)
                {
                    if (StringPropertyInputField.Text == null || StringPropertyInputField.Text == string.Empty)
                    {
                        ValueErrorTextBlock.Visibility = Visibility.Visible;
                        result = false;
                    }
                }

                if (NumberPropertyFilterSection.Visibility == Visibility.Visible)
                {
                    if (NumberPropertyInputField.Text == null || NumberPropertyInputField.Text == string.Empty)
                    {
                        ValueErrorTextBlock.Visibility = Visibility.Visible;
                        result = false;
                    }
                }

                if (DateTimePropertyFilterSection.Visibility == Visibility.Visible)
                {
                    if (DateTimePropertyInputField.SelectedDate == null)
                    {
                        ValueErrorTextBlock.Visibility = Visibility.Visible;
                        result = false;
                    }
                }

                if (EnumPropertyFilterSection.Visibility == Visibility.Visible)
                {
                    if (EnumValueTypesComboBox.SelectedValue == null)
                    {
                        ValueErrorTextBlock.Visibility = Visibility.Visible;
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return result;
        }

        private void RemoveRequiredFieldErrors()
        {
            try
            {
                PropertyErrorTextBlock.Visibility = Visibility.Collapsed;
                ConditionErrorTextBlock.Visibility = Visibility.Collapsed;
                ValueErrorTextBlock.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex) { }
        }

        private ObservableCollection<BaseEntity> FilterItemsByStringProperty(string propertyName, GridFilterRuleType condition, string value)
        {
            try
            {
                IEnumerable<BaseEntity> result = null;

                if (condition == GridFilterRuleType.IsEqualTo)
                {
                    result = _allItems.Where(i => _itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)
                                        .ToString()
                                        .ToLower() == value.ToLower());
                }
                else if (condition == GridFilterRuleType.IsNotEqualTo)
                {
                    result = _allItems.Where(i => _itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)
                                        .ToString()
                                        .ToLower() != value.ToLower());
                }
                else if (condition == GridFilterRuleType.StartsWith)
                {
                    result = _allItems.Where(i => _itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)
                                        .ToString()
                                        .ToLower()
                                        .StartsWith(value.ToLower()));
                }
                else if (condition == GridFilterRuleType.EndsWith)
                {
                    result = _allItems.Where(i => _itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)
                                        .ToString()
                                        .ToLower()
                                        .EndsWith(value.ToLower()));
                }
                else if (condition == GridFilterRuleType.Contains)
                {
                    result = _allItems.Where(i => _itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)
                                        .ToString()
                                        .ToLower()
                                        .Contains(value.ToLower()));
                }
                else if (condition == GridFilterRuleType.DoesNotContain)
                {
                    result = _allItems.Where(i => _itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)
                                        .ToString()
                                        .ToLower()
                                        .Contains(value.ToLower()) == false);
                }

                return result.ToObservableCollection();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<BaseEntity>();
            }
        }

        private ObservableCollection<BaseEntity> FilterItemsByNumberProperty(string propertyName, GridFilterRuleType condition, long value)
        {
            try
            {
                IEnumerable<BaseEntity> result = null;

                if (condition == GridFilterRuleType.IsEqualTo)
                {
                    result = _allItems.Where(i => Convert.ToInt64(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)) == value);
                }
                else if (condition == GridFilterRuleType.IsNotEqualTo)
                {
                    result = _allItems.Where(i => Convert.ToInt64(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)) != value);
                }
                else if (condition == GridFilterRuleType.IsGreaterThanOrEqualTo)
                {
                    result = _allItems.Where(i => Convert.ToInt64(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)) >= value);
                }
                else if (condition == GridFilterRuleType.IsGreaterThan)
                {
                    result = _allItems.Where(i => Convert.ToInt64(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)) > value);
                }
                else if (condition == GridFilterRuleType.IsLessThanOrEqualTo)
                {
                    result = _allItems.Where(i => Convert.ToInt64(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)) <= value);
                }
                else if (condition == GridFilterRuleType.IsLessThan)
                {
                    result = _allItems.Where(i => Convert.ToInt64(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)) < value);
                }

                return result.ToObservableCollection();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<BaseEntity>();
            }
        }

        private ObservableCollection<BaseEntity> FilterItemsByDateTimeProperty(string propertyName, GridFilterRuleType condition, DateTime value)
        {
            try
            {
                IEnumerable<BaseEntity> result = null;

                if (condition == GridFilterRuleType.IsEqualTo)
                {
                    result = _allItems.Where(i => Convert.ToDateTime(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)).Date == value.Date);
                }
                else if (condition == GridFilterRuleType.IsNotEqualTo)
                {
                    result = _allItems.Where(i => Convert.ToDateTime(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)).Date != value.Date);
                }
                else if (condition == GridFilterRuleType.IsGreaterThanOrEqualTo)
                {
                    result = _allItems.Where(i => Convert.ToDateTime(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)).Date >= value.Date);
                }
                else if (condition == GridFilterRuleType.IsGreaterThan)
                {
                    result = _allItems.Where(i => Convert.ToDateTime(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)).Date > value.Date);
                }
                else if (condition == GridFilterRuleType.IsLessThanOrEqualTo)
                {
                    result = _allItems.Where(i => Convert.ToDateTime(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)).Date <= value.Date);
                }
                else if (condition == GridFilterRuleType.IsLessThan)
                {
                    result = _allItems.Where(i => Convert.ToDateTime(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)).Date < value.Date);
                }

                return result.ToObservableCollection();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<BaseEntity>();
            }
        }

        private ObservableCollection<BaseEntity> FilterItemsByBooleanProperty(string propertyName, bool value)
        {
            try
            {
                IEnumerable<BaseEntity> result = null;

                result = _allItems.Where(i => Convert.ToBoolean(_itemSourceType
                                    .GetProperty(propertyName)
                                    .GetValue(i, null)) == value);

                return result.ToObservableCollection();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<BaseEntity>();
            }
        }

        private ObservableCollection<BaseEntity> FilterItemsByEnumProperty(string propertyName, GridFilterRuleType condition, Enum value)
        {
            try
            {
                IEnumerable<BaseEntity> result = null;

                if (condition == GridFilterRuleType.IsEqualTo)
                {
                    result = _allItems.Where(i => Convert.ToInt32(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)) == Convert.ToInt32(value));
                }
                else if (condition == GridFilterRuleType.IsNotEqualTo)
                {
                    result = _allItems.Where(i => Convert.ToInt32(_itemSourceType
                                        .GetProperty(propertyName)
                                        .GetValue(i, null)) != Convert.ToInt32(value));
                }

                return result.ToObservableCollection();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<BaseEntity>();
            }
        }

        #endregion
    }

    public class GridFilterPopupButtonEventArgs : EventArgs
    {
        public ObservableCollection<BaseEntity> FilteredItems { get; set; }
    }
}
