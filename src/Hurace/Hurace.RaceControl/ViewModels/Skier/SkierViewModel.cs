using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Core.Logic;
using Hurace.Mvvm.ViewModels;
using Hurace.RaceControl.ViewModels.Controls;
using Unity;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.Skier
{
    public class SkierViewModel : TabViewModel<MainViewModel>, ISkierViewModel
    {
        [Dependency] public IMapper mapper { get; set; }
        [Dependency] public ISkierLogic SkierLogic { get; set; }
                
        public SkierListViewModel SkierListViewModel { get; set; }
        public SkierDetailViewModel SkierDetailViewModel { get; set; }

        public FilterViewModel<SkierListItemViewModel> Skiers { get; set; }

        private SkierEditViewModel editSkier;
        public SkierEditViewModel Edit
        { 
            get => editSkier;
            set => Set(ref editSkier, value);
        }

        public SkierViewModel()
        {
            SkierListViewModel = new SkierListViewModel(this);
            SkierDetailViewModel = new SkierDetailViewModel(this);
            Skiers = new FilterViewModel<SkierListItemViewModel>(
                s => $"{s.CountryCode} {s.FullName}",
                SkierChanged);
        }

        private Task SkierChanged(SkierListItemViewModel skier)
        {
            Edit = mapper.Map<SkierEditViewModel>(skier.Skier);
            return Task.CompletedTask;
        }

        public override async Task OnInitAsync()
        {
            var skiers = await SkierLogic.GetAllAsync();
            var skierViewModels = skiers.Select(s => new SkierListItemViewModel(s)).ToList();
            Skiers.SetItems(skierViewModels);

            await SkierDetailViewModel.OnInitAsync();
        }

        public async Task SaveAsync()
        {
            Models.Skier skier = mapper.Map<Models.Skier>(Edit);
            var result = await SkierLogic.SaveAsync(skier);

            if (result.IsSuccess)
            {
                Skiers.Selected.Update(skier);

                if (Edit.Original.Id == 0)
                {
                    Skiers.Add(Skiers.Selected);
                }

                Edit.Original = skier;
                Edit.Raise(nameof(SkierEditViewModel.DisplayName));
            }
        }

        public async Task RemoveAsync()
        {
            bool successful = await SkierLogic.RemoveAsync(Edit.Original.Id);
            if (successful)
            {
                Skiers.RemoveSelected();
                Edit = null;
            }
        }

        public Task NewAsync()
        {
            Skiers.Selected = new SkierListItemViewModel(new Models.Skier());
            return Task.CompletedTask;
        }

        public override Task OnDestroyAsync()
        {
            return Task.CompletedTask;
        }
    }
}
