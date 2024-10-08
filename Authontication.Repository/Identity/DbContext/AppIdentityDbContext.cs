﻿using Authontication.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Authontication.Repository.Identity.DbContext
{
    public class AppIdentityDbContext: IdentityDbContext<AppUser>

    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)

        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Address>().ToTable("Addresses");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
