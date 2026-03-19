using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Interfaces;

namespace QuantityMeasurementAppRepositories.Repositories
{
    // UC16 changes:
    // 1. Fixed hardcoded path
    // 2. Implemented the 6 new interface methods
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        // Singleton pattern instance
        private static QuantityMeasurementCacheRepository instance;
        private static readonly object lockObject = new object();

        // List to store operation history
        private List<QuantityMeasurementEntity> history;

        // 
        private static readonly string filePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "QuantityMeasurement.json");

        // Constructor
        private QuantityMeasurementCacheRepository()
        {
            history = new List<QuantityMeasurementEntity>();
            LoadFromDisk();
            Console.WriteLine("[CacheRepository] Initialized. File: " + filePath);
        }

        // Method to get the instance (Singleton Pattern)
        public static QuantityMeasurementCacheRepository GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new QuantityMeasurementCacheRepository();
                    }
                }
            }
            return instance;
        }

        // Method to save entity to cache
        public void Save(QuantityMeasurementEntity entity)
        {
            //lock only the list add to prevent race conditions
            lock (lockObject)
            {
                history.Add(entity);
            }

            //save to disk outside the lock so threads dont block each other on file access
            SaveToDisk();
        }


        // Method to get all entities from cache 
        public List<QuantityMeasurementEntity> GetAll()
        {
            return new List<QuantityMeasurementEntity>(history);
        }

        // ---- UC16 methods ----


        // Method to get entities by operation type
        public List<QuantityMeasurementEntity> GetByOperation(string operation)
        {
            List<QuantityMeasurementEntity> result = new List<QuantityMeasurementEntity>();

            foreach (QuantityMeasurementEntity entity in history)
            {
                if (entity.Operation != null && entity.Operation.Equals(operation, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(entity);
                }
            }
            return result;
        }


        // Method to get entities by measurement type
        public List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType)
        {
            List<QuantityMeasurementEntity> result = new List<QuantityMeasurementEntity>();

            foreach (QuantityMeasurementEntity entity in history)
            {
                if (entity.MeasurementType != null && entity.MeasurementType.Equals(measurementType, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(entity);
                }
            }
            return result;
        }

        // Method to get total count of repository 
        public int GetTotalCount()
        {
            return history.Count;
        }


        // Method to delete all entities from cache
        public void DeleteAll()
        {
            history.Clear();
            SaveToDisk();
            Console.WriteLine("[CacheRepository] All measurements deleted.");
        }


        // Method to get pool statistics (no pool in cache)
        public string GetPoolStatistics()
        {
            return "In-memory cache. No connection pool. Records: " + history.Count;
        }

        // Method to release resources (nothing to release in cache)
        public void ReleaseResources()
        {
            Console.WriteLine("[CacheRepository] No resources to release.");
        }

        // Method to save history to disk as JSON
        private void SaveToDisk()
        {
            try
            {
                string json = JsonSerializer.Serialize(history);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[CacheRepository] Warning: could not save: " + ex.Message);
            }
        }

        // Method to load history from disk JSON file
        private void LoadFromDisk()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    history = JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[CacheRepository] Warning: could not load: " + ex.Message);
                history = new List<QuantityMeasurementEntity>();
            }
        }
    }
}