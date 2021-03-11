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
            => Original.Id == 0 ? "New Race" : "Edit Race";

        private string name;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => Set(ref description, value);
        }

        private ComboBoxItemViewModel<RaceType> raceType;
        public ComboBoxItemViewModel<RaceType> RaceType
        {
            get => raceType;
            set => Set(ref raceType, value);
        }

        private DateTime? raceDate;
        public DateTime? RaceDate
        {
            get => raceDate;
            set => Set(ref raceDate, value);
        }

        private ComboBoxItemViewModel<int> locationId;
        public ComboBoxItemViewModel<int> LocationId
        {
            get => locationId;
            set => Set(ref locationId, value);
        }

        private int sensorAmount;
        public int SensorAmount
        {
            get => sensorAmount;
            set => Set(ref sensorAmount, value);
        }

        private Gender gender;
        public Gender Gender
        {
            get => gender;
            set => Set(ref gender, value);
        }

        private RaceState raceState;
        public RaceState RaceState
        {
            get => raceState;
            set => Set(ref raceState, value);
        }
    }
}
