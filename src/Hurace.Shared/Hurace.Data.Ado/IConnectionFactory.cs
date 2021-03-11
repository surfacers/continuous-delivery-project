using System.Data.Common;
using System.Threading.Tasks;

namespace Hurace.Data.Ado
{
    public interface IConnectionFactory
    {
        string ConnectionString { get; }
        string ProviderName { get; }
        Task<DbConnection> CreateConnectionAsync();
    }
}
