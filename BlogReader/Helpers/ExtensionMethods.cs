using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace BlogReader.Helpers
{
    public static class ExtensionMethods
    {
        public static bool IsValidUri(this string uri)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(uri, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            var observableCollection = new ObservableCollection<T>();
            foreach (T item in collection)
            {
                observableCollection.Add(item);
            }

            return observableCollection;
        }

        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> valuesToAdd)
        {
            foreach (T item in valuesToAdd)
            {
                collection.Add(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var cur in enumerable)
            {
                action(cur);
            }
        }
    }
}
