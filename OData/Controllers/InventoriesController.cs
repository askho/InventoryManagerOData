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
    builder.EntitySet<Inventory>("Inventories");
    builder.EntitySet<Company>("Companies"); 
    builder.EntitySet<Event>("Events"); 
    builder.EntitySet<Member>("Members"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [AuthAction]
    public class InventoriesController : ODataController
    {
        private InventoryDatabaseEntities db = new InventoryDatabaseEntities();

        // GET: odata/Inventories
        [EnableQuery]
        public IQueryable<Inventory> GetInventories()
        {
            return db.Inventories;
        }

        // GET: odata/Inventories(5)
        [EnableQuery]
        public SingleResult<Inventory> GetInventory([FromODataUri] int key)
        {
            return SingleResult.Create(db.Inventories.Where(inventory => inventory.inventory_id == key));
        }

        // PUT: odata/Inventories(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Inventory> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Inventory inventory = await db.Inventories.FindAsync(key);
            if (inventory == null)
            {
                return NotFound();
            }

            patch.Put(inventory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(inventory);
        }

        // POST: odata/Inventories
        public async Task<IHttpActionResult> Post(Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Inventories.Add(inventory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InventoryExists(inventory.inventory_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(inventory);
        }

        // PATCH: odata/Inventories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Inventory> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Inventory inventory = await db.Inventories.FindAsync(key);
            if (inventory == null)
            {
                return NotFound();
            }

            patch.Patch(inventory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(inventory);
        }

        // DELETE: odata/Inventories(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Inventory inventory = await db.Inventories.FindAsync(key);
            if (inventory == null)
            {
                return NotFound();
            }

            db.Inventories.Remove(inventory);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Inventories(5)/Company
        [EnableQuery]
        public SingleResult<Company> GetCompany([FromODataUri] int key)
        {
            return SingleResult.Create(db.Inventories.Where(m => m.inventory_id == key).Select(m => m.Company));
        }

        // GET: odata/Inventories(5)/Event
        [EnableQuery]
        public SingleResult<Event> GetEvent([FromODataUri] int key)
        {
            return SingleResult.Create(db.Inventories.Where(m => m.inventory_id == key).Select(m => m.Event));
        }

        // GET: odata/Inventories(5)/Member
        [EnableQuery]
        public SingleResult<Member> GetMember([FromODataUri] int key)
        {
            return SingleResult.Create(db.Inventories.Where(m => m.inventory_id == key).Select(m => m.Member));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InventoryExists(int key)
        {
            return db.Inventories.Count(e => e.inventory_id == key) > 0;
        }
    }
}
