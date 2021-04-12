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
            this.ConnectionString = connectionString;
            this.ProviderName = providerName;
            this.dbProviderFactory = DbUtil.GetDbProvicerFactory(providerName);
        }

        public string ConnectionString { get; }

        public string ProviderName { get; }

        public async Task<DbConnection> CreateConnectionAsync()
        {
            var conn = this.dbProviderFactory.CreateConnection();
            conn.ConnectionString = this.ConnectionString;
            await conn.OpenAsync();

            return conn;
        }
    }
}
