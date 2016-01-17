using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using OData.Models;
using OData.App_Start.Classes;

namespace OData.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using OData.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Item>("Items");
    builder.EntitySet<Category>("Categories"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [AuthAction]
    public class ItemsController : ODataController
    {
        private InventoryDatabaseEntities db = new InventoryDatabaseEntities();

        // GET: odata/Items
        [EnableQuery]
        public IQueryable<Item> GetItems()
        {
            return db.Items;
        }

        // GET: odata/Items(5)
        [EnableQuery]
        public SingleResult<Item> GetItem([FromODataUri] int key)
        {
            return SingleResult.Create(db.Items.Where(item => item.item_id == key));
        }

        // PUT: odata/Items(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Item> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Item item = await db.Items.FindAsync(key);
            if (item == null)
            {
                return NotFound();
            }

            patch.Put(item);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(item);
        }

        // POST: odata/Items
        public async Task<IHttpActionResult> Post(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Items.Add(item);
            await db.SaveChangesAsync();

            return Created(item);
        }

        // PATCH: odata/Items(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Item> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Item item = await db.Items.FindAsync(key);
            if (item == null)
            {
                return NotFound();
            }

            patch.Patch(item);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(item);
        }

        // DELETE: odata/Items(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Item item = await db.Items.FindAsync(key);
            if (item == null)
            {
                return NotFound();
            }

            db.Items.Remove(item);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Items(5)/Categories
        [EnableQuery]
        public IQueryable<Category> GetCategories([FromODataUri] int key)
        {
            return db.Items.Where(m => m.item_id == key).SelectMany(m => m.Categories);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(int key)
        {
            return db.Items.Count(e => e.item_id == key) > 0;
        }
    }
}
