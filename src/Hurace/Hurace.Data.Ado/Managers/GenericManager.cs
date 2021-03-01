using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Core.Models.Interfaces;
using Hurace.Data.Ado.Queries;
using SqlKata;
using SqlKata.Compilers;

namespace Hurace.Data.Ado.Managers
{
    public class GenericManager<T>
        where T : IEntity
    {
        private readonly IMapper mapper;
        private readonly AdoManager ado;
        private readonly Compiler compiler; 

        public GenericManager(
            IMapper mapper,
            AdoManager ado,
            Compiler compiler)
        {
            this.mapper = mapper ?? throw new ArgumentNullException();
            this.ado = ado ?? throw new ArgumentNullException();
            this.compiler = compiler ?? throw new ArgumentNullException();
        }

        public async Task<IEnumerable<T>> QueryAsync(Query query)
        {
            (string sql, IEnumerable<QueryParameter> queryParams) = query.AsSqlQuery(compiler);
            return await ado.QueryAsync(sql, mapper.Map<T>, queryParams.ToArray());
        }

        public async Task<T> QueryFirstAsync(Query query)
        {
            (string sql, IEnumerable<QueryParameter> queryParams) = query.AsSqlQuery(compiler);
            return await ado.QueryFirstAsync(sql, mapper.Map<T>, queryParams.ToArray());
        }

        public async Task<int> ExecuteAsync(Query query)
        {
            (string sql, IEnumerable<QueryParameter> queryParams) = query.AsSqlQuery(compiler);
            return await ado.ExecuteAsync(sql, queryParams.ToArray());
        }

        public async Task<R> ExecuteAsync<R>(Query query, DbAction<R> action)
        {
            (string sql, IEnumerable<QueryParameter> queryParams) = query.AsSqlQuery(compiler);
            return await ado.ExecuteAsync<R>(sql, action, queryParams.ToArray());
        }

        public async Task CreateAsync(T entity)
        {
            Query query = Query().AsEntityInsert(entity);
            DbAction<int> fetchId = async command => Convert.ToInt32(await command.ExecuteScalarAsync());

            entity.Id = await ExecuteAsync(query, fetchId);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            Query query = typeof(IRemovable).IsAssignableFrom(typeof(T))
                ? Query(id).AsUpdate(nameof(IRemovable.IsRemoved), true) // Only set flag
                : Query(id).AsDelete();

            return (await ExecuteAsync(query)) == 1;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            Query query = Query(entity.Id).AsEntityUpdate(entity);
            return (await ExecuteAsync(query)) == 1;
        }

        public Task<IEnumerable<T>> GetAllAsync() => QueryAsync(Query());
        public Task<T> GetByIdAsync(int id) => QueryFirstAsync(Query(id));

        public Query Query() => new Query(typeof(T).Name);
        public Query Query(int id) => Query().Where(nameof(IEntity.Id), id);
    }
}
