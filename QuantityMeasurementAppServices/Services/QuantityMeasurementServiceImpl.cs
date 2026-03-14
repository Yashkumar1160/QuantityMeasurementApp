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
        private readonly IQuantityMeasurementRepository repository;

        //Constructor
        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
        {
            this.repository = repository;
        }

        //Method to convert string to IMeasurable object
        private IMeasurable ResolveUnit(string measurementType, string unitName)
        {
            switch (measurementType.ToLower())
            {
                case "length":
                    LengthUnit lu = (LengthUnit)Enum.Parse(typeof(LengthUnit), unitName, true);
                    return new LengthMeasurementImpl(lu);

                case "weight":
                    WeightUnit wu = (WeightUnit)Enum.Parse(typeof(WeightUnit), unitName, true);
                    return new WeightMeasurementImpl(wu);

                case "volume":
                    VolumeUnit vu = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), unitName, true);
                    return new VolumeMeasurementImpl(vu);

                case "temperature":
                    TemperatureUnit tu = (TemperatureUnit)Enum.Parse(typeof(TemperatureUnit), unitName, true);
                    return new TemperatureMeasurementImpl(tu);

                default:
                    throw new ArgumentException("Unknown measurement type: " + measurementType);
            }
        }

        //Implementation of Add Method
        public QuantityDTO Add(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                //Resolve units directly from QuantityDTO
                IMeasurable unit1  = ResolveUnit(first.MeasurementType,  first.UnitName);
                IMeasurable unit2  = ResolveUnit(second.MeasurementType, second.UnitName);
                IMeasurable target = ResolveUnit(first.MeasurementType,  targetUnit);

                //Create Quantity objects and perform addition
                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(first.Value,  unit1);
                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(second.Value, unit2);
                Quantity<IMeasurable> result = q1.Add(q2, target);

                //Save to repository
                repository.Save(new QuantityMeasurementEntity("Add",
                    first.Value,  first.UnitName,
                    second.Value, second.UnitName,
                    result.GetValue()));

                return new QuantityDTO(result.GetValue(), targetUnit, first.MeasurementType);
            }
            catch (Exception ex)
            {
                repository.Save(new QuantityMeasurementEntity("Add", ex.Message));
                throw new QuantityMeasurementException("Add operation failed: " + ex.Message, ex);
            }
        }

        //Implementation of Subtract Method
        public QuantityDTO Subtract(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                //Resolve units directly from QuantityDTO
                IMeasurable unit1  = ResolveUnit(first.MeasurementType,  first.UnitName);
                IMeasurable unit2  = ResolveUnit(second.MeasurementType, second.UnitName);
                IMeasurable target = ResolveUnit(first.MeasurementType,  targetUnit);

                //Create Quantity objects and perform subtraction
                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(first.Value,  unit1);
                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(second.Value, unit2);
                Quantity<IMeasurable> result = q1.Subtract(q2, target);

                //Save to repository
                repository.Save(new QuantityMeasurementEntity("Subtract",
                    first.Value,  first.UnitName,
                    second.Value, second.UnitName,
                    result.GetValue()));

                return new QuantityDTO(result.GetValue(), targetUnit, first.MeasurementType);
            }
            catch (Exception ex)
            {
                repository.Save(new QuantityMeasurementEntity("Subtract", ex.Message));
                throw new QuantityMeasurementException("Subtract operation failed: " + ex.Message, ex);
            }
        }

        //Implementation of Divide Method
        public double Divide(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                //Resolve units directly from QuantityDTO
                IMeasurable unit1 = ResolveUnit(first.MeasurementType,  first.UnitName);
                IMeasurable unit2 = ResolveUnit(second.MeasurementType, second.UnitName);

                //Create Quantity objects and perform division
                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(first.Value,  unit1);
                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(second.Value, unit2);
                double result = q1.Divide(q2);

                //Save to repository
                repository.Save(new QuantityMeasurementEntity("Divide",
                    first.Value,  first.UnitName,
                    second.Value, second.UnitName,
                    result));

                return result;
            }
            catch (Exception ex)
            {
                repository.Save(new QuantityMeasurementEntity("Divide", ex.Message));
                throw new QuantityMeasurementException("Divide operation failed: " + ex.Message, ex);
            }
        }

        //Implementation of Compare Method
        public bool Compare(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                //Resolve units directly from QuantityDTO
                IMeasurable unit1 = ResolveUnit(first.MeasurementType,  first.UnitName);
                IMeasurable unit2 = ResolveUnit(second.MeasurementType, second.UnitName);

                //Create Quantity objects and compare
                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(first.Value,  unit1);
                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(second.Value, unit2);
                bool result = q1.Equals(q2);

                //Save to repository
                repository.Save(new QuantityMeasurementEntity("Compare",
                    first.Value,  first.UnitName,
                    second.Value, second.UnitName,
                    result ? 1 : 0));

                return result;
            }
            catch (Exception ex)
            {
                repository.Save(new QuantityMeasurementEntity("Compare", ex.Message));
                throw new QuantityMeasurementException("Compare operation failed: " + ex.Message, ex);
            }
        }

        //Implementation of Convert Method
        public QuantityDTO Convert(QuantityDTO quantity, string targetUnit)
        {
            try
            {
                //Resolve units directly from QuantityDTO
                IMeasurable unit   = ResolveUnit(quantity.MeasurementType, quantity.UnitName);
                IMeasurable target = ResolveUnit(quantity.MeasurementType, targetUnit);

                //Create Quantity object and perform conversion
                Quantity<IMeasurable> q = new Quantity<IMeasurable>(quantity.Value, unit);
                Quantity<IMeasurable> result = q.ConvertTo(target);

                //Save to repository
                repository.Save(new QuantityMeasurementEntity("Convert",
                    quantity.Value, quantity.UnitName,
                    result.GetValue()));

                return new QuantityDTO(result.GetValue(), targetUnit, quantity.MeasurementType);
            }
            catch (Exception ex)
            {
                repository.Save(new QuantityMeasurementEntity("Convert", ex.Message));
                throw new QuantityMeasurementException("Convert operation failed: " + ex.Message, ex);
            }
        }
    }
}