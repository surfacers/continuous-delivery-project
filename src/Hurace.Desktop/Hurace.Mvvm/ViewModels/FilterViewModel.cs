using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Extensions;

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
            get => this.filter;
            set
            {
                this.Set(ref this.filter, value);
                this.FilterList();
            }
        }

        private T selected;
        public T Selected
        {
            get => this.selected;
            set
            {
                this.Set(ref this.selected, value);
                if (this.selected != null)
                {
                    this.OnSelectionChanged(this.selected);
                }
            }
        }

        public ObservableCollection<T> Filtered { get; private set; }
            = new ObservableCollection<T>();

        public ObservableCollection<T> Data { get; private set; }
            = new ObservableCollection<T>();

        public void FilterList()
        {
            string safeFilter = this.filter?.ToLower() ?? string.Empty;

            // Note: cannot clear Filtered list and read it because in some cases
            // the selected item gets then set to null
            foreach (var item in this.Data)
            {
                bool matchesFilter = this.filterPredicate(item).ToLower().Contains(safeFilter);
                bool alreadyInFiltered = this.Filtered.Contains(item);

                switch ((matchesFilter, alreadyInFiltered))
                {
                    case (matchesFilter: true, alreadyInFiltered: false):
                        this.Filtered.Add(item);
                        break;

                    case (matchesFilter: false, alreadyInFiltered: true):
                        this.Filtered.Remove(item);
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
            this.Data.SetItems(items);

            this.Filtered.Clear();            
            this.Filter = null;
            this.FilterList();
        }

        public void Add(T item)
        {
            this.Data.Add(item);
            this.FilterList();
        }

        public void Remove(T item)
        {
            this.Data.Remove(item);
            this.Filtered.Remove(item);
        }

        public void RemoveSelected()
        {
            this.Remove(this.Selected);
            this.Selected = default(T);
        }

        private async void OnSelectionChanged(T item)
        {
            await this.selectionChanged?.Invoke(item);
        }
    }
}
