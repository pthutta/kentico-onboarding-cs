using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TodoApp.Api.Models;

namespace TodoApp.Api.Controllers
{
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

        // GET: api/Items
        public IEnumerable<Item> Get()
        {
            return Items;
        }

        // GET: api/Items/5
        [ResponseType(typeof(Item))]
        public IHttpActionResult Get(string id)
        {
            return Ok(Items.FirstOrDefault(i => i.Id == id));
        }

        // POST: api/Items
        [ResponseType(typeof(Item))]
        public IHttpActionResult Post([FromBody]Item item)
        {
            Items.Add(item);
            return CreatedAtRoute("DefaultApi", new { id = item.Id }, item);
        }

        // PUT: api/Items/5
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

        // DELETE: api/Items/5
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
