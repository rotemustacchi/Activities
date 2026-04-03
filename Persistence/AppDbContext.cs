using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext(DbContextOptions options): IdentityDbContext<User>(options)
    {
        public required DbSet<Activity> Activities { get; set; }
    }
}
