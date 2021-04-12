using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Core.Enums;
using Hurace.Core.Models;
using SqlKata.Compilers;

namespace Hurace.Data.Ado.Managers
{
    public class SkierManager : GenericManager<Skier>, ISkierManager
    {
        public SkierManager(IMapper mapper, AdoManager ado, Compiler compiler)
            : base(mapper, ado, compiler)
        {
        }

        public Task<IEnumerable<Skier>> GetAllAsync(Gender gender, bool isActive)
        {
            var query = this.Query()
                .Where(nameof(Skier.Gender), gender)
                .Where(nameof(Skier.IsActive), isActive);

            return this.QueryAsync(query);
        }

        public Task<IEnumerable<Skier>> GetAllAsync(bool? isActive = null)
        {
            var query = this.Query();

            if (isActive != null)
            {
                query.Where(nameof(Skier.IsActive), isActive);
            }

            return this.QueryAsync(query);
        }
    }
}
