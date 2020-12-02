using System;
using Microsoft.EntityFrameworkCore;

namespace deloveh.david.EfSqlDurableFa
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext() { }
        
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options) { }

        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Employee>(entity =>{
                entity.ToTable("Employees");
                entity.HasKey(n => n.EmployeeID);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = Environment.GetEnvironmentVariable("EmployeesDbConnectionString");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}