using QuantityMeasurementApp.Controllers;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementApp.Interfaces;

namespace QuantityMeasurementApp.Menu
{
    public class QuantityMenu:IMenu
    {
        // Controller
        private QuantityMeasurementController controller;

        // Constructor
        public QuantityMenu(QuantityMeasurementController controller)
        {   
            this.controller = controller;
        }

        public void Run()
        {
            // infinite loop until user exits
            while (true)
            {
                // display menu
                Console.WriteLine("===== Quantity Measurement App =====");
                Console.WriteLine("1.  Compare Two Lengths");
                Console.WriteLine("2.  Convert Length");
                Console.WriteLine("3.  Add Two Lengths");
                Console.WriteLine("4.  Subtract Two Lengths");
                Console.WriteLine("5.  Divide Two Lengths");
                Console.WriteLine("6.  Compare Two Weights");
                Console.WriteLine("7.  Convert Weight");
                Console.WriteLine("8.  Add Two Weights");
                Console.WriteLine("9.  Subtract Two Weights");
                Console.WriteLine("10. Divide Two Weights");
                Console.WriteLine("11. Compare Two Volumes");
                Console.WriteLine("12. Convert Volume");
                Console.WriteLine("13. Add Two Volumes");
                Console.WriteLine("14. Subtract Two Volumes");
                Console.WriteLine("15. Divide Two Volumes");
                Console.WriteLine("16. Compare Two Temperatures");
                Console.WriteLine("17. Convert Temperature");
                Console.WriteLine("18. Try Temperature Arithmetic");
                Console.WriteLine("0.  Exit");
                Console.Write("Enter Your Choice: ");

                // Take user's choice
                int choice = int.Parse(Console.ReadLine());

                // Exit
                if (choice == 0)
                {
                    Console.WriteLine("Exit Successful");
                    return;
                }
                // Compare two lengths
                else if (choice == 1)
                {
                    QuantityDTO first = ReadQuantity("First", "Length", "Feet/Inch/Centimeter/Yard");
                    QuantityDTO second = ReadQuantity("Second", "Length", "Feet/Inch/Centimeter/Yard");
                    controller.PerformLengthComparison(first, second);
                }
                // Convert Length
                else if (choice == 2)
                {
                    QuantityDTO quantity = ReadQuantity("", "Length", "Feet/Inch/Centimeter/Yard");
                    Console.Write("Enter Target Unit (Feet/Inch/Centimeter/Yard): ");
                    string targetUnit = Console.ReadLine();
                    controller.PerformLengthConversion(quantity, targetUnit);
                }
                // Add Two Lengths
                else if (choice == 3)
                {
                    QuantityDTO first = ReadQuantity("First", "Length", "Feet/Inch/Centimeter/Yard");
                    QuantityDTO second = ReadQuantity("Second", "Length", "Feet/Inch/Centimeter/Yard");
                    Console.Write("Enter Target Unit (Feet/Inch/Centimeter/Yard): ");
                    string targetUnit = Console.ReadLine();
                    controller.PerformLengthAddition(first, second, targetUnit);
                }
                // Subtract Two Lengths
                else if (choice == 4)
                {
                    QuantityDTO first = ReadQuantity("First", "Length", "Feet/Inch/Centimeter/Yard");
                    QuantityDTO second = ReadQuantity("Second", "Length", "Feet/Inch/Centimeter/Yard");
                    Console.Write("Enter Target Unit (Feet/Inch/Centimeter/Yard): ");
                    string targetUnit = Console.ReadLine();
                    controller.PerformLengthSubtraction(first, second, targetUnit);
                }
                // Divide Two Lengths
                else if (choice == 5)
                {
                    QuantityDTO first = ReadQuantity("First", "Length", "Feet/Inch/Centimeter/Yard");
                    QuantityDTO second = ReadQuantity("Second", "Length", "Feet/Inch/Centimeter/Yard");
                    controller.PerformLengthDivision(first, second);
                }
                // Compare Weights
                else if (choice == 6)
                {
                    QuantityDTO first = ReadQuantity("First", "Weight", "Kilogram/Gram/Pound");
                    QuantityDTO second = ReadQuantity("Second", "Weight", "Kilogram/Gram/Pound");
                    controller.PerformWeightComparison(first, second);
                }
                // Convert Weight
                else if (choice == 7)
                {
                    QuantityDTO quantity = ReadQuantity("", "Weight", "Kilogram/Gram/Pound");
                    Console.Write("Enter Target Unit (Kilogram/Gram/Pound): ");
                    string targetUnit = Console.ReadLine();
                    controller.PerformWeightConversion(quantity, targetUnit);
                }
                // Add Weight
                else if (choice == 8)
                {
                    QuantityDTO first = ReadQuantity("First", "Weight", "Kilogram/Gram/Pound");
                    QuantityDTO second = ReadQuantity("Second", "Weight", "Kilogram/Gram/Pound");
                    Console.Write("Enter Target Unit (Kilogram/Gram/Pound): ");
                    string targetUnit = Console.ReadLine();
                    controller.PerformWeightAddition(first, second, targetUnit);
                }
                // Subtract Weights
                else if (choice == 9)
                {
                    QuantityDTO first = ReadQuantity("First", "Weight", "Kilogram/Gram/Pound");
                    QuantityDTO second = ReadQuantity("Second", "Weight", "Kilogram/Gram/Pound");
                    Console.Write("Enter Target Unit (Kilogram/Gram/Pound): ");
                    string targetUnit = Console.ReadLine();
                    controller.PerformWeightSubtraction(first, second, targetUnit);
                }
                // Weight Division
                else if (choice == 10)
                {
                    QuantityDTO first = ReadQuantity("First", "Weight", "Kilogram/Gram/Pound");
                    QuantityDTO second = ReadQuantity("Second", "Weight", "Kilogram/Gram/Pound");
                    controller.PerformWeightDivision(first, second);
                }
                // Compare Volume
                else if (choice == 11)
                {
                    QuantityDTO first = ReadQuantity("First", "Volume", "Litre/Millilitre/Gallon");
                    QuantityDTO second = ReadQuantity("Second", "Volume", "Litre/Millilitre/Gallon");
                    controller.PerformVolumeComparison(first, second);
                }
                // Convert Volume
                else if (choice == 12)
                {
                    QuantityDTO quantity = ReadQuantity("", "Volume", "Litre/Millilitre/Gallon");
                    Console.Write("Enter Target Unit (Litre/Millilitre/Gallon): ");
                    string targetUnit = Console.ReadLine();
                    controller.PerformVolumeConversion(quantity, targetUnit);
                }
                // Volume Addition
                else if (choice == 13)
                {
                    QuantityDTO first = ReadQuantity("First", "Volume", "Litre/Millilitre/Gallon");
                    QuantityDTO second = ReadQuantity("Second", "Volume", "Litre/Millilitre/Gallon");
                    Console.Write("Enter Target Unit (Litre/Millilitre/Gallon): ");
                    string targetUnit = Console.ReadLine();
                    controller.PerformVolumeAddition(first, second, targetUnit);
                }
                // Volume Subtraction
                else if (choice == 14)
                {
                    QuantityDTO first = ReadQuantity("First", "Volume", "Litre/Millilitre/Gallon");
                    QuantityDTO second = ReadQuantity("Second", "Volume", "Litre/Millilitre/Gallon");
                    Console.Write("Enter Target Unit (Litre/Millilitre/Gallon): ");
                    string targetUnit = Console.ReadLine();
                    controller.PerformVolumeSubtraction(first, second, targetUnit);
                }
                // Volume Division
                else if (choice == 15)
                {
                    QuantityDTO first = ReadQuantity("First", "Volume", "Litre/Millilitre/Gallon");
                    QuantityDTO second = ReadQuantity("Second", "Volume", "Litre/Millilitre/Gallon");
                    controller.PerformVolumeDivision(first, second);
                }
                // Temperature Comparison
                else if (choice == 16)
                {
                    QuantityDTO first = ReadQuantity("First", "Temperature", "Celsius/Fahrenheit/Kelvin");
                    QuantityDTO second = ReadQuantity("Second", "Temperature", "Celsius/Fahrenheit/Kelvin");
                    controller.PerformTemperatureComparison(first, second);
                }
                // Temperature Conversion
                else if (choice == 17)
                {
                    QuantityDTO quantity = ReadQuantity("", "Temperature", "Celsius/Fahrenheit/Kelvin");
                    Console.Write("Enter Target Unit (Celsius/Fahrenheit/Kelvin): ");
                    string targetUnit = Console.ReadLine();
                    controller.PerformTemperatureConversion(quantity, targetUnit);
                }
                // Temperature Arithmetic
                else if (choice == 18)
                {
                    controller.PerformTemperatureArithmetic(
                        new QuantityDTO(100, "Celsius", "Temperature"),
                        new QuantityDTO(50, "Celsius", "Temperature"),
                        "Celsius");
                }
                // Invalid Choice
                else
                {
                    Console.WriteLine("Invalid Choice");
                }
            }
        }

        // Method to read input
        private QuantityDTO ReadQuantity(string label, string measurementType, string unitHint)
        {
            string prefix = "";
            if (label != "")
            {
                prefix = label + " ";
            }

            Console.Write("Enter " + prefix + "Value: ");
            double value = double.Parse(Console.ReadLine());

            Console.Write("Enter " + prefix + "Unit (" + unitHint + "): ");
            string unit = Console.ReadLine();

            return new QuantityDTO(value, unit, measurementType);
        }
    }
}