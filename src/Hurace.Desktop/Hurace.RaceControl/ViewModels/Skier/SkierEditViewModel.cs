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
            => this.Original.Id == 0 ? "New Skier" : "Edit Skier";

        private string firstName;
        public string FirstName
        {
            get => this.firstName;
            set => this.Set(ref this.firstName, value);
        }

        private string lastName;
        public string LastName
        {
            get => this.lastName;
            set => this.Set(ref this.lastName, value);
        }

        private string countryCode;
        public string CountryCode
        {
            get => this.countryCode;
            set => this.Set(ref this.countryCode, value);
        }

        private DateTime? birthDate;
        public DateTime? BirthDate
        {
            get => this.birthDate;
            set => this.Set(ref this.birthDate, value);
        }

        private Gender gender;
        public Gender Gender
        {
            get => this.gender;
            set => this.Set(ref this.gender, value);
        }

        private bool isActive;
        public bool IsActive
        {
            get => this.isActive;
            set => this.Set(ref this.isActive, value);
        }

        private string image;
        public string Image
        {
            get => this.image;
            set => this.Set(ref this.image, value);
        }
    }
}
