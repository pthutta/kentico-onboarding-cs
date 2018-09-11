using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Web.Http;
using TodoApp.Api.Models;

namespace TodoApp.Api.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/items")]
    public class ItemsController : ApiController
    {
        private static readonly Item[] Items = 
        {
            new Item
            {
                Id = "1",
                Text = "Learn react"
            },
            new Item
            {
                Id = "2",
                Text = "Learn redux"
            },
            new Item
            {
                Id = "3",
                Text = "Write Web API"
            },
            new Item
            {
                Id = "4",
                Text = "Write dummier controller"
            }
        };

        // GET: api/v1/items
        [Route]
        public async Task<IHttpActionResult> GetAllItemsAsync()
            => await Task.FromResult(Ok(Items));

        // GET: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> GetItemByIdAsync(string id)
            => await Task.FromResult(Ok(Items[0]));
        

        // POST: api/v1/items
        [Route("", Name = "PostNewItem")]
        public async Task<IHttpActionResult> PostItemAsync([FromBody]Item item)
            => await Task.FromResult(CreatedAtRoute("PostNewItem", new { id = Items[1].Id }, Items[1]));

        // PUT: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> PutItemAsync(string id, [FromBody]Item value)
            => await Task.FromResult(Ok(Items[2]));

        // DELETE: api/v1/items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteItemAsync(string id)
            => await Task.FromResult(Ok(Items[3]));
    }
}
