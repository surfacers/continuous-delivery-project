using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Core.Models;
using SqlKata;
using SqlKata.Compilers;

namespace Hurace.Data.Ado.Managers
{
    public class LocationManager : GenericManager<Location>, ILocationManager
    {
        public LocationManager(IMapper mapper, AdoManager ado, Compiler compiler)
            : base(mapper, ado, compiler)
        {
        }

        public Task<IEnumerable<Location>> GetAllByIdsAsync(IEnumerable<int> ids)
        {
            Query query = this.Query().WhereIn(nameof(Location.Id), ids);
            return this.QueryAsync(query);
        }
    }
}
