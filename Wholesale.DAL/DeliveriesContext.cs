using Microsoft.EntityFrameworkCore;
using Wholesale.BL.Enums;
using Wholesale.BL.Models;
using Wholesale.BL.Models.Views;

namespace Wholesale.DAL
{
    public class DeliveriesContext : DbContext
    {
        public DeliveriesContext()
        {
        }

        public DeliveriesContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<CourierStatsV> CourierStats { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductStatsV> ProductStats { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<OrderWorth> OrderWorth { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<OrderStatus>(name: "order_status");
            modelBuilder.HasPostgresEnum<UserRole>(name: "user_role");

            modelBuilder.Entity<OrderWorth>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.TotalWorth)
                    .HasColumnName("total_worth")
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("adresses_pkey");

                entity.ToTable("addresses");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddressDetails)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasColumnType("character varying");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasColumnType("character varying");

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasColumnName("postal_code")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Address)
                    .HasForeignKey<Address>(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_user");
            });

            modelBuilder.Entity<CourierStatsV>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("courier_stats");

                entity.Property(e => e.CourierId)
                    .HasColumnName("courier_id");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.NumberOfOrders)
                    .HasColumnName("number_of_orders");

                entity.Property(e => e.TotalWorth)
                    .HasColumnName("total_worth")
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("order_details_pkey");

                entity.ToTable("order_details");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_product");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("orders_pkey");

                entity.ToTable("orders");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasIdentityOptions(null, null, null, null, true, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ClientId)
                    .HasColumnName("client_id");

                entity.Property(e => e.CourierId)
                    .HasColumnName("courier_id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("order_status"); ;

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.OrdersClient)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_client");

                entity.HasOne(d => d.Courier)
                    .WithMany(p => p.OrdersCourier)
                    .HasForeignKey(d => d.CourierId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_courier");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("product_categories_pkey");

                entity.ToTable("product_categories");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .HasIdentityOptions(null, null, null, null, true, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<ProductStatsV>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("product_stats");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasColumnType("character varying");

                entity.Property(e => e.CurrentPrice)
                    .HasColumnName("current_price")
                    .HasColumnType("numeric(12,2)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.NumberSold)
                    .HasColumnName("number_sold");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("products_pkey");

                entity.ToTable("products");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasIdentityOptions(null, null, null, null, true, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("numeric(12,2)");

                entity.Property(e => e.Stock)
                    .HasColumnName("stock");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_category");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("users_pkey");

                entity.ToTable("users");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasIdentityOptions(null, null, null, null, true, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CompanyName)
                    .HasColumnName("company_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(32);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("password_hash");

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasColumnName("password_salt");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(20);

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .HasColumnType("user_role");
            });
        }
    }
}
