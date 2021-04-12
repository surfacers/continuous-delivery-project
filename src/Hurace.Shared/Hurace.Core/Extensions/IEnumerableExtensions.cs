using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Hurace.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IList<T> items, Action<T, int> action)
        {
            for (int i = 0; i < items.Count; i++)
            {
                action(items[i], i);
            }
        }

        public static void SetItems<T>(this ObservableCollection<T> items, IEnumerable<T> newItems)
        {
            items.Clear();
            foreach (var item in newItems)
            {
                items.Add(item);
            }
        }

        public static void SetItems<TFrom, T>(this ObservableCollection<T> items, IEnumerable<TFrom> newItems, Func<TFrom, T> map)
            => SetItems(items, newItems.Select(n => map(n)));
    }
}
