using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Enums;
using Hurace.Core.Logic.Models;
using Hurace.Core.Models;

namespace Hurace.Core.Logic
{
    public interface ISkierLogic
    {
        Task<IEnumerable<Skier>> GetAllAsync(Gender gender, bool isActive = true);

        Task<IEnumerable<Skier>> GetAllAsync(bool? isActive = null);

        Task<Skier> GetByIdAsync(int id);

        Task<bool> RemoveAsync(int id);

        Task<SaveResult> SaveAsync(Skier skier);
    }
}
