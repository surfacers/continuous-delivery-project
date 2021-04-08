using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using Hurace.Core.Enums;
using Hurace.Core.Models;
using Hurace.Mvvm.Commands;
using Hurace.Mvvm.Enums;
using Hurace.Mvvm.ViewModels;
using Hurace.Mvvm.ViewModels.Controls;
using Hurace.RaceControl.Extensions;
using Hurace.RaceControl.Services;
using Hurace.RaceControl.ViewModels.Controls;
using Unity;
using static Hurace.RaceControl.Extensions.IDropInfoExtensions;

namespace Hurace.RaceControl.ViewModels.Race.Detail
{
    public class RaceDetailStartListViewModel : TabViewModel<IRaceViewModel>, IDropTarget
    {
        private int raceId => this.Parent.Races.Selected.Race.Id;

        [Dependency] public INotificationService NotificationService { get; set; }

        public FilterViewModel<StartListItemViewModel> Skiers { get; set; }
        
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public CommandViewModel StartRaceCommandViewModel { get; }
        public CommandViewModel SaveCommandViewModel { get; }

        public RaceDetailStartListViewModel()
        {
            this.Skiers = new FilterViewModel<StartListItemViewModel>(
                s => $"{s.CountryCode} {s.FullName}");

            this.AddCommand = new DelegateParameterCommand<StartListItemViewModel>(
                skier => this.Add(skier, this.Parent.StartList1.Count),
                _ => this.CanAdd());

            this.RemoveCommand = new DelegateParameterCommand<StartListItemViewModel>(
                skier => this.Remove(skier));

            this.StartRaceCommandViewModel = new CommandViewModel(
                "Start Race", 
                "Start selected race",
                async () => await this.Parent.StartRaceAsync(),
                () => this.Parent.StartList1.Count >= Settings.DefaultSettings.MinSkierAmount,
                withStyle: ButtonStyle.Flat);

            this.StartRaceCommandViewModel.OnSuccess += () => this.NotificationService.ShowMessage("Race started successfully");
            this.StartRaceCommandViewModel.OnFailure += ex => this.NotificationService.ShowMessage("Race start failed");

            this.SaveCommandViewModel = new CommandViewModel(
                "Save", 
                "Save start list",
                async () => await this.Parent.SaveAsync(this.Parent.StartList1));

            this.SaveCommandViewModel.OnSuccess += () => this.NotificationService.ShowMessage("Race saved successfully");
            this.SaveCommandViewModel.OnFailure += ex => this.NotificationService.ShowMessage("Save failed");
        }

        public override Task OnInitAsync()
        {
            Gender gender = this.Parent.Races.Selected.Gender;
            var skiers = this.Parent.AllSkiers.Where(s => s.Gender == gender && s.IsActive);
            var skierViewModels = this.GetSkiers(this.Parent.StartList1, skiers);
            this.Skiers.SetItems(skierViewModels);

            return Task.CompletedTask;
        }

        private IEnumerable<StartListItemViewModel> GetSkiers(
            IEnumerable<StartListItemViewModel> startList,
            IEnumerable<Core.Models.Skier> skiers)
        {
            var skierIdsInStartList = startList.Select(s => s.Skier.Id).Distinct().ToList();
            var notPickedSkiers = skiers.Where(s => !skierIdsInStartList.Contains(s.Id)).ToList();

            return notPickedSkiers
                .Select(skier =>
                {
                    var startList = new Core.Models.StartList
                    {
                        Id = 0,
                        RaceId = this.raceId,
                        RunNumber = 1,
                        SkierId = skier.Id,
                        StartNumber = -1,
                        IsDisqualified = false
                    };

                    return new StartListItemViewModel(skier, startList);
                })
                .ToList();
        }

        private bool CanAdd() => this.Parent.StartList1.Count < Settings.DefaultSettings.MaxSkierAmount;
        private void Add(StartListItemViewModel skier, int newIndex)
        {
            this.Parent.StartList1.Insert(newIndex, skier);
            this.UpdateStartNumbers();

            this.Skiers.Remove(skier);
        }

        private void Remove(StartListItemViewModel skier)
        {
            this.Parent.StartList1.Remove(skier);
            this.UpdateStartNumbers();

            this.Skiers.Add(skier);
        }

        private void Move(StartListItemViewModel skier, int newIndex)
        {
            this.Parent.StartList1.Remove(skier);
            this.Parent.StartList1.Insert(newIndex, skier);
            this.UpdateStartNumbers();
        }

        private void UpdateStartNumbers()
        {
            for (int i = 0; i < this.Parent.StartList1.Count; i++)
            {
                this.Parent.StartList1[i].StartList.StartNumber = i + 1;
                this.Parent.StartList1[i].Raise(nameof(StartListItemViewModel.StartNumber));
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            DragDropAction<StartListItemViewModel> action
                = dropInfo.GetDragDropAction<StartListItemViewModel>(inSource: s => this.Skiers.Data.Contains(s));

            if (action == null)
            {
                return;
            }

            switch (action.Drag, action.Drop)
            {
                case (DragDropSource.Target, DragDropSource.Target): dropInfo.Effects = DragDropEffects.Move; break;
                case (DragDropSource.Target, DragDropSource.Source): dropInfo.Effects = DragDropEffects.Copy; break;
                case (DragDropSource.Source, DragDropSource.Target):
                    dropInfo.Effects = this.CanAdd() ? DragDropEffects.Copy : DragDropEffects.None;
                    break;
                default: dropInfo.Effects = DragDropEffects.None; break;
            }

            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
        }

        public void Drop(IDropInfo dropInfo)
        {
            DragDropAction<StartListItemViewModel> action 
                = dropInfo.GetDragDropAction<StartListItemViewModel>(inSource: s => this.Skiers.Data.Contains(s));

            if (action == null)
            {
                return;
            }

            switch (action.Drag, action.Drop)
            {
                case (DragDropSource.Target, DragDropSource.Target): this.Move(action.Item, dropInfo.GetIndex()); break;
                case (DragDropSource.Target, DragDropSource.Source): this.Remove(action.Item); break;
                case (DragDropSource.Source, DragDropSource.Target): this.Add(action.Item, dropInfo.GetIndex()); break;
                default: throw new NotImplementedException();
            }
        }

        public override Task OnDestroyAsync()
        {
            return Task.CompletedTask;
        }
    }
}
