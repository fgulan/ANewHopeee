﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wild8.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Wild8DBEntities : DbContext
    {
        public Wild8DBEntities()
            : base("name=Wild8DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AddOn> AddOns { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<MealAddOn> MealAddOns { get; set; }
        public virtual DbSet<MealCategory> MealCategories { get; set; }
        public virtual DbSet<Meal> Meals { get; set; }
        public virtual DbSet<MealType> MealTypes { get; set; }
        public virtual DbSet<OrderMealAddOn> OrderMealAddOns { get; set; }
        public virtual DbSet<OrderMeal> OrderMeals { get; set; }
        public virtual DbSet<OrderMealsAddOn> OrderMealsAddOns { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}