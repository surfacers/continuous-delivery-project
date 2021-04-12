using System;
using AutoMapper;
using Hurace.Core.Enums;
using Hurace.Core.Validators;
using Hurace.Mvvm.ViewModels;
using Hurace.RaceControl.ViewModels.Controls;
using Unity;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.Race
{
    public class RaceEditViewModel : EditViewModel<Models.Race, RaceValidator>
    {
        public RaceEditViewModel()
            : base(App.Container.Resolve<IMapper>())
        {
        }

        public string DisplayName 
            => this.Original.Id == 0 ? "New Race" : "Edit Race";

        private string name;
        public string Name
        {
            get => this.name;
            set => this.Set(ref this.name, value);
        }

        private string description;
        public string Description
        {
            get => this.description;
            set => this.Set(ref this.description, value);
        }

        private ComboBoxItemViewModel<RaceType> raceType;
        public ComboBoxItemViewModel<RaceType> RaceType
        {
            get => this.raceType;
            set => this.Set(ref this.raceType, value);
        }

        private DateTime? raceDate;
        public DateTime? RaceDate
        {
            get => this.raceDate;
            set => this.Set(ref this.raceDate, value);
        }

        private ComboBoxItemViewModel<int> locationId;
        public ComboBoxItemViewModel<int> LocationId
        {
            get => this.locationId;
            set => this.Set(ref this.locationId, value);
        }

        private int sensorAmount;
        public int SensorAmount
        {
            get => this.sensorAmount;
            set => this.Set(ref this.sensorAmount, value);
        }

        private Gender gender;
        public Gender Gender
        {
            get => this.gender;
            set => this.Set(ref this.gender, value);
        }

        private RaceState raceState;
        public RaceState RaceState
        {
            get => this.raceState;
            set => this.Set(ref this.raceState, value);
        }
    }
}
