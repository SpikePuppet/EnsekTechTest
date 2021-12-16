using MeterReadingServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeterReadingWebApi.Controllers
{
    [ApiController]
    public class EntityController<T> : ControllerBase where T : class, new()
    {
        private readonly IEntityService<T> _entityService;

        public EntityController(IEntityService<T> entityService)
        {
            _entityService = entityService;
        }

        [HttpGet]
        public IActionResult Get(int Id)
        {
            var entities = _entityService.Get(new List<int> { Id });

            if (entities == null)
            {
                return NotFound();
            }

            return Ok(entities);
        }

        [HttpPost]
        public IActionResult Post([FromBody] T entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            _entityService.Add(new List<T> {entity});

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] T entity)
        {
            _entityService.Delete(new List<T>{entity});

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] T entity)
        {
            _entityService.Update(new List<T>{entity});

            return Ok();
        }
    }
}