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
        private readonly IItemObtainingService _itemObtainingService;
        private readonly IItemCreatingService _itemCreatingService;
        private readonly IItemUpdatingService _itemUpdatingService;
        private readonly IItemRepository _itemRepository;
        private readonly IUrlService _urlService;

        public ItemsController(IItemObtainingService itemObtainingObtainingService, IItemCreatingService itemCreatingService,
            IItemUpdatingService itemUpdatingService, IItemRepository itemRepository, IUrlService urlService)
        {
            _itemObtainingService = itemObtainingObtainingService;
            _itemCreatingService = itemCreatingService;
            _itemUpdatingService = itemUpdatingService;
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
            if (!IsIdValid(id))
            {
                return BadRequest(ModelState);
            }

            if (!await _itemObtainingService.ExistsAsync(id))
            {
                return NotFound();
            }

            var item = _itemObtainingService.GetById(id);

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

            var createdItem = await _itemCreatingService.CreateAsync(item);
            var itemUrl = _urlService.GetItemUrl(createdItem.Id);

            return Created(itemUrl, createdItem);
        }

        // PUT: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> PutItemAsync(Guid id, [FromBody]Item item)
        {
            if (!IsIdValid(id) || !PutIsItemValid(id, item))
            {
                return BadRequest(ModelState);
            }

            if (!await _itemObtainingService.ExistsAsync(id))
            {
                return await PostItemAsync(new Item
                {
                    Text = item.Text
                });
            }

            await _itemUpdatingService.UpdateAsync(item);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteItemAsync(Guid id)
        {
            if (!IsIdValid(id))
            {
                return BadRequest(ModelState);
            }

            if (!await _itemObtainingService.ExistsAsync(id))
            {
                return NotFound();
            }

            var deletedItem = await _itemRepository.DeleteAsync(id);

            return Ok(deletedItem);
        }

        private bool PostIsItemValid(Item item)
        {
            IsItemValid(item);

            if (item != null && item.Id != Guid.Empty)
            {
                ModelState.AddModelError(nameof(item.Id), "Item id must not be set.");
            }

            return ModelState.IsValid;
        }

        private bool PutIsItemValid(Guid id, Item item)
        {
            IsItemValid(item);

            if (item != null && item.Id != id)
            {
                ModelState.AddModelError(nameof(item.Id), "Item id is not the same as provided id.");
            }

            return ModelState.IsValid;
        }

        private void IsItemValid(Item item)
        {
            if (item == null)
            {
                ModelState.AddModelError("", "Provided item is null.");
                return;
            }

            if (item.Text == null)
            {
                ModelState.AddModelError(nameof(item.Text), "Item text must not be null.");
            }
            else if (item.Text?.Trim() == string.Empty)
            {
                ModelState.AddModelError(nameof(item.Text), "Item text must be non-empty.");
            }

            if (item.CreationTime != DateTime.MinValue)
            {
                ModelState.AddModelError(nameof(item.CreationTime), "Item creation time must not be set.");
            }

            if (item.LastUpdateTime != DateTime.MinValue)
            {
                ModelState.AddModelError(nameof(item.LastUpdateTime), "Item last update time must not be set.");
            }
        }

        private bool IsIdValid(Guid id)
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError("", "Provided id is empty.");
            }

            return ModelState.IsValid;
        }
    }
}
