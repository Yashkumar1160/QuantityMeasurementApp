using Microsoft.EntityFrameworkCore;
using QuantityMeasurementAppModels.Entities;

namespace QuantityMeasurementAppRepositories.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ============ quantity_measurements table ============
            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                entity.ToTable("quantity_measurements");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
                entity.Property(e => e.Operation).HasColumnName("operation").IsRequired().HasMaxLength(50);
                entity.Property(e => e.FirstValue).HasColumnName("first_value");
                entity.Property(e => e.FirstUnit).HasColumnName("first_unit").HasMaxLength(50);
                entity.Property(e => e.SecondValue).HasColumnName("second_value");
                entity.Property(e => e.SecondUnit).HasColumnName("second_unit").HasMaxLength(50);
                entity.Property(e => e.ResultValue).HasColumnName("result_value");
                entity.Property(e => e.MeasurementType).HasColumnName("measurement_type").HasMaxLength(50);
                entity.Property(e => e.IsError).HasColumnName("is_error");
                entity.Property(e => e.ErrorMessage).HasColumnName("error_message").HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.HasIndex(e => e.UserId).HasDatabaseName("IX_quantity_measurements_user_id");
                entity.HasIndex(e => e.Operation).HasDatabaseName("IX_quantity_measurements_operation");
            });

            // ============ users table ============
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("IX_users_email");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired().HasMaxLength(100);

                // map the role column
                entity.Property(e => e.Role).HasColumnName("role").IsRequired().HasMaxLength(20).HasDefaultValue("User");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.LastLoginAt).HasColumnName("last_login_at");
            });
        }
    }
}