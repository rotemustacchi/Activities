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
        public required DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public required DbSet<Photo> Photos { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityAttendee>(x=> x.HasKey(a=> new {a.UserId,a.ActivityId}));

            builder.Entity<ActivityAttendee>()
                .HasOne(x => x.User)
                .WithMany(x => x.Activities)
                .HasForeignKey(x => x.UserId);

            builder.Entity<ActivityAttendee>()
                .HasOne(x => x.Activity)
                .WithMany(x => x.Attendees)
                .HasForeignKey(x => x.ActivityId);
        }
    }
}
