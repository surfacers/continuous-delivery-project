using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Enums;
using Hurace.Core.Models;

namespace Hurace.Data
{
    public interface ISkierManager
    {
        Task<Skier> GetByIdAsync(int id);
        Task<IEnumerable<Skier>> GetAllAsync(bool? isActive = null);
        Task<IEnumerable<Skier>> GetAllAsync(Gender gender, bool isActive = true);
        Task CreateAsync(Skier skier);
        Task<bool> UpdateAsync(Skier skier);
        Task<bool> RemoveAsync(int id);
    }
}
