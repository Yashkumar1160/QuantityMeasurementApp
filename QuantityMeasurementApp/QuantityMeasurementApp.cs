using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementApp
    {
        public static void Main(string[] args)
        {
            DemonstrateLengthEquality();
        }

        public static void DemonstrateLengthEquality()
        {
            // Take first value
            Console.WriteLine("Enter First Value: ");
            double firstValue = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter First Unit (Feet/Inch/Centimeter/Yard): ");
            LengthUnit firstUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

            // Take second value
            Console.WriteLine("Enter Second Value: ");
            double secondValue = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter Second Unit (Feet/Inch/Centimeter/Yard): ");
            LengthUnit secondUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

            // Create QuantityLength objects
            QuantityLength first = new QuantityLength(firstValue, firstUnit);
            QuantityLength second = new QuantityLength(secondValue, secondUnit);

            // Compare
            Console.WriteLine(first.Equals(second)
                ? "Both Values Are Equal"
                : "Both Values Are Different");
        }
    }
}