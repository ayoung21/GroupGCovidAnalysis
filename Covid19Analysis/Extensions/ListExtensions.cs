using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Covid19Analysis.Extensions
{
    /// <summary>
    /// Extends the i enumerable interface
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Converts to observablecollection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }
}
