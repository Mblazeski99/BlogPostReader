using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace BlogReader.Helpers
{
    public static class ExtensionMethods
    {
        public static bool IsValidUri(this string uri)
        {
            bool result = false;

            try
            {
                Uri uriResult;
                result = Uri.TryCreate(uri, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            }
            catch (Exception ex) { }

            return result;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            var observableCollection = new ObservableCollection<T>();
            
            try
            {
                foreach (T item in collection)
                {
                    observableCollection.Add(item);
                }
            }
            catch (Exception ex) { }

            return observableCollection;
        }

        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> valuesToAdd)
        {
            try
            {
                foreach (T item in valuesToAdd)
                {
                    collection.Add(item);
                }
            }
            catch (Exception ex) { }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            try
            {
                foreach (var cur in enumerable)
                {
                    action(cur);
                }
            }
            catch (Exception ex) { }
        }
    }
}
