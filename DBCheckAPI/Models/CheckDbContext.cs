using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentAPI.Database.Entities;

namespace DBCheckAPI.Models
{
    public class CheckDbContext:DbContext
    {
        public CheckDbContext(DbContextOptions<CheckDbContext> options):base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
    }
}
