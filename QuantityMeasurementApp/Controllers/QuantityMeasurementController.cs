using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppServices.Interfaces;

namespace QuantityMeasurementApp.Controllers
{
    public class QuantityMeasurementController
    {
        private IQuantityMeasurementService service;

        // Controller
        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            if (service == null)
            {
                throw new ArgumentException("Service cannot be null");
            }
            this.service = service;
        }

        // Method to compare lengths
        public void PerformLengthComparison(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                bool result = service.Compare(first, second);
                if (result)
                    Console.WriteLine("Both Lengths Are Equal");
                else
                    Console.WriteLine("Lengths Are Not Equal");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method to cnvert length
        public void PerformLengthConversion(QuantityDTO quantity, string targetUnit)
        {
            try
            {
                QuantityDTO result = service.Convert(quantity, targetUnit);
                Console.WriteLine(quantity + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for length addition
        public void PerformLengthAddition(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                QuantityDTO result = service.Add(first, second, targetUnit);
                Console.WriteLine(first + " + " + second + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for length subtraction
        public void PerformLengthSubtraction(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                QuantityDTO result = service.Subtract(first, second, targetUnit);
                Console.WriteLine(first + " - " + second + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for length division
        public void PerformLengthDivision(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                double result = service.Divide(first, second);
                Console.WriteLine(first + " / " + second + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for weight comparison
        public void PerformWeightComparison(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                bool result = service.Compare(first, second);
                if (result)
                    Console.WriteLine("Weights Are Equal");
                else
                    Console.WriteLine("Weights Are Not Equal");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for weight conversion
        public void PerformWeightConversion(QuantityDTO quantity, string targetUnit)
        {
            try
            {
                QuantityDTO result = service.Convert(quantity, targetUnit);
                Console.WriteLine(quantity + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for weight addition
        public void PerformWeightAddition(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                QuantityDTO result = service.Add(first, second, targetUnit);
                Console.WriteLine(first + " + " + second + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for weight subtraction
        public void PerformWeightSubtraction(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                QuantityDTO result = service.Subtract(first, second, targetUnit);
                Console.WriteLine(first + " - " + second + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for weight division
        public void PerformWeightDivision(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                double result = service.Divide(first, second);
                Console.WriteLine(first + " / " + second + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for volume comparison
        public void PerformVolumeComparison(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                bool result = service.Compare(first, second);
                if (result)
                    Console.WriteLine("Volumes Are Equal");
                else
                    Console.WriteLine("Volumes Are Not Equal");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for volume conversion
        public void PerformVolumeConversion(QuantityDTO quantity, string targetUnit)
        {
            try
            {
                QuantityDTO result = service.Convert(quantity, targetUnit);
                Console.WriteLine(quantity + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for volume addition
        public void PerformVolumeAddition(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                QuantityDTO result = service.Add(first, second, targetUnit);
                Console.WriteLine(first + " + " + second + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for volume subtraction
        public void PerformVolumeSubtraction(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                QuantityDTO result = service.Subtract(first, second, targetUnit);
                Console.WriteLine(first + " - " + second + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for volume division
        public void PerformVolumeDivision(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                double result = service.Divide(first, second);
                Console.WriteLine(first + " / " + second + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for temperature comparison
        public void PerformTemperatureComparison(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                bool result = service.Compare(first, second);
                if (result)
                    Console.WriteLine("Temperatures Are Equal");
                else
                    Console.WriteLine("Temperatures Are Not Equal");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for temperature conversion
        public void PerformTemperatureConversion(QuantityDTO quantity, string targetUnit)
        {
            try
            {
                QuantityDTO result = service.Convert(quantity, targetUnit);
                Console.WriteLine(quantity + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Method for temperature arithmetic operation
        public void PerformTemperatureArithmetic(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            try
            {
                QuantityDTO result = service.Add(first, second, targetUnit);
                Console.WriteLine(first + " + " + second + " = " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}