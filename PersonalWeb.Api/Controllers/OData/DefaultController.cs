using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

namespace PersonalWeb.Api.Controllers.OData
{
    public abstract class DefaultController<TEntity, TRepository> : ODataController
        where TEntity : class, IAuditable, new()
        where TRepository : IDefaultModelRepositories<TEntity>
    {
        private readonly TRepository _repository;

        protected DefaultController(TRepository repository)
        {
            _repository = repository;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_repository.GetAll());
        }

        [EnableQuery]
        public IActionResult Get(Guid key)
        {
            return Ok(_repository.Get(key));
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (entity == null)
            {
                return BadRequest();
            }
            var createdData = await _repository.Create(entity);
            return Created(createdData);
        }

        [EnableQuery]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<TEntity> entity)
        {
            var patchedData = await _repository.Patch(key, entity);
            return Updated(patchedData);
        }

        [EnableQuery]
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            await _repository.Delete(key);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
