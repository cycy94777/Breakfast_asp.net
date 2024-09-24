using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace User.Site.Models.EFModels
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
        public virtual DbSet<OrderAddOnDetail> OrderAddOnDetails { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<PointDetail> PointDetails { get; set; }
        public virtual DbSet<ProductAddOnDetail> ProductAddOnDetails { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<TakeOrderNumber> TakeOrderNumbers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddOnCategory>()
                .HasMany(e => e.AddOnOptions)
                .WithRequired(e => e.AddOnCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AddOnCategory>()
                .HasMany(e => e.ProductAddOnDetails)
                .WithRequired(e => e.AddOnCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AddOnOption>()
                .HasMany(e => e.ProductAddOnDetails)
                .WithRequired(e => e.AddOnOption)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.EncryptedPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.ConfirmCode)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.PointDetails)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.PointDetails)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductAddOnDetail>()
                .HasMany(e => e.OrderAddOnDetails)
                .WithRequired(e => e.ProductAddOnDetail)
                .HasForeignKey(e => e.ProductAddOnDetailsID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.ProductCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductAddOnDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Store>()
                .Property(e => e.EncryptedPassword)
                .IsUnicode(false);
        }
    }
}
