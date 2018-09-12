using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;

namespace TodoApp.Api.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/items")]
    public class ItemsController : ApiController
    {
        private readonly IItemRepository _repository;

        public ItemsController(IItemRepository repository)
        {
            _repository = repository;
        }

        // GET: api/v1/items
        [Route]
        public async Task<IHttpActionResult> GetAllItemsAsync()
            => Ok(await _repository.GetAllAsync());

        // GET: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> GetItemByIdAsync(string id)
            => Ok(await _repository.GetByIdAsync(id));
        

        // POST: api/v1/items
        [Route("", Name = "PostNewItem")]
        public async Task<IHttpActionResult> PostItemAsync([FromBody]Item item)
        {
            await _repository.CreateAsync(item);
            return CreatedAtRoute("PostNewItem", new { id = item.Id }, item);
        }

        // PUT: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> PutItemAsync(string id, [FromBody]Item value)
        {
            await _repository.UpdateAsync(value);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteItemAsync(string id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item != null)
            {
                await _repository.DeleteAsync(item);
            }
            return Ok(item);
        }
    }
}
