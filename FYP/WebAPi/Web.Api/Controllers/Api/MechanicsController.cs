using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Routing;
using MechModels;
using Web.Api.Models;
using Web.Api.Utils;

namespace Web.Api.Controllers.Api
{
    public class MechanicsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        public IQueryable<Mechanic> GetMechanic(double lati, double longi)
        {
            var GlobalConstant1 = 57.2957795130823D;

            var GlobalConstant2 = 3958.75586574D; 

            double latitude = lati;
            double longitude = longi;
            var loc = (from mechanic in db.Mechanics
                       let temp = SqlFunctions.Sin(mechanic.Latitude / GlobalConstant1)
                       * SqlFunctions.Sin(latitude / GlobalConstant1)
                       + SqlFunctions.Cos(mechanic.Latitude / GlobalConstant1)
                       * SqlFunctions.Cos(latitude / GlobalConstant1)
                       * SqlFunctions.Cos(longitude / GlobalConstant1)
                       - (mechanic.Longitude / GlobalConstant1)
                       let calMiles = (GlobalConstant2 * SqlFunctions.Acos(temp > 1 ? 1 : (temp < -1 ? -1 : temp)))
                       where (mechanic.Latitude > 0 && mechanic.Longitude > 0)
                       orderby calMiles
                       select mechanic).Take(5);
            return loc;

        }

         
        [ResponseType(typeof(Mechanic))]
        public IHttpActionResult GetMechanic(int id)
        {
            Mechanic mechanic = db.Mechanics.Find(id);
            if (mechanic == null)
            {
                return NotFound();
            }

            return Ok(mechanic);
        }

         
        [ResponseType(typeof(Mechanic))]
        public IHttpActionResult PutMechanic(int id, Mechanic mechanic)
        {
            var mechanicInDb = db.Mechanics.Find(id);

            if(mechanicInDb == null)
            {
                return NotFound();
            }

            mechanicInDb.Name = mechanic.Name;
            mechanicInDb.Title = mechanic.Title;
            mechanicInDb.ContactNo = mechanic.ContactNo;
            mechanicInDb.Latitude = mechanic.Latitude;
            mechanicInDb.Longitude = mechanic.Longitude;
            mechanicInDb.Address = mechanic.Address;
            db.SaveChanges();

            return Ok(mechanicInDb);
        }

        // POST: api/Mechanics
        [ResponseType(typeof(Mechanic))]
        public IHttpActionResult PostMechanic(Mechanic mechanic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var mechanicInDb = db.Mechanics.Where(m => m.ContactNo == mechanic.ContactNo).FirstOrDefault();

            if (mechanicInDb!=null)
            {
                return CreatedAtRoute("DefaultApi", new { id = mechanicInDb.Id }, mechanicInDb);
            }

            var initial = mechanic.Name[0].ToString().ToUpper();
            mechanic.ImageUrl = $"Images\\{initial}.jpg";

            db.Mechanics.Add(mechanic);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = mechanic.Id }, mechanic);
        }


        [ResponseType(typeof(Mechanic))]
        public IHttpActionResult DeleteMechanic(int id)
        {
            Mechanic mechanic = db.Mechanics.Find(id);
            if (mechanic == null)
            {
                return NotFound();
            }

            db.Mechanics.Remove(mechanic);
            db.SaveChanges();

            return Ok(mechanic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MechanicExists(int id)
        {
            return db.Mechanics.Count(e => e.Id == id) > 0;
        }
    }
}