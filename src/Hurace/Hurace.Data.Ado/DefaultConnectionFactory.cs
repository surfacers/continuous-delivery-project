using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Hurace.Data.Ado
{
    public class DefaultConnectionFactory : IConnectionFactory
    {
        private readonly DbProviderFactory dbProviderFactory;

        public static IConnectionFactory FromConfiguration(IConfiguration config, string connectionStringConfigName)
        {
            var connectionConfig = config.GetSection("ConnectionStrings").GetSection(connectionStringConfigName);
            string connectionString = connectionConfig["ConnectionString"];
            string providerName = connectionConfig["ProviderName"];

            return new DefaultConnectionFactory(connectionString, providerName);
        }

        public DefaultConnectionFactory(string connectionString, string providerName)
        {
            ConnectionString = connectionString;
            ProviderName = providerName;
            dbProviderFactory = DbUtil.GetDbProvicerFactory(providerName);
        }

        public string ConnectionString { get; }

        public string ProviderName { get; }

        public async Task<DbConnection> CreateConnectionAsync()
        {
            var conn = dbProviderFactory.CreateConnection();
            conn.ConnectionString = ConnectionString;
            await conn.OpenAsync();

            return conn;
        }
    }
}
