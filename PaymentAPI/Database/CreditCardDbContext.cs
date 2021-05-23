using Microsoft.EntityFrameworkCore;
using PaymentAPI.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Database
{
    public class CreditCardDbContext:DbContext
    {
        public CreditCardDbContext(DbContextOptions<CreditCardDbContext> options):base(options)
        {

        }

        public DbSet<CreditCard> CreditCards { get; set; }
    }
}
