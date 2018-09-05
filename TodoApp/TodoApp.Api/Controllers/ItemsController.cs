using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Web.Http;
using TodoApp.Api.Models;

namespace TodoApp.Api.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/Items")]
    public class ItemsController : ApiController
    {
        private static readonly IList<Item> Items = new List<Item>();

        public ItemsController()
        {
            if (Items.Count != 0) return;

            Items.Add(new Item
            {
                Id = "1",
                Text = "Learn react"
            });
            Items.Add(new Item
            {
                Id = "2",
                Text = "Learn redux"
            });
            Items.Add(new Item
            {
                Id = "3",
                Text = "Write Web API"
            });
        }

        // GET: api/v1/Items
        [Route]
        public IEnumerable<Item> Get()
        {
            return Items;
        }

        // GET: api/v1/Items/5
        [Route("{id}")]
        [ResponseType(typeof(Item))]
        public IHttpActionResult Get(string id)
        {
            return Ok(Items.FirstOrDefault(i => i.Id == id));
        }

        // POST: api/v1/Items
        [Route("", Name = "PostNewItem")]
        [ResponseType(typeof(Item))]
        public IHttpActionResult Post([FromBody]Item item)
        {
            Items.Add(item);
            return CreatedAtRoute("PostNewItem", new { id = item.Id }, item);
        }

        // PUT: api/v1/Items/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(string id, [FromBody]Item value)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.Text = value.Text;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/v1/Items/5
        [Route("{id}")]
        [ResponseType(typeof(Item))]
        public IHttpActionResult Delete(string id)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                Items.Remove(item);
            }
            return Ok(item);
        }
    }
}
