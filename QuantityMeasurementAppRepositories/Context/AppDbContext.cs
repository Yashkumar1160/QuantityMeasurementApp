using Microsoft.EntityFrameworkCore;
using QuantityMeasurementAppModels.Entities;

namespace QuantityMeasurementAppRepositories.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }


        // To configure how entity maps to database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                // Table Name
                entity.ToTable("quantity_measurements");

                // Primary key
                entity.HasKey(e => e.Id);

                // Column name (id) auto increment value
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                // Column name (operation), required, max character 50 
                entity.Property(e => e.Operation)
                    .HasColumnName("operation")
                    .IsRequired()
                    .HasMaxLength(50);

                // Column name (first_value) 
                entity.Property(e => e.FirstValue)
                    .HasColumnName("first_value");

                // Column name (first_unit) 
                entity.Property(e => e.FirstUnit)
                    .HasColumnName("first_unit")
                    .HasMaxLength(50);

                // Column name (second_value)
                entity.Property(e => e.SecondValue)
                    .HasColumnName("second_value");

                // Column name (second_unit)
                entity.Property(e => e.SecondUnit)
                    .HasColumnName("second_unit")
                    .HasMaxLength(50);

                // Column name (result_value)
                entity.Property(e => e.ResultValue)
                    .HasColumnName("result_value");

                // Column name (measurement_type)
                entity.Property(e => e.MeasurementType)
                    .HasColumnName("measurement_type")
                    .HasMaxLength(50);

                // Column name (is_error)
                entity.Property(e => e.IsError)
                    .HasColumnName("is_error");

                // Column name (error_message)
                entity.Property(e => e.ErrorMessage)
                    .HasColumnName("error_message")
                    .HasMaxLength(500);

                // Column name (created_at)
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at");

                // Column name (updated_at)
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at");

                // Indexes for commonly queried fields
                entity.HasIndex(e => e.Operation)
                    .HasDatabaseName("IX_quantity_measurements_operation");

                entity.HasIndex(e => e.MeasurementType)
                    .HasDatabaseName("IX_quantity_measurements_measurement_type");

                entity.HasIndex(e => e.IsError)
                    .HasDatabaseName("IX_quantity_measurements_is_error");
            });
        }
    }
}
