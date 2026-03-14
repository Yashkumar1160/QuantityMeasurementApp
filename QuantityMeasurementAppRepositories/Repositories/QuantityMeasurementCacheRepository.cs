using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Interfaces;

namespace QuantityMeasurementAppRepositories.Repositories
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository instance;
        private static readonly object lockObject = new object();

        private List<QuantityMeasurementEntity> history;
        private const string path = @"C:\Users\yk444\Videos\BTECH\QuantityMeasurementApp\UC-15\QuantityMeasurementAppRepositories\QuantityMeasurement.json";

        private QuantityMeasurementCacheRepository()
        {
            history = new List<QuantityMeasurementEntity>();
            LoadFromDisk();
        }

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

        public void Save(QuantityMeasurementEntity entity)
        {
            history.Add(entity);
            SaveToDisk();
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            return new List<QuantityMeasurementEntity>(history);
        }

        private void SaveToDisk()
        {
            try
            {
                string json = JsonSerializer.Serialize(history);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Warning: Could not save to disk: " + ex.Message);
            }
        }

        private void LoadFromDisk()
        {
            try
            {
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    history = JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Warning: Could not load from disk: " + ex.Message);
                history = new List<QuantityMeasurementEntity>();
            }
        }
    }
}