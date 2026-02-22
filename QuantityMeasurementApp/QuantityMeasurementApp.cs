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
            //display menu
            Console.WriteLine("===== Quantity Measurement App =====");
            Console.WriteLine("1. Check Length Equality");
            Console.WriteLine("2. Convert Length");
            Console.WriteLine("3. Compare Two Lengths");
            Console.Write("Enter Your Choice: ");

            //take user's choice
            int choice = int.Parse(Console.ReadLine());

            //handle user's choice
            switch (choice)
            {
                //demonstrate length equality 
                case 1:
                    Console.Write("Enter First Value: ");
                    double firstValue = double.Parse(Console.ReadLine());

                    Console.Write("Enter First Unit (Feet/Inch/Centimeter/Yard): ");
                    LengthUnit firstUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

                    Console.Write("Enter Second Value: ");
                    double secondValue = double.Parse(Console.ReadLine());

                    Console.Write("Enter Second Unit (Feet/Inch/Centimeter/Yard): ");
                    LengthUnit secondUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

                    QuantityLength first = new QuantityLength(firstValue, firstUnit);
                    QuantityLength second = new QuantityLength(secondValue, secondUnit);

                    DemonstrateLengthEquality(first,second);
                    break;


                //convert length
                case 2:
                    // Take value
                    Console.WriteLine("Enter Value: ");
                    double value = double.Parse(Console.ReadLine());

                    Console.WriteLine("Enter From Unit (Feet/Inch/Centimeter/Yard): ");
                    LengthUnit fromUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

                    Console.WriteLine("Enter To Unit (Feet/Inch/Centimeter/Yard): ");
                    LengthUnit toUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

                    DemonstrateLengthConversion(value, fromUnit, toUnit);
                    break;

                //Compare Units
                case 3:
                    //Take First Value
                    Console.Write("Enter First Value: ");
                    double value1 = double.Parse(Console.ReadLine());

                    //Take First Unit
                    Console.Write("Enter First Unit (Feet/Inch/Centimeter/Yard): ");
                    LengthUnit unit1 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

                    //Take Second Value
                    Console.Write("Enter Second Value: ");
                    double value2 = double.Parse(Console.ReadLine());
                    //Take Second Unit
                    Console.Write("Enter Second Unit (Feet/Inch/Centimeter/Yard): ");
                    LengthUnit unit2 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

                    DemonstrateLengthComparison(value1, unit1, value2, unit2);
                    break;

                default:
                    Console.WriteLine("Invalid Choice");
                    break;
            }
        }

        //Method to Demonstrate Length Equality
        public static void DemonstrateLengthEquality(QuantityLength first, QuantityLength second)
        {
            Console.WriteLine(first.Equals(second)
                 ? "Both Values Are Equal"
                 : "Both Values Are Different");
        }



        //Method to Convert Unit
        public static void DemonstrateLengthConversion(double value, LengthUnit fromUnit, LengthUnit toUnit)
        {
            double result = QuantityLength.Convert(value, fromUnit, toUnit);
            Console.WriteLine($"{value} {fromUnit} = {result} {toUnit}");
        }

        //Overloaded DemonstrateLengthConversion method
        public static void DemonstrateLengthConversion(QuantityLength length, LengthUnit toUnit)
        {
            QuantityLength converted = length.ConvertTo(toUnit);
            Console.WriteLine($"{length} = {converted}");
        }

        //Method To Compare Two Units
        public static void DemonstrateLengthComparison(double value1, LengthUnit unit1,
                                                       double value2, LengthUnit unit2)
        {
            QuantityLength first = new QuantityLength(value1, unit1);
            QuantityLength second = new QuantityLength(value2, unit2);

            DemonstrateLengthEquality(first, second);
        }
    }
}