using Microsoft.AspNetCore.OData.Deltas;
using PersonalWeb.Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWeb.Api.Repositories
{
    public interface IDefaultModelRepositories<TEntity>
        where TEntity : class, IAuditable
    {
        IQueryable<TEntity> GetAll();
        TEntity Get(Guid key);
        Task<TEntity> Create(TEntity data);
        Task<TEntity> Patch(Guid key, Delta<TEntity> updatedData);
        Task<TEntity> Put(Guid key, TEntity updatedData);
        Task Delete(Guid key);
    }
}
