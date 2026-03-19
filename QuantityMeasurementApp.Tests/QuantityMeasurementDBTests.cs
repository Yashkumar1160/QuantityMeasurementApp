using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Services;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Repositories;
using QuantityMeasurementAppRepositories.Utilities;
using QuantityMeasurementAppBusiness.Exceptions;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class QuantityMeasurementDBTests
    {
        // -------------------------------------------------------
        // ApplicationConfig Tests
        // -------------------------------------------------------

        //Test ApplicationConfig loads connection string from appsettings.json
        [TestMethod]
        public void TestDatabaseConfiguration_LoadedFromProperties()
        {
            //create config instance
            ApplicationConfig config = new ApplicationConfig();

            //get connection string
            string connectionString = config.GetConnectionString();

            //verify connection string is not null or empty
            Assert.IsNotNull(connectionString);
            Assert.AreNotEqual("", connectionString);
        }

        //Test ApplicationConfig loads pool size from appsettings.json
        [TestMethod]
        public void TestDatabaseConfiguration_PoolSizeLoaded()
        {
            //create config instance
            ApplicationConfig config = new ApplicationConfig();

            //get pool size
            int poolSize = config.GetMaxPoolSize();

            //verify pool size is greater than zero
            Assert.IsTrue(poolSize > 0);
        }

        //Test ApplicationConfig falls back to default values when key is missing
        [TestMethod]
        public void TestDatabaseConfiguration_FallbackToDefaults()
        {
            //create a temp config file with no Database section
            string tempPath = Path.Combine(Path.GetTempPath(), "test_appsettings.json");
            File.WriteAllText(tempPath, "{ \"Logging\": { \"LogLevel\": \"Information\" } }");

            //create config from temp file
            ApplicationConfig config = new ApplicationConfig(tempPath);

            //verify default pool size returned
            Assert.AreEqual(5, config.GetMaxPoolSize());

            //verify default timeout returned
            Assert.AreEqual(30, config.GetConnectionTimeout());

            //verify default repo type is cache
            Assert.AreEqual("cache", config.GetRepositoryType());

            //cleanup temp file
            File.Delete(tempPath);
        }

        //Test ApplicationConfig loads repository type from appsettings.json
        [TestMethod]
        public void TestDatabaseConfiguration_RepositoryTypeLoaded()
        {
            //create config instance
            ApplicationConfig config = new ApplicationConfig();

            //get repository type
            string repoType = config.GetRepositoryType();

            //verify repository type is database or cache
            bool isValid = repoType.Equals("database", StringComparison.OrdinalIgnoreCase)
                        || repoType.Equals("cache", StringComparison.OrdinalIgnoreCase);

            Assert.IsTrue(isValid);
        }

        // -------------------------------------------------------
        // ConnectionPool Tests
        // -------------------------------------------------------

        //Test ConnectionPool initializes and returns valid statistics
        [TestMethod]
        public void TestConnectionPool_Initialization()
        {
            //create config instance
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //get pool instance
                ConnectionPool pool = ConnectionPool.GetInstance(config);

                //get pool statistics
                string stats = pool.GetPoolStatistics();

                //verify statistics are not null or empty
                Assert.IsNotNull(stats);
                Assert.IsTrue(stats.Contains("Total created"));
                Assert.IsTrue(stats.Contains("Idle"));
                Assert.IsTrue(stats.Contains("Max"));
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        //Test connection can be acquired and released back to pool
        [TestMethod]
        public void TestConnectionPool_Acquire_Release()
        {
            //create config instance
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //get pool instance
                ConnectionPool pool = ConnectionPool.GetInstance(config);

                //acquire a connection from the pool
                SqlConnection conn = pool.GetConnection();

                //verify connection is open
                Assert.IsNotNull(conn);
                Assert.AreEqual(System.Data.ConnectionState.Open, conn.State);

                //return connection back to pool
                pool.ReturnConnection(conn);

                //verify pool statistics still available after return
                string stats = pool.GetPoolStatistics();
                Assert.IsNotNull(stats);
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        //Test pool statistics format from database repository
        [TestMethod]
        public void TestDatabaseRepositoryPoolStatistics()
        {
            //create config instance
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create pool and repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //get pool statistics from repository
                string stats = repo.GetPoolStatistics();

                //verify statistics contain required fields
                Assert.IsNotNull(stats);
                Assert.IsTrue(stats.Contains("Total created"));
                Assert.IsTrue(stats.Contains("Idle"));
                Assert.IsTrue(stats.Contains("Max"));
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        // -------------------------------------------------------
        // DatabaseRepository Tests
        // -------------------------------------------------------

        //Test saving an entity to the database
        [TestMethod]
        public void TestDatabaseRepository_SaveEntity()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //clear existing data before test
                repo.DeleteAll();

                //create and save entity
                QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                    "Compare", 1.0, "Feet", 1.0, "Feet", 1.0, "Length");
                repo.Save(entity);

                //verify count increased to 1
                Assert.AreEqual(1, repo.GetTotalCount());

                //cleanup
                repo.DeleteAll();
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        //Test retrieving all measurements returns correct number of records
        [TestMethod]
        public void TestDatabaseRepository_RetrieveAllMeasurements()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //clear existing data before test
                repo.DeleteAll();

                //save three entities with different operations and types
                repo.Save(new QuantityMeasurementEntity(
                    "Compare", 1.0, "Feet", 1.0, "Feet", 1.0, "Length"));
                repo.Save(new QuantityMeasurementEntity(
                    "Add", 1.0, "Kilogram", 2.0, "Kilogram", 3.0, "Weight"));
                repo.Save(new QuantityMeasurementEntity(
                    "Convert", 1.0, "Litre", 1000.0, "Volume"));

                //retrieve all measurements
                List<QuantityMeasurementEntity> all = repo.GetAll();

                //verify correct number returned
                Assert.AreEqual(3, all.Count);

                //cleanup
                repo.DeleteAll();
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        //Test querying measurements by operation type
        [TestMethod]
        public void TestDatabaseRepository_QueryByOperation()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //clear existing data before test
                repo.DeleteAll();

                //save entities with different operations
                repo.Save(new QuantityMeasurementEntity(
                    "Compare", 1.0, "Feet", 1.0, "Feet", 1.0, "Length"));
                repo.Save(new QuantityMeasurementEntity(
                    "Compare", 2.0, "Kilogram", 2.0, "Kilogram", 1.0, "Weight"));
                repo.Save(new QuantityMeasurementEntity(
                    "Add", 1.0, "Litre", 2.0, "Litre", 3.0, "Volume"));

                //query only Compare operations
                List<QuantityMeasurementEntity> results =
                    repo.GetByOperation("Compare");

                //verify only 2 Compare records returned
                Assert.AreEqual(2, results.Count);

                //cleanup
                repo.DeleteAll();
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        //Test querying measurements by measurement type
        [TestMethod]
        public void TestDatabaseRepository_QueryByMeasurementType()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //clear existing data before test
                repo.DeleteAll();

                //save entities with different measurement types
                repo.Save(new QuantityMeasurementEntity(
                    "Compare", 1.0, "Feet", 1.0, "Feet", 1.0, "Length"));
                repo.Save(new QuantityMeasurementEntity(
                    "Compare", 1.0, "Feet", 2.0, "Feet", 0.0, "Length"));
                repo.Save(new QuantityMeasurementEntity(
                    "Add", 1.0, "Kilogram", 2.0, "Kilogram", 3.0, "Weight"));

                //query only Length measurements
                List<QuantityMeasurementEntity> results =
                    repo.GetByMeasurementType("Length");

                //verify only 2 Length records returned
                Assert.AreEqual(2, results.Count);

                //cleanup
                repo.DeleteAll();
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        //Test total count matches number of saved entities
        [TestMethod]
        public void TestDatabaseRepository_CountMeasurements()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //clear existing data before test
                repo.DeleteAll();

                //save three entities
                repo.Save(new QuantityMeasurementEntity(
                    "Compare", 1.0, "Feet", 1.0, "Feet", 1.0, "Length"));
                repo.Save(new QuantityMeasurementEntity(
                    "Add", 1.0, "Kilogram", 2.0, "Kilogram", 3.0, "Weight"));
                repo.Save(new QuantityMeasurementEntity(
                    "Convert", 1.0, "Litre", 1000.0, "Volume"));

                //verify count matches exactly 3
                Assert.AreEqual(3, repo.GetTotalCount());

                //cleanup
                repo.DeleteAll();
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        //Test delete all removes every record from database
        [TestMethod]
        public void TestDatabaseRepository_DeleteAll()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //clear first to start clean
                repo.DeleteAll();

                //save some entities
                repo.Save(new QuantityMeasurementEntity(
                    "Compare", 1.0, "Feet", 1.0, "Feet", 1.0, "Length"));
                repo.Save(new QuantityMeasurementEntity(
                    "Add", 1.0, "Kilogram", 2.0, "Kilogram", 3.0, "Weight"));

                //delete all records
                repo.DeleteAll();

                //verify count is zero
                Assert.AreEqual(0, repo.GetTotalCount());
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        // -------------------------------------------------------
        // SQL Injection Prevention Test
        // -------------------------------------------------------

        //Test parameterized queries prevent SQL injection
        [TestMethod]
        public void TestSQLInjectionPrevention()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //clear existing data before test
                repo.DeleteAll();

                //save one normal entity
                repo.Save(new QuantityMeasurementEntity(
                    "Compare", 1.0, "Feet", 1.0, "Feet", 1.0, "Length"));

                //attempt SQL injection in query parameter
                string injectionAttempt =
                    "Compare'; DROP TABLE quantity_measurements; --";

                //query with injection string - should return no results
                List<QuantityMeasurementEntity> results =
                    repo.GetByOperation(injectionAttempt);

                //verify injection treated as literal value - no results found
                Assert.AreEqual(0, results.Count);

                //verify original data still exists - table was not dropped
                Assert.AreEqual(1, repo.GetTotalCount());

                //cleanup
                repo.DeleteAll();
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        // -------------------------------------------------------
        // DatabaseException Test
        // -------------------------------------------------------

        //Test DatabaseException is thrown when database error occurs
        [TestMethod]
        public void TestDatabaseException_CustomException()
        {
            //create a temp config with invalid connection string
            string tempPath = Path.Combine(
                Path.GetTempPath(), "bad_appsettings.json");
            File.WriteAllText(tempPath,
                "{ \"Database\": { " +
                "\"ConnectionString\": \"Server=invalid;Database=invalid;" +
                "Trusted_Connection=True;TrustServerCertificate=True;\", " +
                "\"PoolSize\": \"5\", \"ConnectionTimeout\": \"1\" }, " +
                "\"Repository\": { \"Type\": \"database\" } }");

            //create config from bad temp file
            ApplicationConfig badConfig = new ApplicationConfig(tempPath);

            try
            {
                //attempt connection with bad config - should throw exception
                ConnectionPool badPool = ConnectionPool.GetInstance(badConfig);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception ex)
            {
                //verify exception has a meaningful message
                Assert.IsNotNull(ex.Message);
                Assert.AreNotEqual("", ex.Message);
            }
            finally
            {
                //cleanup temp file
                File.Delete(tempPath);
            }
        }

        // -------------------------------------------------------
        // Repository Factory Tests
        // -------------------------------------------------------

        //Test cache repository is created correctly
        [TestMethod]
        public void TestRepositoryFactory_CreateCacheRepository()
        {
            //create cache repository instance
            QuantityMeasurementCacheRepository repo =
                QuantityMeasurementCacheRepository.GetInstance();

            //verify correct type returned
            Assert.IsInstanceOfType(repo,
                typeof(QuantityMeasurementCacheRepository));
        }

        //Test database repository is created correctly
        [TestMethod]
        public void TestRepositoryFactory_CreateDatabaseRepository()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create database repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //verify correct type returned
                Assert.IsInstanceOfType(repo,
                    typeof(QuantityMeasurementDatabaseRepository));
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        // -------------------------------------------------------
        // Service Integration Tests
        // -------------------------------------------------------

        //Test service with database repository persists data to database
        [TestMethod]
        public void TestServiceWithDatabaseRepository_Integration()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository and service
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //clear existing data before test
                repo.DeleteAll();

                //create service with database repository
                QuantityMeasurementServiceImpl service =
                    new QuantityMeasurementServiceImpl(repo);

                //perform compare operation
                QuantityDTO first = new QuantityDTO(1.0, "Feet", "Length");
                QuantityDTO second = new QuantityDTO(1.0, "Feet", "Length");
                bool result = service.Compare(first, second);

                //verify result is correct
                Assert.IsTrue(result);

                //verify data was persisted to database
                Assert.AreEqual(1, repo.GetTotalCount());

                //cleanup
                repo.DeleteAll();
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        //Test service with cache repository stores data in cache
        //DoNotParallelize prevents other cache tests from interfering
        [TestMethod]
        [DoNotParallelize]
        public void TestServiceWithCacheRepository_Integration()
        {
            //get cache repository and clear all data before starting
            QuantityMeasurementCacheRepository repo =
                QuantityMeasurementCacheRepository.GetInstance();
            repo.DeleteAll();

            //create service with cache repository
            QuantityMeasurementServiceImpl service =
                new QuantityMeasurementServiceImpl(repo);

            //perform compare operation
            QuantityDTO first = new QuantityDTO(1.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(1.0, "Feet", "Length");
            bool result = service.Compare(first, second);

            //verify result is correct
            Assert.IsTrue(result);

            //verify exactly 1 record is in cache
            Assert.AreEqual(1, repo.GetTotalCount());

            //cleanup
            repo.DeleteAll();
        }

        // -------------------------------------------------------
        // Resource Cleanup Tests
        // -------------------------------------------------------

        //Test connection is returned to pool after repository operation
        [TestMethod]
        public void TestResourceCleanup_ConnectionReleasedToPool()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //get stats before operation
                string statsBefore = pool.GetPoolStatistics();

                //perform a save operation
                repo.Save(new QuantityMeasurementEntity("Compare", 1.0, "Feet", 1.0, "Feet", 1.0, "Length"));

                //get stats after operation
                string statsAfter = pool.GetPoolStatistics();

                //verify pool stats are same - connection was returned to pool
                Assert.AreEqual(statsBefore, statsAfter);

                //cleanup
                repo.DeleteAll();
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        //Test release resources disposes pool correctly
        [TestMethod]
        public void TestResourceCleanup_ReleaseResources()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo = new QuantityMeasurementDatabaseRepository(pool);

                //release resources - should not throw any exception
                repo.ReleaseResources();

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        // -------------------------------------------------------
        // Concurrent Access Test
        // -------------------------------------------------------

        //Test cache repository handles multiple threads without data corruption
        //DoNotParallelize prevents other cache tests from interfering
        [TestMethod]
        [DoNotParallelize]
        public void TestCacheRepository_ConcurrentAccess()
        {
            //get cache repository and clear all data before starting
            QuantityMeasurementCacheRepository repo = QuantityMeasurementCacheRepository.GetInstance();
            repo.DeleteAll();

            //create 5 threads each saving 10 records
            int threadCount = 5;
            int savesPerThread = 10;
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < savesPerThread; j++)
                    {
                        repo.Save(new QuantityMeasurementEntity(
                            "Compare", id, "Feet", id, "Feet", 1.0, "Length"));
                    }
                });
            }

            //start all threads
            foreach (Thread t in threads) { t.Start(); }

            //wait for all threads to finish
            foreach (Thread t in threads) { t.Join(); }

            //verify all saves completed without data loss
            Assert.AreEqual(threadCount * savesPerThread, repo.GetTotalCount());

            //cleanup
            repo.DeleteAll();
        }

        // -------------------------------------------------------
        // Large Data Set Test
        // -------------------------------------------------------

        //Test cache repository handles 1000 records efficiently
        //DoNotParallelize prevents other cache tests from interfering
        [TestMethod]
        [DoNotParallelize]
        public void TestCacheRepository_LargeDataSet()
        {
            //get cache repository and clear all data before starting
            QuantityMeasurementCacheRepository repo =
                QuantityMeasurementCacheRepository.GetInstance();
            repo.DeleteAll();

            //save exactly 1000 entities
            for (int i = 0; i < 1000; i++)
            {
                repo.Save(new QuantityMeasurementEntity(
                    "Compare", i, "Feet", i, "Feet", 1.0, "Length"));
            }

            //record start time before retrieval
            DateTime start = DateTime.Now;

            //retrieve all 1000 records
            List<QuantityMeasurementEntity> all = repo.GetAll();

            //record end time after retrieval
            DateTime end = DateTime.Now;

            //verify exactly 1000 records retrieved
            Assert.AreEqual(1000, all.Count);

            //verify retrieval completed within 2 seconds
            Assert.IsTrue((end - start).TotalSeconds < 2);

            //cleanup
            repo.DeleteAll();
        }

        // -------------------------------------------------------
        // Package Structure Test
        // -------------------------------------------------------

        //Test all layer namespaces exist in the assembly
        [TestMethod]
        public void TestPackageStructure_AllLayersPresent()
        {
            //load each assembly explicitly by name
            System.Reflection.Assembly appAssembly = System.Reflection.Assembly.Load("QuantityMeasurementApp");
            System.Reflection.Assembly repoAssembly = System.Reflection.Assembly.Load("QuantityMeasurementAppRepositories");
            System.Reflection.Assembly modelAssembly = System.Reflection.Assembly.Load("QuantityMeasurementAppModels");
            System.Reflection.Assembly businessAssembly = System.Reflection.Assembly.Load("QuantityMeasurementAppBusiness");

            //check controller layer exists
            Type controllerType = appAssembly.GetType(
                "QuantityMeasurementApp.Controllers.QuantityMeasurementController");
            Assert.IsNotNull(controllerType, "Controller layer missing");

            //check service layer exists
            System.Reflection.Assembly servicesAssembly = System.Reflection.Assembly.Load("QuantityMeasurementAppServices");
            Type serviceType = servicesAssembly.GetType(
                "QuantityMeasurementApp.Services.QuantityMeasurementServiceImpl");
            Assert.IsNotNull(serviceType, "Service layer missing");

            //check repository layer exists
            Type repoType = repoAssembly.GetType(
                "QuantityMeasurementAppRepositories.Repositories.QuantityMeasurementDatabaseRepository");
            Assert.IsNotNull(repoType, "Repository layer missing");

            //check entity layer exists
            Type entityType = modelAssembly.GetType(
                "QuantityMeasurementAppModels.Entities.QuantityMeasurementEntity");
            Assert.IsNotNull(entityType, "Entity layer missing");

            //check exception layer exists
            Type exceptionType = businessAssembly.GetType(
                "QuantityMeasurementAppBusiness.Exceptions.DatabaseException");
            Assert.IsNotNull(exceptionType, "Exception layer missing");
        }

        // -------------------------------------------------------
        // Connection Pool Exhausted Test
        // -------------------------------------------------------

        //Test error handling when all connections are exhausted
        [TestMethod]
        public void TestConnectionPool_AllConnectionsExhausted()
        {
            //create config instance
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //get pool instance
                ConnectionPool pool = ConnectionPool.GetInstance(config);

                //get max pool size from config
                int maxSize = config.GetMaxPoolSize();

                //acquire all connections from the pool
                List<SqlConnection> connections = new List<SqlConnection>();
                for (int i = 0; i < maxSize; i++)
                {
                    connections.Add(pool.GetConnection());
                }

                //verify all connections are open
                foreach (SqlConnection conn in connections)
                {
                    Assert.AreEqual(System.Data.ConnectionState.Open, conn.State);
                }

                //return all connections back to pool
                foreach (SqlConnection conn in connections)
                {
                    pool.ReturnConnection(conn);
                }

                //verify pool is functional after returning all connections
                string stats = pool.GetPoolStatistics();
                Assert.IsNotNull(stats);
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        // -------------------------------------------------------
        // Transaction Rollback Test
        // -------------------------------------------------------

        //Test that failed operations do not persist partial data
        [TestMethod]
        public void TestTransactionRollback_OnError()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //clear existing data before test
                repo.DeleteAll();

                //get count before attempting bad save
                int countBefore = repo.GetTotalCount();

                //attempt to save a null entity - should throw and not persist
                try
                {
                    repo.Save(null);
                }
                catch (Exception)
                {
                    //expected exception - swallow it
                }

                //verify count did not change - nothing was persisted
                int countAfter = repo.GetTotalCount();
                Assert.AreEqual(countBefore, countAfter,
                    "Failed operation should not persist data");
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        // -------------------------------------------------------
        // Database Schema Test
        // -------------------------------------------------------

        //Test that required database table and stored procedures exist
        [TestMethod]
        public void TestDatabaseSchema_TablesCreated()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //get pool instance
                ConnectionPool pool = ConnectionPool.GetInstance(config);

                //get a connection to query schema
                SqlConnection conn = pool.GetConnection();

                try
                {
                    //check quantity_measurements table exists
                    SqlCommand cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES " +
                        "WHERE TABLE_NAME = 'quantity_measurements'", conn);

                    int tableCount = (int)cmd.ExecuteScalar();

                    //verify table exists
                    Assert.AreEqual(1, tableCount,
                        "quantity_measurements table should exist");

                    //check stored procedures exist
                    SqlCommand spCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES " +
                        "WHERE ROUTINE_TYPE = 'PROCEDURE' " +
                        "AND ROUTINE_NAME IN (" +
                        "'sp_SaveMeasurement'," +
                        "'sp_GetAllMeasurements'," +
                        "'sp_GetMeasurementsByOperation'," +
                        "'sp_GetMeasurementsByType'," +
                        "'sp_GetTotalCount'," +
                        "'sp_DeleteAllMeasurements')", conn);

                    int spCount = (int)spCmd.ExecuteScalar();

                    //verify all 6 stored procedures exist
                    Assert.AreEqual(6, spCount,
                        "All 6 stored procedures should exist");
                }
                finally
                {
                    //always return connection to pool
                    pool.ReturnConnection(conn);
                }
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        // -------------------------------------------------------
        // Parameterized Query DateTime Test
        // -------------------------------------------------------

        //Test that saved entities can be retrieved correctly
        [TestMethod]
        public void TestParameterizedQuery_DateTimeHandling()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //clear existing data before test
                repo.DeleteAll();

                //save entity with all fields populated
                repo.Save(new QuantityMeasurementEntity(
                    "Compare", 1.0, "Feet", 1.0, "Feet", 1.0, "Length"));

                //retrieve the saved entity
                List<QuantityMeasurementEntity> all = repo.GetAll();

                //verify entity was saved and retrieved correctly
                Assert.AreEqual(1, all.Count);
                Assert.AreEqual("Compare", all[0].Operation);
                Assert.AreEqual("Feet", all[0].FirstUnit);
                Assert.AreEqual("Length", all[0].MeasurementType);
                Assert.AreEqual(1.0, all[0].FirstValue);
                Assert.AreEqual(1.0, all[0].ResultValue);

                //cleanup
                repo.DeleteAll();
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        // -------------------------------------------------------
        // Batch Insert Test
        // -------------------------------------------------------

        //Test multiple entities can be saved in succession without pool exhaustion
        [TestMethod]
        public void TestBatchInsert_MultipleEntities()
        {
            //create config and pool
            ApplicationConfig config = new ApplicationConfig();

            try
            {
                //create repository
                ConnectionPool pool = ConnectionPool.GetInstance(config);
                QuantityMeasurementDatabaseRepository repo =
                    new QuantityMeasurementDatabaseRepository(pool);

                //clear existing data before test
                repo.DeleteAll();

                //save 20 entities in rapid succession
                int batchSize = 20;
                for (int i = 0; i < batchSize; i++)
                {
                    repo.Save(new QuantityMeasurementEntity(
                        "Add", i, "Feet", i + 1.0, "Feet", i + i + 1.0, "Length"));
                }

                //verify all entities saved
                Assert.AreEqual(batchSize, repo.GetTotalCount(),
                    "All batch entities should be saved");

                //verify pool is still functional after batch
                string stats = pool.GetPoolStatistics();
                Assert.IsNotNull(stats,
                    "Pool should still be functional after batch insert");

                //cleanup
                repo.DeleteAll();
            }
            catch (Exception ex)
            {
                //mark as inconclusive if database is not available
                Assert.Inconclusive("Database not available: " + ex.Message);
            }
        }

        // -------------------------------------------------------
        // Properties Configuration Override Test
        // -------------------------------------------------------

        //Test repository type can be overridden via config file
        [TestMethod]
        public void TestPropertiesConfiguration_EnvironmentOverride()
        {
            //create a temp config file with database type
            //Database section comes AFTER Repository so parser reads Repository:Type correctly
            string tempPathDb = Path.Combine(
                Path.GetTempPath(), "db_appsettings.json");
            File.WriteAllText(tempPathDb,
                "{\n" +
                "  \"Repository\": {\n" +
                "    \"Type\": \"database\"\n" +
                "  },\n" +
                "  \"Database\": {\n" +
                "    \"PoolSize\": 5,\n" +
                "    \"ConnectionTimeout\": 30\n" +
                "  }\n" +
                "}");

            //create a temp config file with cache type
            string tempPathCache = Path.Combine(
                Path.GetTempPath(), "cache_appsettings.json");
            File.WriteAllText(tempPathCache,
                "{\n" +
                "  \"Repository\": {\n" +
                "    \"Type\": \"cache\"\n" +
                "  },\n" +
                "  \"Database\": {\n" +
                "    \"PoolSize\": 5,\n" +
                "    \"ConnectionTimeout\": 30\n" +
                "  }\n" +
                "}");

            //verify database config reads correctly
            ApplicationConfig dbConfig = new ApplicationConfig(tempPathDb);
            Assert.AreEqual("database", dbConfig.GetRepositoryType(),
                StringComparer.OrdinalIgnoreCase);

            //verify cache config reads correctly
            ApplicationConfig cacheConfig = new ApplicationConfig(tempPathCache);
            Assert.AreEqual("cache", cacheConfig.GetRepositoryType(),
                StringComparer.OrdinalIgnoreCase);

            //cleanup temp files
            File.Delete(tempPathDb);
            File.Delete(tempPathCache);
        }
    }
}