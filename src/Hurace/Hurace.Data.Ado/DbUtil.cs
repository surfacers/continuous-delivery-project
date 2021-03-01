using System;
using System.Data.Common;
using SqlKata.Compilers;

namespace Hurace.Data.Ado
{
    public static class DbUtil
    {
        public static DbProviderFactory GetDbProvicerFactory(string providerName)
        {
            switch (providerName)
            {
                case "Microsoft.Data.SqlClient": return Microsoft.Data.SqlClient.SqlClientFactory.Instance;
                case "MySql.Data.MySqlClient": return MySql.Data.MySqlClient.MySqlClientFactory.Instance;
                default: throw new ArgumentException($"Invalid provider name \"{providerName}\"");
            }
        }

        public static Compiler GetCompiler(string providerName)
        {
            switch (providerName)
            {
                case "Microsoft.Data.SqlClient": return new SqlServerCompiler();
                case "MySql.Data.MySqlClient": return new MySqlCompiler();
                default: throw new ArgumentException($"Invalid provider name \"{providerName}\"");
            }
        }
    }
}
