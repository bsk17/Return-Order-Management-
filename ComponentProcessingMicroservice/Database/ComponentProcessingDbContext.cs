﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComponentProcessingMicroservice.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComponentProcessingMicroservice.Database
{
    public class ComponentProcessingDbContext:DbContext
    {
        public ComponentProcessingDbContext(DbContextOptions<ComponentProcessingDbContext> options):base(options)
        {

        }

        public DbSet<CreditCard> CreditCards { get; set; } 
        public DbSet<ProcessRequest> ProcessRequests { get; set; }
        public DbSet<ProcessResponse> ProcessResponses { get; set; }
        public DbSet<DefectiveComponent> DefectiveComponents { get; set; }
    }
}
