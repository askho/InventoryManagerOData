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
    builder.EntitySet<Member>("Members");
    builder.EntitySet<Inventory>("Inventories"); 
    builder.EntitySet<Event>("Events"); 
    builder.EntitySet<Phone>("Phones"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [AuthAction]
    public class MembersController : ODataController
    {
        private InventoryDatabaseEntities db = new InventoryDatabaseEntities();

        // GET: odata/Members
        [EnableQuery]
        public IQueryable<Member> GetMembers()
        {
            return db.Members;
        }

        // GET: odata/Members(5)
        [EnableQuery]
        public SingleResult<Member> GetMember([FromODataUri] int key)
        {
            return SingleResult.Create(db.Members.Where(member => member.member_id == key));
        }

        // PUT: odata/Members(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Member> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Member member = await db.Members.FindAsync(key);
            if (member == null)
            {
                return NotFound();
            }

            patch.Put(member);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(member);
        }

        // POST: odata/Members
        public async Task<IHttpActionResult> Post(Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Members.Add(member);
            await db.SaveChangesAsync();

            return Created(member);
        }

        // PATCH: odata/Members(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Member> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Member member = await db.Members.FindAsync(key);
            if (member == null)
            {
                return NotFound();
            }

            patch.Patch(member);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(member);
        }

        // DELETE: odata/Members(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Member member = await db.Members.FindAsync(key);
            if (member == null)
            {
                return NotFound();
            }

            db.Members.Remove(member);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Members(5)/Inventories
        [EnableQuery]
        public IQueryable<Inventory> GetInventories([FromODataUri] int key)
        {
            return db.Members.Where(m => m.member_id == key).SelectMany(m => m.Inventories);
        }

        // GET: odata/Members(5)/Events
        [EnableQuery]
        public IQueryable<Event> GetEvents([FromODataUri] int key)
        {
            return db.Members.Where(m => m.member_id == key).SelectMany(m => m.Events);
        }

        // GET: odata/Members(5)/Phones
        [EnableQuery]
        public IQueryable<Phone> GetPhones([FromODataUri] int key)
        {
            return db.Members.Where(m => m.member_id == key).SelectMany(m => m.Phones);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberExists(int key)
        {
            return db.Members.Count(e => e.member_id == key) > 0;
        }
    }
}
