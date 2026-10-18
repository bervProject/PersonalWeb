using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            var actor = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                ?? User.FindFirst("email")?.Value
                ?? "system";

            entity.Id = Guid.NewGuid();
            entity.CreateBy = actor;
            entity.CreatedDate = DateTimeOffset.UtcNow;
            entity.IsDeleted = false;

            ModelState.Clear();
            TryValidateModel(entity);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdData = await _repository.Create(entity);
            return Created(createdData);
        }

        [EnableQuery]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<TEntity> entity)
        {
            var actor = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                ?? User.FindFirst("email")?.Value
                ?? "system";

            entity.TrySetPropertyValue("UpdatedBy", actor);
            entity.TrySetPropertyValue("UpdatedDate", DateTimeOffset.UtcNow);

            var patchedData = await _repository.Patch(key, entity);
            return Updated(patchedData);
        }

        [EnableQuery]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            await _repository.Delete(key);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
