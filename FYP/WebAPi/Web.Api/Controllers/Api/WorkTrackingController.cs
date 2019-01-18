using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Api.Models; 

namespace Web.Api.Controllers.Api
{
    public class WorkTrackingController : ApiController
    {
        ApplicationDbContext Db = new ApplicationDbContext();

        [HttpPut]
        [Route("api/worktracking/WorkStarted/{id}")]
        public IHttpActionResult WorkStarted(int id)
        {
            var contract = Db.Contracts.Find(id);

            contract.IsWorkStarted = true;
            contract.WorkStarted = DateTime.UtcNow;
            
            Db.SaveChanges();


            return Ok(contract);
        }

        [HttpPut]
        [Route("api/worktracking/WorkFinished/{id}")]
        public  IHttpActionResult WorkFinished(int id)
        {
            var contract = Db.Contracts.Find(id);

            contract.IsWorkFinished = true;
            contract.WorkFinished = DateTime.UtcNow;
            Db.SaveChanges();

            
            return Ok(contract);
        }

        public string Get()
        {
            return "Tracking";
        }

        
        public string Get(int id)
        {
            return "Tracking";
        }

        
        public void Post([FromBody]string value)
        {
        }

      
        public void Put(int id, [FromBody]string value)
        {
        }

       
        public void Delete(int id)
        {
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
        }
    }
}
