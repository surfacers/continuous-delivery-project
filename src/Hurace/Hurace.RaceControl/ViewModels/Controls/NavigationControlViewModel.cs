using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Hurace.Core.Extensions;
using Hurace.Mvvm;

namespace Hurace.RaceControl.ViewModels.Controls
{
    public class NavigationControlViewModel<TParentViewModel> : NotifyPropertyChanged
    {
        public ObservableCollection<NavigationItemViewModel<TParentViewModel>> Items { get; set; }
            = new ObservableCollection<NavigationItemViewModel<TParentViewModel>>();

        private NavigationItemViewModel<TParentViewModel> currentItem;
        public NavigationItemViewModel<TParentViewModel> CurrentItem
        {
            get => currentItem;
            set 
            {
                if (currentItem != value)
                {
                    currentItem?.ViewModel.OnDestroyAsync();

                    Set(ref currentItem, value);
                    currentItem?.ViewModel.OnInit();
                }
            }
        }

        public NavigationControlViewModel()
        {
        }

        public NavigationControlViewModel(
            IEnumerable<NavigationItemViewModel<TParentViewModel>> items,
            NavigationItemViewModel<TParentViewModel> currentItem = null)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (!items.Any()) throw new ArgumentException(nameof(items));

            Items = new ObservableCollection<NavigationItemViewModel<TParentViewModel>>(items);
            CurrentItem = currentItem ?? Items.First();
        }

        public void SetItems(
            IEnumerable<NavigationItemViewModel<TParentViewModel>> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (!items.Any()) throw new ArgumentException(nameof(items));

            string lastSelected = currentItem?.Title;
            Items.SetItems(items);

            CurrentItem = Items
                .Where(i => i.Title.Equals(lastSelected))
                .FirstOrDefault();

            if (currentItem == null)
            {
                CurrentItem = items.First();
            }
        }
    }
}
