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
            Console.WriteLine("9. Check Volume Equality");
            Console.WriteLine("10. Convert Volume");
            Console.WriteLine("11. Compare Two Volumes");
            Console.WriteLine("12. Add Two Volumes");
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

                    Quantity<IMeasurable> first = new Quantity<IMeasurable>(firstValue, new LengthMeasurementImpl(firstUnit));

                    Quantity<IMeasurable> second = new Quantity<IMeasurable>(secondValue, new LengthMeasurementImpl(secondUnit));

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
                    Quantity<IMeasurable> length1 = new Quantity<IMeasurable>(addValue1, new LengthMeasurementImpl(addUnit1));
                    Quantity<IMeasurable> length2 = new Quantity<IMeasurable>(addValue2, new LengthMeasurementImpl(addUnit2));

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
                    Quantity<IMeasurable> weight1 = new Quantity<IMeasurable>(w1, new WeightMeasurementImpl(wu1));
                    Quantity<IMeasurable> weight2 = new Quantity<IMeasurable>(w2, new WeightMeasurementImpl(wu2));

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
                    Quantity<IMeasurable> qw1 = new Quantity<IMeasurable>(aw1, new WeightMeasurementImpl(au1));
                    Quantity<IMeasurable> qw2 = new Quantity<IMeasurable>(aw2, new WeightMeasurementImpl(au2));

                    DemonstrateWeightAddition(qw1, qw2, targetW);
                    break;


                //Check Volume Equality
                case 9:

                    Console.Write("Enter First Value: ");
                    double v1 = double.Parse(Console.ReadLine());

                    Console.Write("Enter First Unit (Litre/Millilitre/Gallon): ");
                    VolumeUnit vu1 = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), Console.ReadLine(), true);

                    Console.Write("Enter Second Value: ");
                    double v2 = double.Parse(Console.ReadLine());

                    Console.Write("Enter Second Unit (Litre/Millilitre/Gallon): ");
                    VolumeUnit vu2 = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), Console.ReadLine(), true);

                    Quantity<IMeasurable> volume1 =
                        new Quantity<IMeasurable>(v1, new VolumeMeasurementImpl(vu1));

                    Quantity<IMeasurable> volume2 =
                        new Quantity<IMeasurable>(v2, new VolumeMeasurementImpl(vu2));

                    DemonstrateVolumeEquality(volume1, volume2);

                    break;

                case 10:

                    Console.Write("Enter Value: ");
                    double valueV = double.Parse(Console.ReadLine());

                    Console.Write("From Unit (Litre/Millilitre/Gallon): ");
                    VolumeUnit fromV = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), Console.ReadLine(), true);

                    Console.Write("To Unit (Litre/Millilitre/Gallon): ");
                    VolumeUnit toV = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), Console.ReadLine(), true);

                    DemonstrateVolumeConversion(valueV, fromV, toV);

                    break;


                case 11:

                    Console.Write("Enter First Value: ");
                    double cv1 = double.Parse(Console.ReadLine());

                    Console.Write("Enter First Unit (Litre/Millilitre/Gallon): ");
                    VolumeUnit cu1 = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), Console.ReadLine(), true);

                    Console.Write("Enter Second Value: ");
                    double cv2 = double.Parse(Console.ReadLine());

                    Console.Write("Enter Second Unit (Litre/Millilitre/Gallon): ");
                    VolumeUnit cu2 = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), Console.ReadLine(), true);

                    DemonstrateVolumeComparison(cv1, cu1, cv2, cu2);

                    break;

                case 12:

                    Console.Write("Enter First Value: ");
                    double av1 = double.Parse(Console.ReadLine());

                    Console.Write("Enter First Unit (Litre/Millilitre/Gallon): ");
                    VolumeUnit auv1 = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), Console.ReadLine(), true);

                    Console.Write("Enter Second Value: ");
                    double av2 = double.Parse(Console.ReadLine());

                    Console.Write("Enter Second Unit (Litre/Millilitre/Gallon): ");
                    VolumeUnit auv2 = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), Console.ReadLine(), true);

                    Console.Write("Enter Target Unit (Litre/Millilitre/Gallon): ");
                    VolumeUnit targetV = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), Console.ReadLine(), true);

                    Quantity<IMeasurable> vol1 =
                        new Quantity<IMeasurable>(av1, new VolumeMeasurementImpl(auv1));

                    Quantity<IMeasurable> vol2 =
                        new Quantity<IMeasurable>(av2, new VolumeMeasurementImpl(auv2));

                    DemonstrateVolumeAddition(vol1, vol2, targetV);

                    break;

                //Invalid Choice
                default:
                    Console.WriteLine("Invalid Choice");
                    break;
            }
        }


        //Method to Demonstrate Length Equality
        public static void DemonstrateLengthEquality(Quantity<IMeasurable> first, Quantity<IMeasurable> second)
        {
            Console.WriteLine(first.Equals(second)
                 ? "Both Values Are Equal"
                 : "Both Values Are Different");
        }


        //Method to Convert Length Units
        public static void DemonstrateLengthConversion(double value, LengthUnit fromUnit, LengthUnit toUnit)
        {
            //Create Quantity Object
            Quantity<IMeasurable> length = new Quantity<IMeasurable>(value, new LengthMeasurementImpl(fromUnit));

            //Convert to Target Unit
            Quantity<IMeasurable> converted = length.ConvertTo(new LengthMeasurementImpl(toUnit));

            //Display Result
            Console.WriteLine($"{length} = {converted}");
        }

        //Overloaded DemonstrateLengthConversion method
        public static void DemonstrateLengthConversion(Quantity<IMeasurable> length, LengthUnit toUnit)
        {
            Quantity<IMeasurable> converted = length.ConvertTo(new LengthMeasurementImpl(toUnit));
            Console.WriteLine($"{length} = {converted}");
        }

        //Method To Compare Two Length Units
        public static void DemonstrateLengthComparison(double value1, LengthUnit unit1,
                                                       double value2, LengthUnit unit2)
        {
            //Create Quantity Objects
            Quantity<IMeasurable> first =
                new Quantity<IMeasurable>(value1, new LengthMeasurementImpl(unit1));

            Quantity<IMeasurable> second =
                new Quantity<IMeasurable>(value2, new LengthMeasurementImpl(unit2));

            DemonstrateLengthEquality(first, second);
        }

        //Method To Demonstrate Lengths Addition
        public static void DemonstrateLengthAddition(Quantity<IMeasurable> first,
                                                     Quantity<IMeasurable> second,
                                                     LengthUnit targetUnit)
        {
            //UC-10 Add Method
            Quantity<IMeasurable> sum =
                first.Add(second, new LengthMeasurementImpl(targetUnit));

            //Display Results
            Console.WriteLine($"{first} + {second} = {sum}");
        }



        //UC-9 Weight Measurement Equality, Conversion, and Addition
        //Method to Demonstrate Weights Equality
        public static void DemonstrateWeightEquality(Quantity<IMeasurable> first,
                                                     Quantity<IMeasurable> second)
        {
            Console.WriteLine(first.Equals(second)
                ? "Weights Are Equal"
                : "Weights Are Not Equal");
        }

        //Method to Demonstrate Weights Addition
        public static void DemonstrateWeightAddition(Quantity<IMeasurable> first,
                                                     Quantity<IMeasurable> second,
                                                     WeightUnit target)
        {
            Quantity<IMeasurable> sum = first.Add(second, new WeightMeasurementImpl(target));

            Console.WriteLine($"{first} + {second} = {sum}");
        }

        //Method to Demonstrate Weight Comparison
        public static void DemonstrateWeightComparison(double value1, WeightUnit unit1,
                                                       double value2, WeightUnit unit2)
        {
            Quantity<IMeasurable> first = new Quantity<IMeasurable>(value1, new WeightMeasurementImpl(unit1));

            Quantity<IMeasurable> second = new Quantity<IMeasurable>(value2, new WeightMeasurementImpl(unit2));

            DemonstrateWeightEquality(first, second);
        }

        //Method to Demonstrate Weight Conversion
        public static void DemonstrateWeightConversion(Quantity<IMeasurable> weight, WeightUnit toUnit)
        {
            Quantity<IMeasurable> converted = weight.ConvertTo(new WeightMeasurementImpl(toUnit));

            Console.WriteLine($"{weight} = {converted}");
        }

        //Overload DemonstrateWeightConversion method
        public static void DemonstrateWeightConversion(double valueW, WeightUnit fromW, WeightUnit toW)
        {
            Quantity<IMeasurable> weight = new Quantity<IMeasurable>(valueW, new WeightMeasurementImpl(fromW));

            Quantity<IMeasurable> converted = weight.ConvertTo(new WeightMeasurementImpl(toW));

            Console.WriteLine($"{weight} = {converted}");
        }


        //UC-11
        //Method to Demonstrate Volume Equality
        public static void DemonstrateVolumeEquality(Quantity<IMeasurable> first, Quantity<IMeasurable> second)
        {
            Console.WriteLine(first.Equals(second)
                ? "Volumes Are Equal"
                : "Volumes Are Not Equal");
        }

        //Method to Demonstrate Volume Addition
        public static void DemonstrateVolumeAddition(Quantity<IMeasurable> first, Quantity<IMeasurable> second, VolumeUnit target)
        {
            Quantity<IMeasurable> sum = first.Add(second, new VolumeMeasurementImpl(target));

            Console.WriteLine($"{first} + {second} = {sum}");
        }

        //Method to Demonstrate Volume Comparison
        public static void DemonstrateVolumeComparison(double value1, VolumeUnit unit1, double value2, VolumeUnit unit2)
        {
            Quantity<IMeasurable> first = new Quantity<IMeasurable>(value1, new VolumeMeasurementImpl(unit1));

            Quantity<IMeasurable> second = new Quantity<IMeasurable>(value2, new VolumeMeasurementImpl(unit2));

            DemonstrateVolumeEquality(first, second);
        }


        //Method to Demonstrate Volume Conversion
        public static void DemonstrateVolumeConversion(double value, VolumeUnit from, VolumeUnit to)
        {
            Quantity<IMeasurable> volume = new Quantity<IMeasurable>(value, new VolumeMeasurementImpl(from));

            Quantity<IMeasurable> converted = volume.ConvertTo(new VolumeMeasurementImpl(to));

            Console.WriteLine($"{volume} = {converted}");
        }
    }
}