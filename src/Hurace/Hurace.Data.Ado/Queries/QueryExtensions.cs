using System.Collections.Generic;
using System.Linq;
using Hurace.Core.Models.Interfaces;
using SqlKata;
using SqlKata.Compilers;

namespace Hurace.Data.Ado.Queries
{
    internal static class QueryExtensions
    {
        internal static (string query, IEnumerable<QueryParameter> queryParams) AsSqlQuery(
            this Query query, 
            Compiler compiler)
        {
            var result = compiler.Compile(query);
            var parameters = result.Bindings.Select((value, i) => new QueryParameter($"@p{i}", value)).ToList();

            return (result.Sql, parameters);
        }

        internal static Query AsUpdate(this Query query, string column, object value)
            => query.AsUpdate(new[] { column }, new[] { value });

        internal static Query SelectOnly<T>(this Query query)
            => query.Select($"{typeof(T).Name}.*");

        internal static Query RemoveColumn(this Query query, string column)
        {
            InsertClause clause = (InsertClause)query.Clauses.FirstOrDefault(m => m is InsertClause);
            int idIndex = clause.Columns.IndexOf(column);
            clause.Columns.RemoveAt(idIndex);
            clause.Values.RemoveAt(idIndex);

            return query;
        }

        internal static Query AsEntityInsert<T>(this Query query, T entity, bool returnId = true)
            where T : IEntity
            => query.AsInsert(entity, returnId).RemoveColumn(nameof(IEntity.Id));

        internal static Query AsEntityUpdate<T>(this Query query, T entity)
            where T : IEntity
            => query.AsUpdate(entity).RemoveColumn(nameof(IEntity.Id));
    }
}
