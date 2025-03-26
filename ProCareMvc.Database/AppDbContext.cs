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
        //public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderItem>()
             .HasDiscriminator<OrderItemType>("ItemType")
             .HasValue<OrderItem>(OrderItemType.General)
             .HasValue<AppointmentOrderItem>(OrderItemType.Appointments)
             .HasValue<DrugOrderItem>(OrderItemType.Drugs)
             .HasValue<LabOrderItem>(OrderItemType.Labs);

            //User user = new User()
            //{
            //    UserName = "admin",
            //    BirthDate = DateOnly.FromDateTime(DateTime.Now),
            //    Email = "islam@swds.com",
            //    FirstName = "Islam",
            //    Gender = Gender.Male,
            //    PhoneNumber = "ssss",
            //};
            //builder.Entity<OrderItem>().HasData(user);
            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=SABREEN\\SQLEXPRESS;Database=ProCareMvc;Trusted_Connection=True;MultipleActiveResultSets=true");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
