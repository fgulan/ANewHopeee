using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using EntityFramework.Triggers;
using Wild8.Models;

namespace Wild8.DAL
{
    public class RestaurauntContext : DbContext
    {
        public RestaurauntContext() : base("RestaurauntContext")
        {
        }

        public DbSet<Meal> Meals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<AddOn> AddOns { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderMealAddOn> OrderMealAddOns { get; set; }
        public DbSet<MealType> MealTypes { get; set; }
        public DbSet<MealAddOn> MealAddOns { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}