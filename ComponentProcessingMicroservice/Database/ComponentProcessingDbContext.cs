using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Database.Entities;

namespace ComponentProcessingMicroservice.Database
{
    public class ComponentProcessingDbContext:DbContext
    {
        public ComponentProcessingDbContext(DbContextOptions<ComponentProcessingDbContext> options):base(options)
        {

        }

        public DbSet<CreditCard> CreditCards { get; set; } 
    }
}
