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
        private readonly IItemService _itemService;
        private readonly IItemRepository _itemRepository;
        private readonly IUrlService _urlService;

        public ItemsController(IItemService itemService, IItemRepository itemRepository, IUrlService urlService)
        {
            _itemService = itemService;
            _itemRepository = itemRepository;
            _urlService = urlService;
        }

        // GET: api/v1/items
        [Route]
        public async Task<IHttpActionResult> GetAllItemsAsync()
            => Ok(await _itemRepository.GetAllAsync());

        // GET: api/v1/items/5
        [Route("{id}", Name = RouteNames.NewItemRouteName)]
        public async Task<IHttpActionResult> GetItemByIdAsync(Guid id)
        {
            var item = await _itemService.GetItemByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST: api/v1/items
        [Route]
        public async Task<IHttpActionResult> PostItemAsync([FromBody]Item item)
        {
            if (!PostIsItemValid(item))
            {
                return BadRequest(ModelState);
            }

            var createdItem = await _itemService.CreateItemAsync(item);
            var itemUrl = _urlService.GetItemUrl(createdItem.Id);

            return Created(itemUrl, createdItem);
        }

        // PUT: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> PutItemAsync(Guid id, [FromBody]Item item)
        {
            if (!IsItemValid(item))
            {
                return BadRequest(ModelState);
            }

            if (item.Id != id)
            {
                return Conflict();
            }

            if (!await _itemService.UpdateItemAsync(item))
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteItemAsync(Guid id)
        {
            var deletedItem = await _itemRepository.DeleteAsync(id);

            if (deletedItem == null)
            {
                return NotFound();
            }
            return Ok(deletedItem);
        }

        private bool PostIsItemValid(Item item)
        {
            IsItemValid(item);

            if (item != null && item.Id != Guid.Empty)
            {
                ModelState.AddModelError("item.Id", "Item id must not be set.");
            }

            return ModelState.IsValid;
        }

        private bool IsItemValid(Item item)
        {
            if (item == null)
            {
                ModelState.AddModelError("item", "Provided item is null.");
                return false;
            }
            if (item.Text.Trim() == string.Empty)
            {
                ModelState.AddModelError("item.Text", "Item text must be non-empty.");
            }

            return ModelState.IsValid;
        }
    }
}
