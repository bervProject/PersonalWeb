using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.EntityFrameworkCore;
using PersonalWeb.Api.Exceptions;
using PersonalWeb.Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWeb.Api.Repositories
{
    public class DefaultModelRepositories<TEntity, TContext> : IDefaultModelRepositories<TEntity>
        where TEntity : class, IAuditable
        where TContext : DbContext
    {
        private readonly TContext _dbContext;
        public DefaultModelRepositories(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().Where(x => !x.IsDeleted);
        }

        public TEntity Get(Guid key)
        {
            return GetAll().SingleOrDefault(x => x.Id == key);
        }

        public async Task<TEntity> Create(TEntity data)
        {
            _dbContext.Set<TEntity>().Add(data);
            await _dbContext.SaveChangesAsync();
            return data;
        }

        public async Task<TEntity> Patch(Guid key, Delta<TEntity> updatedData)
        {
            var existing = GetOrException(key);
            updatedData.Patch(existing);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DataNotExists(key))
                {
                    throw new EntityNotFoundException();
                }
                else
                {
                    throw;
                }
            }
            return existing;
        }

        public async Task<TEntity> Put(Guid key, TEntity updatedData)
        {
            if (key != updatedData.Id)
            {
                throw new EntityNotFoundException();
            }

            _dbContext.Entry(updatedData).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DataNotExists(key))
                {
                    throw new EntityNotFoundException();
                }
                else
                {
                    throw;
                }
            }

            return updatedData;
        }


        public async Task Delete(Guid key)
        {
            var existing = GetOrException(key);
            existing.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }

        private TEntity GetOrException(Guid key)
        {
            var existing = Get(key);
            if (existing == null)
            {
                throw new EntityNotFoundException();
            }
            return existing;
        }

        private bool DataNotExists(Guid key)
        {
            return _dbContext.Set<TEntity>().Any(x => !x.IsDeleted && x.Id == key);
        }
    }
}
