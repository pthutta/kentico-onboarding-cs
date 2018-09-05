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
        internal static readonly IList<Item> Items = new List<Item>
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
            }
        };

        // test for content/value, status code, 

        // GET: api/v1/Items
        [Route]
        public async Task<IEnumerable<Item>> GetAllItemsAsync()
            => await Task.FromResult(Items);

        // GET: api/v1/Items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> GetItemByIdAsync(string id)
            => await Task.FromResult(Ok(Items.FirstOrDefault(i => i.Id == id)));
        

        // POST: api/v1/Items
        [Route("", Name = "PostNewItem")]
        public async Task<IHttpActionResult> PostItemAsync([FromBody]Item item)
        {
            Items.Add(item);
            return await Task.FromResult(CreatedAtRoute("PostNewItem", new { id = item.Id }, item));
        }

        // PUT: api/v1/Items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> PutItemAsync(string id, [FromBody]Item value)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.Text = value.Text;
            }
            return await Task.FromResult(StatusCode(HttpStatusCode.NoContent));
        }

        // DELETE: api/v1/Items/5
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteItemAsync(string id)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                Items.Remove(item);
            }
            return await Task.FromResult(Ok(item));
        }
    }
}
