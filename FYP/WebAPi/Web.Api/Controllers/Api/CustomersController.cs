using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MechModels;
using Web.Api.Models;

namespace Web.Api.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext Db; 

        public CustomersController()
        {
            Db = new ApplicationDbContext();
        }
        
        public IQueryable<Customer> GetCustomers()
        {
            return Db.Customers;
        }

        
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = Db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

         
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            Db.Entry(customer).State = EntityState.Modified;

            try
            {
                Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Customers
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Customer customer)
        {

            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerInDb = Db.Customers.Where(c => c.ContactNo == customer.ContactNo).FirstOrDefault();

            if (customerInDb != null)
            {
                return CreatedAtRoute("DefaultApi", new { id = customerInDb.Id }, customerInDb);
            }


            Db.Customers.Add(customer);
            Db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customer.Id }, customer);
        }
         
        
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = Db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            Db.Customers.Remove(customer);
            Db.SaveChanges();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return Db.Customers.Count(e => e.Id == id) > 0;
        }
    }
}