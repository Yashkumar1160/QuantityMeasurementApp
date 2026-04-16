using Microsoft.EntityFrameworkCore;
using QuantityMeasurementAppModels.Entities;

namespace HistoryService.Context
{
    public class HistoryDbContext : DbContext
    {
        public HistoryDbContext(DbContextOptions<HistoryDbContext> options) : base(options) { }

        // Only quantity_measurements table — no Users
        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=HistoryServiceDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}