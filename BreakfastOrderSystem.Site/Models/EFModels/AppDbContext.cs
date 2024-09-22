using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BreakfastOrderSystem.Site.Models.EFModels
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=AppDbContext")
        {
        }

        public virtual DbSet<AddOnCategory> AddOnCategories { get; set; }
        public virtual DbSet<AddOnOption> AddOnOptions { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductAddOnDetail> ProductAddOnDetails { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddOnCategory>()
                .Property(e => e.AddOnCategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<AddOnOption>()
                .Property(e => e.AddOnOptionName)
                .IsUnicode(false);

            modelBuilder.Entity<AddOnOption>()
                .Property(e => e.AddOnOptionPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Member>()
                .Property(e => e.MemberName)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.ConfirmCode)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.EncryptedPassword)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.UnitPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .Property(e => e.TotalAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ProductCategory>()
                .Property(e => e.ProductCategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductImage)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Description)
                .IsUnicode(false);
        }
    }
}
