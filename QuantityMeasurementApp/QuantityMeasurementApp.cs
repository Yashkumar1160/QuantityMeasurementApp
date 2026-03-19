using System;
using System.Collections.Generic;
using QuantityMeasurementApp.Controllers;
using QuantityMeasurementApp.Interfaces;
using QuantityMeasurementApp.Menu;
using QuantityMeasurementApp.Services;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppRepositories.Repositories;
using QuantityMeasurementAppRepositories.Utilities;
using QuantityMeasurementAppServices.Interfaces;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementApp
    {
        private static QuantityMeasurementApp instance;
        private static readonly object lockObject = new object();

        private QuantityMeasurementController controller;
        private IMenu menu;
        private IQuantityMeasurementRepository repository;
        private string activeRepositoryType;

        // Constructor
        private QuantityMeasurementApp()
        {
            Console.WriteLine("[App] Starting Quantity Measurement Application...");

            ApplicationConfig config = new ApplicationConfig();

            // Read repository type from appsettings.json
            string repoType = config.GetRepositoryType();
            Console.WriteLine("[App] Configured repository type: " + repoType);

            if (repoType.Equals("database", StringComparison.OrdinalIgnoreCase))
            {
                // Try database first, fall back to cache if DB is offline
                TryInitializeDatabaseRepository(config);
            }
            else
            {
                Console.WriteLine("[App] Repository type is cache. Skipping database.");
                InitializeCacheRepository();
            }

            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repository);

            controller = new QuantityMeasurementController(service);
            menu = new QuantityMenu(controller);

            Console.WriteLine("[App] Initialization complete.");
            Console.WriteLine("[App] Active repository: " + activeRepositoryType);
            Console.WriteLine("");
        }

        // Tries SQL Server based on config setting
        // Falls back to cache automatically if SQL Server is unreachable
        private void TryInitializeDatabaseRepository(ApplicationConfig config)
        {
            Console.WriteLine("[App] Attempting to connect to SQL Server...");

            try
            {
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                repository = new QuantityMeasurementDatabaseRepository(pool);
                activeRepositoryType = "Database (SQL Server)";

                Console.WriteLine("[App] SQL Server connected. Using Database Repository.");
            }
            catch (Exception ex)
            {
                // SQL Server is offline or unreachable
                // Automatically switch to cache 
                Console.WriteLine("");
                Console.WriteLine("[App] WARNING: SQL Server is not available.");
                Console.WriteLine("[App] Reason : " + ex.Message);
                Console.WriteLine("[App] Automatically switching to Cache Repository...");
                Console.WriteLine("");

                InitializeCacheRepository();
            }
        }

        // Initializes the in-memory cache repository with JSON file backup
        private void InitializeCacheRepository()
        {
            repository = QuantityMeasurementCacheRepository.GetInstance();
            activeRepositoryType = "Cache (SQL Server offline - data saved to JSON file)";
            Console.WriteLine("[App] Cache Repository ready.");
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

        // Prints all stored measurements after the menu exits
        public void ReportAllMeasurements()
        {
            Console.WriteLine("\n========== Measurement History ==========");
            Console.WriteLine("Repository : " + activeRepositoryType);

            List<QuantityMeasurementEntity> all = repository.GetAll();

            Console.WriteLine("Total records: " + all.Count);

            for (int i = 0; i < all.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + all[i].ToString());
            }

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Pool stats : " + repository.GetPoolStatistics());
            Console.WriteLine("=========================================\n");
        }

        // Deletes all records
        public void DeleteAllMeasurements()
        {
            Console.WriteLine("[App] Deleting all measurements...");
            repository.DeleteAll();
            Console.WriteLine("[App] Done. Count now: " + repository.GetTotalCount());
        }

        // Releases DB connections on shutdown
        public void CloseResources()
        {
            Console.WriteLine("[App] Closing resources...");
            repository.ReleaseResources();
            Console.WriteLine("[App] Resources closed.");
        }
    }
}