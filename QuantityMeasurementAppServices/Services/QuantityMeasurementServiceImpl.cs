using QuantityMeasurementAppServices.Interfaces;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppModels.Enums;
using QuantityMeasurementAppBusiness.Exceptions;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppBusiness;
using QuantityMeasurementAppBusiness.Implementations;
using QuantityMeasurementAppRepositories.Interfaces;

namespace QuantityMeasurementApp.Services
{
    // This service is responsible ONLY for calculation logic.
    // Saving records to the database is done in QuantityWebServiceImpl, which has access to the current userId from the JWT token.
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        // Empty Constructor 
        public QuantityMeasurementServiceImpl(IQuantityRecordRepository repository)
        {
        }

        // Method to Resolve a measurement type + unit name string into an IMeasurable object
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

        // Method to Add two quantities and return the result
        public QuantityDTO Add(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                IMeasurable unit1  = ResolveUnit(first.MeasurementType, first.UnitName);
                IMeasurable unit2  = ResolveUnit(second.MeasurementType, second.UnitName);
                IMeasurable target = ResolveUnit(first.MeasurementType, targetUnit);

                Quantity<IMeasurable> q1     = new Quantity<IMeasurable>(first.Value, unit1);
                Quantity<IMeasurable> q2     = new Quantity<IMeasurable>(second.Value, unit2);
                Quantity<IMeasurable> result = q1.Add(q2, target);

                return new QuantityDTO(result.GetValue(), targetUnit, first.MeasurementType);
            }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException("Add operation failed: " + ex.Message, ex);
            }
        }

        // Method to Subtract one quantity from another and return the result
        public QuantityDTO Subtract(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                IMeasurable unit1  = ResolveUnit(first.MeasurementType, first.UnitName);
                IMeasurable unit2  = ResolveUnit(second.MeasurementType, second.UnitName);
                IMeasurable target = ResolveUnit(first.MeasurementType, targetUnit);

                Quantity<IMeasurable> q1     = new Quantity<IMeasurable>(first.Value, unit1);
                Quantity<IMeasurable> q2     = new Quantity<IMeasurable>(second.Value, unit2);
                Quantity<IMeasurable> result = q1.Subtract(q2, target);

                return new QuantityDTO(result.GetValue(), targetUnit, first.MeasurementType);
            }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException("Subtract operation failed: " + ex.Message, ex);
            }
        }

        // Method to Divide one quantity by another and return the scalar result
        public double Divide(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                IMeasurable unit1 = ResolveUnit(first.MeasurementType, first.UnitName);
                IMeasurable unit2 = ResolveUnit(second.MeasurementType, second.UnitName);

                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(first.Value, unit1);
                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(second.Value, unit2);

                return q1.Divide(q2);
            }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException("Divide operation failed: " + ex.Message, ex);
            }
        }

        // Method to Compare two quantities for equality and return true/false
        public bool Compare(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                IMeasurable unit1 = ResolveUnit(first.MeasurementType, first.UnitName);
                IMeasurable unit2 = ResolveUnit(second.MeasurementType, second.UnitName);

                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(first.Value, unit1);
                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(second.Value, unit2);

                return q1.Equals(q2);
            }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException("Compare operation failed: " + ex.Message, ex);
            }
        }

        // Method to Convert a quantity to a different unit and return the result
        public QuantityDTO Convert(QuantityDTO quantity, string targetUnit)
        {
            try
            {
                IMeasurable unit   = ResolveUnit(quantity.MeasurementType, quantity.UnitName);
                IMeasurable target = ResolveUnit(quantity.MeasurementType, targetUnit);

                Quantity<IMeasurable> q      = new Quantity<IMeasurable>(quantity.Value, unit);
                Quantity<IMeasurable> result = q.ConvertTo(target);

                return new QuantityDTO(result.GetValue(), targetUnit, quantity.MeasurementType);
            }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException("Convert operation failed: " + ex.Message, ex);
            }
        }
    }
}