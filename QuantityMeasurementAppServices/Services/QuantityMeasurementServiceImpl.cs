using System;
using QuantityMeasurementAppServices.Interfaces;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppModels.Enums;
using QuantityMeasurementAppBusiness.Exceptions;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppBusiness;
using QuantityMeasurementAppBusiness.Implementations;

namespace QuantityMeasurementApp.Services
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {

        // Instance of IQuantityRecordRepository
        private readonly IQuantityRecordRepository repository;

        // Constructor
        public QuantityMeasurementServiceImpl(IQuantityRecordRepository repository)
        {
            this.repository = repository;
        }

        // Method to convert string to IMeasurable object (required to perform operations present in Quantity class)
        private IMeasurable ResolveUnit(string measurementType, string unitName)
        {
            if (measurementType.ToLower() == "length")
            {
                LengthUnit lu = (LengthUnit)Enum.Parse(typeof(LengthUnit), unitName, true);
                return new LengthMeasurementImpl(lu);
            }
            else if (measurementType.ToLower() == "weight")
            {
                WeightUnit wu = (WeightUnit)Enum.Parse(typeof(WeightUnit), unitName, true);
                return new WeightMeasurementImpl(wu);
            }
            else if (measurementType.ToLower() == "volume")
            {
                VolumeUnit vu = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), unitName, true);
                return new VolumeMeasurementImpl(vu);
            }
            else if (measurementType.ToLower() == "temperature")
            {
                TemperatureUnit tu = (TemperatureUnit)Enum.Parse(typeof(TemperatureUnit), unitName, true);
                return new TemperatureMeasurementImpl(tu);
            }
            else
            {
                throw new ArgumentException("Unknown measurement type: " + measurementType);
            }
        }

        // Method for Addition
        public QuantityDTO Add(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                IMeasurable unit1 = ResolveUnit(first.MeasurementType, first.UnitName);
                IMeasurable unit2 = ResolveUnit(second.MeasurementType, second.UnitName);
                IMeasurable target = ResolveUnit(first.MeasurementType, targetUnit);

                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(first.Value, unit1);
                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(second.Value, unit2);
                Quantity<IMeasurable> result = q1.Add(q2, target);

                repository.Save(new QuantityMeasurementEntity(
                    OperationType.Add.ToString(),
                    first.Value, first.UnitName,
                    second.Value, second.UnitName,
                    result.GetValue(),
                    first.MeasurementType));

                return new QuantityDTO(result.GetValue(), targetUnit, first.MeasurementType);
            }
            catch (Exception ex)
            {
                repository.Save(new QuantityMeasurementEntity(OperationType.Add.ToString(), ex.Message));

                throw new QuantityMeasurementException("Add operation failed: " + ex.Message, ex);
            }
        }

        // Method for Subtraction
        public QuantityDTO Subtract(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                IMeasurable unit1 = ResolveUnit(first.MeasurementType, first.UnitName);
                IMeasurable unit2 = ResolveUnit(second.MeasurementType, second.UnitName);
                IMeasurable target = ResolveUnit(first.MeasurementType, targetUnit);

                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(first.Value, unit1);
                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(second.Value, unit2);
                Quantity<IMeasurable> result = q1.Subtract(q2, target);

                repository.Save(new QuantityMeasurementEntity(
                    OperationType.Subtract.ToString(),
                    first.Value, first.UnitName,
                    second.Value, second.UnitName,
                    result.GetValue(),
                    first.MeasurementType));

                return new QuantityDTO(result.GetValue(), targetUnit, first.MeasurementType);
            }
            catch (Exception ex)
            {
                repository.Save(new QuantityMeasurementEntity(OperationType.Subtract.ToString(), ex.Message));

                throw new QuantityMeasurementException("Subtract operation failed: " + ex.Message, ex);
            }
        }

        // Method for Division
        public double Divide(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                IMeasurable unit1 = ResolveUnit(first.MeasurementType, first.UnitName);
                IMeasurable unit2 = ResolveUnit(second.MeasurementType, second.UnitName);

                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(first.Value, unit1);
                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(second.Value, unit2);
                double result = q1.Divide(q2);

                repository.Save(new QuantityMeasurementEntity(
                    OperationType.Divide.ToString(),
                    first.Value, first.UnitName,
                    second.Value, second.UnitName,
                    result,
                    first.MeasurementType));

                return result;
            }
            catch (Exception ex)
            {
                repository.Save(new QuantityMeasurementEntity(OperationType.Divide.ToString(), ex.Message));

                throw new QuantityMeasurementException("Divide operation failed: " + ex.Message, ex);
            }
        }

        // Method for Comparison
        public bool Compare(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                IMeasurable unit1 = ResolveUnit(first.MeasurementType, first.UnitName);
                IMeasurable unit2 = ResolveUnit(second.MeasurementType, second.UnitName);

                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(first.Value, unit1);
                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(second.Value, unit2);
                bool result = q1.Equals(q2);

                repository.Save(new QuantityMeasurementEntity(
                    OperationType.Compare.ToString(),
                    first.Value, first.UnitName,
                    second.Value, second.UnitName,
                    result ? 1 : 0,
                    first.MeasurementType));

                return result;
            }
            catch (Exception ex)
            {
                repository.Save(new QuantityMeasurementEntity(OperationType.Compare.ToString(), ex.Message));

                throw new QuantityMeasurementException("Compare operation failed: " + ex.Message, ex);
            }
        }

        // Method for Conversion
        public QuantityDTO Convert(QuantityDTO quantity, string targetUnit)
        {
            try
            {
                IMeasurable unit = ResolveUnit(quantity.MeasurementType, quantity.UnitName);
                IMeasurable target = ResolveUnit(quantity.MeasurementType, targetUnit);

                Quantity<IMeasurable> q = new Quantity<IMeasurable>(quantity.Value, unit);
                Quantity<IMeasurable> result = q.ConvertTo(target);

                repository.Save(new QuantityMeasurementEntity(
                    OperationType.Convert.ToString(),
                    quantity.Value, quantity.UnitName,
                    result.GetValue(),
                    quantity.MeasurementType));

                return new QuantityDTO(result.GetValue(), targetUnit, quantity.MeasurementType);
            }
            catch (Exception ex)
            {
                repository.Save(new QuantityMeasurementEntity(OperationType.Convert.ToString(), ex.Message));

                throw new QuantityMeasurementException("Convert operation failed: " + ex.Message, ex);
            }
        }
    }
}