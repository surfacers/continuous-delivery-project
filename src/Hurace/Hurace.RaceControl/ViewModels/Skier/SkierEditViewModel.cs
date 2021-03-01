using System;
using AutoMapper;
using Hurace.Core.Enums;
using Hurace.Core.Validators;
using Hurace.Mvvm.ViewModels;
using Unity;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.Skier
{
    public class SkierEditViewModel : EditViewModel<Models.Skier, SkierValidator>
    {
        public SkierEditViewModel()
            : base(App.Container.Resolve<IMapper>())
        {
        }

        public string DisplayName
            => Original.Id == 0 ? "New Skier" : "Edit Skier";

        private string firstName;
        public string FirstName
        {
            get => firstName;
            set => Set(ref firstName, value);
        }

        private string lastName;
        public string LastName
        {
            get => lastName;
            set => Set(ref lastName, value);
        }

        private string countryCode;
        public string CountryCode
        {
            get => countryCode;
            set => Set(ref countryCode, value);
        }

        private DateTime? birthDate;
        public DateTime? BirthDate
        {
            get => birthDate;
            set => Set(ref birthDate, value);
        }

        private Gender gender;
        public Gender Gender
        {
            get => gender;
            set => Set(ref gender, value);
        }

        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set => Set(ref isActive, value);
        }

        private string image;
        public string Image
        {
            get => image;
            set => Set(ref image, value);
        }

    }
}
