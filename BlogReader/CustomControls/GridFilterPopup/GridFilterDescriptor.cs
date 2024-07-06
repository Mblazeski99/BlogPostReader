using BlogReader.DataModels;
using BlogReader.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BlogReader.CustomControls.GridFilterPopup
{
    public class GridFilterDescriptor
    {
        public ObservableCollection<BaseEntity> ItemsSource { get; } = new ObservableCollection<BaseEntity>();
        public ObservableCollection<GridFilterDescriptorProperty> PropertyNames { get; }

        public GridFilterDescriptor(ObservableCollection<BaseEntity> itemsSource, ObservableCollection<GridFilterDescriptorProperty> propertyNames = null)
        {
            try
            {
                ItemsSource = itemsSource;
                PropertyNames = propertyNames;

                if ((PropertyNames == null || PropertyNames.Count == 0)
                    && ItemsSource != null && ItemsSource.Count > 0)
                {
                    var item = ItemsSource[0];
                    var properties = item.GetType().GetProperties();

                    PropertyNames = properties.Select(p => new GridFilterDescriptorProperty(p.Name, p.Name)).ToObservableCollection();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }

    public class GridFilterDescriptorProperty
    {
        public string PropertyName { get; set; }
        public string DisplayName { get; set; }

        public GridFilterDescriptorProperty(string propertyName, string displayName)
        {
            PropertyName = propertyName;
            DisplayName = displayName;
        }
    }
}
