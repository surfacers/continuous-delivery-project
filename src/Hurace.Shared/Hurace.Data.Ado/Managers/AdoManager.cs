using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Data.Ado.Queries;

namespace Hurace.Data.Ado
{
    public class AdoManager
    {
        private readonly IConnectionFactory connectionFactory;

        public AdoManager(
            IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException();
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query, RowMapper<T> mapper, params QueryParameter[] parameters)
        {
            using (var connection = await this.connectionFactory.CreateConnectionAsync())
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                this.AddParameters(command, parameters);

                var items = new List<T>();
                using (DbDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        items.Add(mapper(reader));
                    }
                }

                return items;
            }
        }

        public async Task<T> QueryFirstAsync<T>(string query, RowMapper<T> mapper, params QueryParameter[] parameters)
        {
            return (await this.QueryAsync(query, mapper, parameters)).FirstOrDefault();
        }

        public async Task<int> ExecuteAsync(string query, params QueryParameter[] parameters)
        {
            using (var connection = await this.connectionFactory.CreateConnectionAsync())
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                this.AddParameters(command, parameters);

                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<T> ExecuteAsync<T>(string query, DbAction<T> action, params QueryParameter[] parameters)
        {
            using (var connection = await this.connectionFactory.CreateConnectionAsync())
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                this.AddParameters(command, parameters);

                return await action(command);
            }
        }

        private void AddParameters(DbCommand command, QueryParameter[] parameters)
        {
            foreach (var p in parameters)
            {
                var dbParam = command.CreateParameter();
                dbParam.ParameterName = p.Name;
                dbParam.Value = p.Value != null
                    ? p.Value
                    : DBNull.Value;

                command.Parameters.Add(dbParam);
            }
        }
    }
}
