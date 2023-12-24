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
        public ObservableCollection<string> PropertyNames { get; }

        public GridFilterDescriptor(ObservableCollection<BaseEntity> itemsSource, ObservableCollection<string> propertyNames = null)
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
                    PropertyNames = properties.Select(p => p.Name).ToObservableCollection();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
