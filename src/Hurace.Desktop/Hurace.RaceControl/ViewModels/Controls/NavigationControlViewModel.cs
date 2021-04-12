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
            get => this.currentItem;
            set 
            {
                if (this.currentItem != value)
                {
                    this.currentItem?.ViewModel.OnDestroyAsync();

                    this.Set(ref this.currentItem, value);
                    this.currentItem?.ViewModel.OnInit();
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
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (!items.Any())
            {
                throw new ArgumentException(nameof(items));
            }

            this.Items = new ObservableCollection<NavigationItemViewModel<TParentViewModel>>(items);
            this.CurrentItem = currentItem ?? this.Items.First();
        }

        public void SetItems(
            IEnumerable<NavigationItemViewModel<TParentViewModel>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (!items.Any())
            {
                throw new ArgumentException(nameof(items));
            }

            string lastSelected = this.currentItem?.Title;
            this.Items.SetItems(items);

            this.CurrentItem = this.Items
                .Where(i => i.Title.Equals(lastSelected))
                .FirstOrDefault();

            if (this.currentItem == null)
            {
                this.CurrentItem = items.First();
            }
        }
    }
}
