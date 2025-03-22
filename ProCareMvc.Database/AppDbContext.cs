using System.Reflection.Emit;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProCareMvc.Database.Entity;
using ProCareMvc.Database.Utility;

namespace ProCareMvc.Database
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<TestLab> TestLabs { get; set; }
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderItem>()
             .HasDiscriminator<OrderItemType>("ItemType")
             .HasValue<OrderItem>(OrderItemType.General)
             .HasValue<AppointmentOrderItem>(OrderItemType.Appointments)
             .HasValue<DrugOrderItem>(OrderItemType.Drugs)
             .HasValue<LabOrderItem>(OrderItemType.Labs);
            base.OnModelCreating(builder);
        }
    }
}
