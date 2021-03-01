using System.Threading.Tasks;

namespace Hurace.Mvvm.ViewModels
{
    public interface IComponentViewModel
    {
        Task OnInitAsync();

        Task OnDestroyAsync();
    }
}
