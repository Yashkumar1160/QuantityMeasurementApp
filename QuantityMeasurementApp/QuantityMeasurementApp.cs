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
            Console.WriteLine("4. Add Two Lengths");
            Console.WriteLine("5. Check Weight Equality");
            Console.WriteLine("6. Convert Weight");
            Console.WriteLine("7. Compare Two Weights");
            Console.WriteLine("8. Add Two Weights");
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

                    DemonstrateLengthEquality(first, second);
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

                //add two lengths
                case 4:
                    //Take First Value
                    Console.Write("Enter First Value: ");
                    double addValue1 = double.Parse(Console.ReadLine());

                    //Take First Unit
                    Console.Write("Enter First Unit (Feet/Inch/Centimeter/Yard): ");
                    LengthUnit addUnit1 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

                    //Take Second Value
                    Console.Write("Enter Second Value: ");
                    double addValue2 = double.Parse(Console.ReadLine());

                    //Take Second Unit
                    Console.Write("Enter Second Unit (Feet/Inch/Centimeter/Yard): ");
                    LengthUnit addUnit2 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);

                    //Take Target Unit
                    Console.Write("Enter Target Unit (Feet/Inch/Centimeter/Yard): ");
                    LengthUnit targetUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine(), true);


                    //Create Instances
                    QuantityLength length1 = new QuantityLength(addValue1, addUnit1);
                    QuantityLength length2 = new QuantityLength(addValue2, addUnit2);

                    DemonstrateLengthAddition(length1, length2, targetUnit);
                    break;

                    
                //Uc-9 Weight Measurement Equality, Conversion, and Addition
                //Check weight equality
                case 5:
                    //Take First Value
                    Console.Write("Enter First Value: ");
                    double w1 = double.Parse(Console.ReadLine());

                    //Take First Unit
                    Console.Write("Enter First Unit (Kilogram/Gram/Pound): ");
                    WeightUnit wu1 = (WeightUnit)Enum.Parse(typeof(WeightUnit), Console.ReadLine(), true);

                    //Take Second Value
                    Console.Write("Enter Second Value: ");
                    double w2 = double.Parse(Console.ReadLine());

                    //Take Second Unit
                    Console.Write("Enter Second Unit (Kilogram/Gram/Pound): ");
                    WeightUnit wu2 = (WeightUnit)Enum.Parse(typeof(WeightUnit), Console.ReadLine(), true);

                    //Create Instances
                    QuantityWeight weight1 = new QuantityWeight(w1, wu1);
                    QuantityWeight weight2 = new QuantityWeight(w2, wu2);

                    DemonstrateWeightEquality(weight1, weight2);
                    break;

                //Convert Weight 
                case 6:
                    //Take Value
                    Console.Write("Enter Value: ");
                    double valueW = double.Parse(Console.ReadLine());

                    //Take From Unit
                    Console.Write("From Unit (Kilogram/Gram/Pound): ");
                    WeightUnit fromW = (WeightUnit)Enum.Parse(typeof(WeightUnit), Console.ReadLine(), true);

                    //Take To Unit
                    Console.Write("To Unit (Kilogram/Gram/Pound): ");
                    WeightUnit toW = (WeightUnit)Enum.Parse(typeof(WeightUnit), Console.ReadLine(), true);

                    DemonstrateWeightConversion(valueW, fromW, toW);
                    break;

                //Compare Two Weight Units
                case 7:
                    //Take First Value
                    Console.Write("Enter First Value: ");
                    double compareWeight1 = double.Parse(Console.ReadLine());

                    //Take First Unit
                    Console.Write("Enter First Unit (Kilogram/Gram/Pound): ");
                    WeightUnit compareWeightUnit1 = (WeightUnit)Enum.Parse(typeof(WeightUnit), Console.ReadLine(), true);

                    //Take Second Value
                    Console.Write("Enter Second Value: ");
                    double compareWeight2 = double.Parse(Console.ReadLine());
                    //Take Second Unit
                    Console.Write("Enter Second Unit (Kilogram/Gram/Pound): ");
                    WeightUnit compareWeightUnit2 = (WeightUnit)Enum.Parse(typeof(WeightUnit), Console.ReadLine(), true);

                    DemonstrateWeightComparison(compareWeight1, compareWeightUnit1, compareWeight2, compareWeightUnit2);
                    break;

                //Add Weights
                case 8:
                    //Take First Value
                    Console.Write("Enter First Value: ");
                    double aw1 = double.Parse(Console.ReadLine());

                    //Take First Unit
                    Console.Write("Enter First Unit (Kilogram/Gram/Pound): ");
                    WeightUnit au1 = (WeightUnit)Enum.Parse(typeof(WeightUnit), Console.ReadLine(), true);

                    //Take Second Value
                    Console.Write("Enter Second Value: ");
                    double aw2 = double.Parse(Console.ReadLine());

                    //Take Second Unit
                    Console.Write("Enter Second Unit (Kilogram/Gram/Pound): ");
                    WeightUnit au2 = (WeightUnit)Enum.Parse(typeof(WeightUnit), Console.ReadLine(), true);

                    //Take Target Unit
                    Console.Write("Enter Target Unit (Kilogram/Gram/Pound): ");
                    WeightUnit targetW = (WeightUnit)Enum.Parse(typeof(WeightUnit), Console.ReadLine(), true);

                    //Create Instances
                    QuantityWeight qw1 = new QuantityWeight(aw1, au1);
                    QuantityWeight qw2 = new QuantityWeight(aw2, au2);

                    DemonstrateWeightAddition(qw1, qw2, targetW);
                    break;


                //Invalid Choice
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

        //Method To Demonstrate Lengths Addition
        public static void DemonstrateLengthAddition(QuantityLength first, QuantityLength second, LengthUnit targetUnit)
        {
            //UC - 7 Add Method
            QuantityLength sum = first.Add(second, targetUnit);

            //display results
            Console.WriteLine($"{first} + {second} = {sum}");
        }



        //UC-9 Weight Measurement Equality, Conversion, and Addition
        //Method to Demonstrate Weights Equality
        public static void DemonstrateWeightEquality(QuantityWeight first, QuantityWeight second)
        {
            //Compare both
            Console.WriteLine(first.Equals(second)
                ? "Weights Are Equal"
                : "Weights Are Not Equal");
        }

        //Method to Demonstrate Weights Addition
        public static void DemonstrateWeightAddition(QuantityWeight first, QuantityWeight second, WeightUnit target)
        {
            //Store sum
            QuantityWeight sum = first.Add(second, target);

            //Display Results
            Console.WriteLine($"{first} + {second} = {sum}");
        }

        //Method to Demonstrate Weight Comparison
        public static void DemonstrateWeightComparison(double value1, WeightUnit unit1,
                                                       double value2, WeightUnit unit2)
        {

            QuantityWeight first = new QuantityWeight(value1, unit1);
            QuantityWeight second = new QuantityWeight(value2, unit2);

            DemonstrateWeightEquality(first, second);
        }

        //Method to Demonstrate Weight Conversion
        public static void DemonstrateWeightConversion(QuantityWeight weight, WeightUnit toUnit)
        {
            QuantityWeight converted = weight.ConvertTo(toUnit);
            Console.WriteLine($"{weight} = {converted}");
        }

        //Overload DemonstrateWeightConversion method
        public static void DemonstrateWeightConversion(double valueW, WeightUnit fromW, WeightUnit toW)
        {
            //Store Converted Weight
            double resultW = QuantityWeight.Convert(valueW, fromW, toW);

            //Display Result
            Console.WriteLine($"{valueW} {fromW} = {resultW} {toW}");
        }
    }
}