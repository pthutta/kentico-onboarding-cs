using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;
using TodoApp.Api.Routes;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Repositories;
using TodoApp.Contracts.Services;

namespace TodoApp.Api.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/items")]
    public class ItemsController : ApiController
    {
        private readonly IItemRepository _repository;
        private readonly IUrlService _urlService;

        public ItemsController(IItemRepository repository, IUrlService urlService)
        {
            _repository = repository;
            _urlService = urlService;
        }

        // GET: api/v1/items
        [Route]
        public async Task<IHttpActionResult> GetAllItemsAsync()
            => Ok(await _repository.GetAllAsync());

        // GET: api/v1/items/5
        [Route("{id}", Name = RouteNames.NewItemRouteName)]
        public async Task<IHttpActionResult> GetItemByIdAsync(Guid id)
            => Ok(await _repository.GetByIdAsync(id));

        // POST: api/v1/items
        [Route]
        public async Task<IHttpActionResult> PostItemAsync([FromBody]Item item)
        {
            var createdItem = await _repository.CreateAsync(item);
            var itemUrl = _urlService.GetItemUrl(createdItem.Id);

            return Created(itemUrl, createdItem);
        }

        // PUT: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> PutItemAsync(Guid id, [FromBody]Item item)
        {
            await _repository.UpdateAsync(item);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteItemAsync(Guid id)
            => Ok(await _repository.DeleteAsync(id));
    }
}
