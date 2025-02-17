using Gym_DataBase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_DataBase.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<Users> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subscriptions> Subscriptions { get; set; }
        public DbSet<WorkoutPlans> WorkoutPlans { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<Testimonials> Testimonials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relation in User && WorkoutPlans => (Member)
            modelBuilder.Entity<Users>()
                .HasMany(u => u.MemberWorkoutPlans)
                .WithOne(w => w.Member)
                .HasForeignKey(w => w.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            //  Relation in User و WorkoutPlans >= (Trainer)
            modelBuilder.Entity<Users>()
                .HasMany(u => u.TrainerWorkoutPlans)
                .WithOne(w => w.Trainer)
                .HasForeignKey(w => w.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }


    }
}
