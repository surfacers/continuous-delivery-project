using System.Threading.Tasks;
using Hurace.Mvvm.ViewModels;

namespace Hurace.RaceControl.ViewModels.Skier
{
    public interface ISkierViewModel
    {
        FilterViewModel<SkierListItemViewModel> Skiers { get; set; }
        
        SkierEditViewModel Edit { get; set; }

        Task SaveAsync();

        Task RemoveAsync();

        Task NewAsync();
    }
}
