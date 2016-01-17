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
    builder.EntitySet<Phone>("Phones");
    builder.EntitySet<Member>("Members"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [AuthAction]
    public class PhonesController : ODataController
    {
        private InventoryDatabaseEntities db = new InventoryDatabaseEntities();

        // GET: odata/Phones
        [EnableQuery]
        public IQueryable<Phone> GetPhones()
        {
            return db.Phones;
        }

        // GET: odata/Phones(5)
        [EnableQuery]
        public SingleResult<Phone> GetPhone([FromODataUri] int key)
        {
            return SingleResult.Create(db.Phones.Where(phone => phone.phone_id == key));
        }

        // PUT: odata/Phones(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Phone> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Phone phone = await db.Phones.FindAsync(key);
            if (phone == null)
            {
                return NotFound();
            }

            patch.Put(phone);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(phone);
        }

        // POST: odata/Phones
        public async Task<IHttpActionResult> Post(Phone phone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Phones.Add(phone);
            await db.SaveChangesAsync();

            return Created(phone);
        }

        // PATCH: odata/Phones(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Phone> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Phone phone = await db.Phones.FindAsync(key);
            if (phone == null)
            {
                return NotFound();
            }

            patch.Patch(phone);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(phone);
        }

        // DELETE: odata/Phones(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Phone phone = await db.Phones.FindAsync(key);
            if (phone == null)
            {
                return NotFound();
            }

            db.Phones.Remove(phone);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Phones(5)/Members
        [EnableQuery]
        public IQueryable<Member> GetMembers([FromODataUri] int key)
        {
            return db.Phones.Where(m => m.phone_id == key).SelectMany(m => m.Members);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhoneExists(int key)
        {
            return db.Phones.Count(e => e.phone_id == key) > 0;
        }
    }
}
