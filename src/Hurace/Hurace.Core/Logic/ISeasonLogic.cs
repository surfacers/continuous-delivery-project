using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Models;

namespace Hurace.Core.Logic
{
    public interface ISeasonLogic
    {
        Task<IEnumerable<Season>> GetAllAsync();
    }
}
