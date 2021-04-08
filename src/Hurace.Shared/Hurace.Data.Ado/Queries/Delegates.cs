using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Hurace.Data.Ado.Queries
{
    public delegate T RowMapper<T>(IDataRecord row);
    public delegate Task<T> DbAction<T>(DbCommand command);
}
