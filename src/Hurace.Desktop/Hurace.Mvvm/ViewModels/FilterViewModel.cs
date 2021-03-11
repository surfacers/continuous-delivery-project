using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Extensions;
using Hurace.Mvvm;

namespace Hurace.Mvvm.ViewModels
{
    public class FilterViewModel<T> : NotifyPropertyChanged
    {
        private readonly Func<T, string> filterPredicate;
        private readonly Func<T, Task> selectionChanged;

        public FilterViewModel(
            Func<T, string> filterPredicate,
            Func<T, Task> selectionChanged = null)
        {
            this.filterPredicate = filterPredicate ?? throw new ArgumentNullException(nameof(filterPredicate));
            this.selectionChanged = selectionChanged;
        }

        private string filter;
        public string Filter
        {
            get => filter;
            set
            {
                Set(ref filter, value);
                FilterList();
            }
        }

        private T selected;
        public T Selected
        {
            get => selected;
            set
            {
                Set(ref selected, value);
                if (selected != null)
                {
                    OnSelectionChanged(selected);
                }
            }
        }

        public ObservableCollection<T> Filtered { get; private set; }
            = new ObservableCollection<T>();

        public ObservableCollection<T> Data { get; private set; }
            = new ObservableCollection<T>();

        public void FilterList()
        {
            string safeFilter = filter?.ToLower() ?? string.Empty;

            // Note: cannot clear Filtered list and readd it because in some cases
            // the selected item gets then set to null
            foreach (var item in Data)
            {
                bool matchesFilter = filterPredicate(item).ToLower().Contains(safeFilter);
                bool alreadyInFiltered = Filtered.Contains(item);

                switch ((matchesFilter, alreadyInFiltered))
                {
                    case (matchesFilter: true, alreadyInFiltered: false):
                        Filtered.Add(item);
                        break;

                    case (matchesFilter: false, alreadyInFiltered: true):
                        Filtered.Remove(item);
                        break;

                    case (matchesFilter: true, alreadyInFiltered: true): 
                    case (matchesFilter: false, alreadyInFiltered: false):
                        // Nothing to do
                        break;
                }             
            }
        }

        public void SetItems(IEnumerable<T> items)
        {
            Data.SetItems(items);

            Filtered.Clear();            
            Filter = null;
            FilterList();
        }

        public void Add(T item)
        {
            Data.Add(item);
            FilterList();
        }

        public void Remove(T item)
        {
            Data.Remove(item);
            Filtered.Remove(item);
        }

        public void RemoveSelected()
        {
            Remove(Selected);
            Selected = default(T);
        }

        private async void OnSelectionChanged(T item)
        {
            await selectionChanged?.Invoke(item);
        }
    }
}
