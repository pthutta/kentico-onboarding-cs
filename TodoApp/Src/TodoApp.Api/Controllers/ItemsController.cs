﻿using System;
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
            if (!IsIdValid(id))
            {
                return BadRequest(ModelState);
            }

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
            if (!IsIdValid(id) || !PutIsItemValid(id, item))
            {
                return BadRequest(ModelState);
            }

            if (!await _itemService.UpdateItemAsync(item))
            {
                return await PostItemAsync(item);
            }

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

            var deletedItem = await _itemRepository.DeleteAsync(id);

            if (deletedItem == null)
            {
                return NotFound();
            }
            return Ok(deletedItem);
        }

        private bool PostIsItemValid(Item item)
        {
            if (!IsItemValid(item))
            {
                return false;
            }

            if (item.Id != Guid.Empty)
            {
                ModelState.AddModelError(nameof(item.Id), "Resource id must not be set.");
            }

            return ModelState.IsValid;
        }

        private bool PutIsItemValid(Guid id, Item item)
        {
            if (!IsItemValid(item))
            {
                return false;
            }

            if (item.Id != id)
            {
                ModelState.AddModelError(nameof(item.Id), "Resource id is not the same as provided id.");
            }

            return ModelState.IsValid;
        }

        private bool IsItemValid(Item item)
        {
            if (item == null)
            {
                ModelState.AddModelError("", "Provided Resource is null.");
                return false;
            }

            if (item.Text.Trim() == string.Empty)
            {
                ModelState.AddModelError(nameof(item.Text), "Resource text must be non-empty.");
            }

            if (item.CreationTime != default(DateTime))
            {
                ModelState.AddModelError(nameof(item.CreationTime), "Resource creation time must not be set.");
            }

            if (item.LastUpdateTime != default(DateTime))
            {
                ModelState.AddModelError(nameof(item.LastUpdateTime), "Resource last update time must not be set.");
            }

            return ModelState.IsValid;
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
