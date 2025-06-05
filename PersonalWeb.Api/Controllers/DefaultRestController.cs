using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;
using System;
using System.Threading.Tasks;

namespace PersonalWeb.Api.Controllers
{
    public abstract class DefaultRestController<TEntity, TRepository> : ControllerBase
        where TEntity : class, IAuditable
        where TRepository : IDefaultModelRepositories<TEntity>
    {
        private readonly TRepository _repository;

        protected DefaultRestController(TRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_repository.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            var createdData = await _repository.Create(entity);
            return Created(createdData.Id.ToString(), createdData);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid key, [FromBody] TEntity entity)
        {
            var patchedData = await _repository.Put(key, entity);
            return Ok(patchedData);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid key)
        {
            await _repository.Delete(key);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
