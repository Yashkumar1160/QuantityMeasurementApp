using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.Controllers;
using QuantityMeasurementApp.Interfaces;
using QuantityMeasurementApp.Menu;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppRepositories.Repositories;
using QuantityMeasurementAppServices.Interfaces;
using QuantityMeasurementAppRepositories.Context;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementApp
    {
        private static QuantityMeasurementApp? instance;
        private static readonly object lockObject = new object();

        private QuantityMeasurementController controller;
        private IMenu menu;
        private IQuantityRecordRepository repository;

        // Constructor
        private QuantityMeasurementApp()
        {
            Console.WriteLine("[App] Starting Quantity Measurement Application...");

            // Directly use EF Core repository
            repository = new QuantityRecordRepository(CreateDbContext());

            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repository);

            controller = new QuantityMeasurementController(service);
            menu = new QuantityMenu(controller);

            Console.WriteLine("[App] Initialized using EF Core Repository");
            Console.WriteLine("");
        }

        // Create DbContext manually (since no DI in console app)
        private static AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=QuantityMeasurementAppDB;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            return new AppDbContext(options);
        }

        // Singleton
        public static QuantityMeasurementApp GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new QuantityMeasurementApp();
                    }
                }
            }
            return instance;
        }

        // Start the application
        public void Start()
        {
            menu.Run();
        }

        // Prints all stored measurements
        // Console app has no logged-in user, so userId = 0 fetches all records
        public void ReportAllMeasurements()
        {
            Console.WriteLine("\n========== Measurement History ==========");

            List<QuantityMeasurementEntity> all = repository.GetAll(0);

            Console.WriteLine("Total records: " + all.Count);

            for (int i = 0; i < all.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + all[i]);
            }

            Console.WriteLine("=========================================\n");
        }

        // Deletes all records 
        public void DeleteAllMeasurements()
        {
            Console.WriteLine("[App] Deleting all measurements...");
            Console.WriteLine("[App] Done.");
        }

        // No need to manually release resources in EF Core
        public void CloseResources()
        {
            Console.WriteLine("[App] Closing resources...");

            // EF Core handles connections automatically

            Console.WriteLine("[App] Resources closed.");
        }
    }
}