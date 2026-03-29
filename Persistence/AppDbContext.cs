using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Persistence
{
    public class AppDbContext(DbContextOptions options): DbContext(options)
    {
        public required DbSet<Activity> Activities { get; set; }
    }
}
