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
            get => this.editSkier;
            set => this.Set(ref this.editSkier, value);
        }

        public SkierViewModel()
        {
            this.SkierListViewModel = new SkierListViewModel(this);
            this.SkierDetailViewModel = new SkierDetailViewModel(this);
            this.Skiers = new FilterViewModel<SkierListItemViewModel>(
                s => $"{s.CountryCode} {s.FullName}",
                this.SkierChanged);
        }

        private Task SkierChanged(SkierListItemViewModel skier)
        {
            this.Edit = this.mapper.Map<SkierEditViewModel>(skier.Skier);
            return Task.CompletedTask;
        }

        public override async Task OnInitAsync()
        {
            var skiers = await this.SkierLogic.GetAllAsync();
            var skierViewModels = skiers.Select(s => new SkierListItemViewModel(s)).ToList();
            this.Skiers.SetItems(skierViewModels);

            await this.SkierDetailViewModel.OnInitAsync();
        }

        public async Task SaveAsync()
        {
            Models.Skier skier = this.mapper.Map<Models.Skier>(this.Edit);
            var result = await this.SkierLogic.SaveAsync(skier);

            if (result.IsSuccess)
            {
                this.Skiers.Selected.Update(skier);

                if (this.Edit.Original.Id == 0)
                {
                    this.Skiers.Add(this.Skiers.Selected);
                }

                this.Edit.Original = skier;
                this.Edit.Raise(nameof(SkierEditViewModel.DisplayName));
            }
        }

        public async Task RemoveAsync()
        {
            bool successful = await this.SkierLogic.RemoveAsync(this.Edit.Original.Id);
            if (successful)
            {
                this.Skiers.RemoveSelected();
                this.Edit = null;
            }
        }

        public Task NewAsync()
        {
            this.Skiers.Selected = new SkierListItemViewModel(new Models.Skier());
            return Task.CompletedTask;
        }

        public override Task OnDestroyAsync()
        {
            return Task.CompletedTask;
        }
    }
}
