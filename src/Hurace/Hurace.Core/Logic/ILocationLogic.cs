using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Models;

namespace Hurace.Core.Logic
{
    public interface ILocationLogic
    {
        Task<IEnumerable<Location>> GetAllAsync();

        Task<IEnumerable<string>> GetCountriesAsync();
    }
}
