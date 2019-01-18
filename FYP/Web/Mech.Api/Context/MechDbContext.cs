using Mech.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mech.Api.Context
{
    public class MechDbContext : DbContext
    {

        public MechDbContext(DbContextOptions<MechDbContext> options) :base(options)
        {
            

        }


        public DbSet<Mechanic> Mechanics { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Contract> Contracts { get; set; }

    }
}
