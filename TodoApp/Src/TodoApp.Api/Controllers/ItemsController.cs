using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Web.Http;
using TodoApp.Api.Routes;
using TodoApp.Contracts.Models;
using TodoApp.Contracts.Services;

namespace TodoApp.Api.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/items")]
    public class ItemsController : ApiController
    {
        private readonly IItemService _itemService;
        private readonly IUrlService _urlService;

        public ItemsController(IItemService itemService, IUrlService urlService)
        {
            _itemService = itemService;
            _urlService = urlService;
        }

        // GET: api/v1/items
        [Route]
        public async Task<IHttpActionResult> GetAllItemsAsync()
            => Ok(await _itemService.GetAllItemsAsync());

        // GET: api/v1/items/5
        [Route("{id}", Name = RouteNames.NewItemRouteName)]
        public async Task<IHttpActionResult> GetItemByIdAsync(Guid id)
            => Ok(await _itemService.GetItemByIdAsync(id));

        // POST: api/v1/items
        [Route]
        public async Task<IHttpActionResult> PostItemAsync([FromBody]Item item)
        {
            var createdItem = await _itemService.CreateItemAsync(item);
            var itemUrl = _urlService.GetItemUrl(createdItem.Id);

            return Created(itemUrl, createdItem);
        }

        // PUT: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> PutItemAsync(Guid id, [FromBody]Item item)
        {
            await _itemService.UpdateItemAsync(item);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteItemAsync(Guid id)
            => Ok(await _itemService.DeleteItemAsync(id));
    }
}
