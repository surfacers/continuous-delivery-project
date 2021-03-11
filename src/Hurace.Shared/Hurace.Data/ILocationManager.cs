using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Models;

namespace Hurace.Data
{
    public interface ILocationManager
    {
        Task<IEnumerable<Location>> GetAllAsync();

        Task<IEnumerable<Location>> GetAllByIdsAsync(IEnumerable<int> ids);
    }
}
