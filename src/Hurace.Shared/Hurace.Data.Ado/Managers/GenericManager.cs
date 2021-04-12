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
            (string sql, IEnumerable<QueryParameter> queryParams) = query.AsSqlQuery(this.compiler);
            return await this.ado.QueryAsync(sql, this.mapper.Map<T>, queryParams.ToArray());
        }

        public async Task<T> QueryFirstAsync(Query query)
        {
            (string sql, IEnumerable<QueryParameter> queryParams) = query.AsSqlQuery(this.compiler);
            return await this.ado.QueryFirstAsync(sql, this.mapper.Map<T>, queryParams.ToArray());
        }

        public async Task<int> ExecuteAsync(Query query)
        {
            (string sql, IEnumerable<QueryParameter> queryParams) = query.AsSqlQuery(this.compiler);
            return await this.ado.ExecuteAsync(sql, queryParams.ToArray());
        }

        public async Task<TRow> ExecuteAsync<TRow>(Query query, DbAction<TRow> action)
        {
            (string sql, IEnumerable<QueryParameter> queryParams) = query.AsSqlQuery(this.compiler);
            return await this.ado.ExecuteAsync<TRow>(sql, action, queryParams.ToArray());
        }

        public async Task CreateAsync(T entity)
        {
            Query query = this.Query().AsEntityInsert(entity);
            DbAction<int> fetchId = async command => Convert.ToInt32(await command.ExecuteScalarAsync());

            entity.Id = await this.ExecuteAsync(query, fetchId);
        }

        public virtual async Task<bool> RemoveAsync(int id)
        {
            Query query = typeof(IRemovable).IsAssignableFrom(typeof(T))
                ? this.Query(id).AsUpdate(nameof(IRemovable.IsRemoved), true) // Only set flag
                : this.Query(id).AsDelete();

            return (await this.ExecuteAsync(query)) == 1;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            Query query = this.Query(entity.Id).AsEntityUpdate(entity);
            return (await this.ExecuteAsync(query)) == 1;
        }

        public Task<IEnumerable<T>> GetAllAsync() => this.QueryAsync(this.Query());
        public Task<T> GetByIdAsync(int id) => this.QueryFirstAsync(this.Query(id));

        public Query Query() => new Query(typeof(T).Name);
        public Query Query(int id) => this.Query().Where(nameof(IEntity.Id), id);
    }
}
