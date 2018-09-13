using System;
using System.Threading.Tasks;
using System.Web.Http;
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
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Text = "Learn react"
            },
            new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Text = "Learn redux"
            },
            new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Text = "Write Web API"
            },
            new Item
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
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
