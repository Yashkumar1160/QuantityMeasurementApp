using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityLengthTests
    {
        //Epsilon value for floating point comparison
        private const double epsilon = 0.0001;

        //Test same unit (Feet) with same value
        [TestMethod]
        public void TestEquality_FeetToFeet_SameValue_ReturnsTrue()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength second = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsTrue(first.Equals(second));
        }

        //Test same unit (Inch) with same value
        [TestMethod]
        public void TestEquality_InchToInch_SameValue_ReturnsTrue()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Inch);
            QuantityLength second = new QuantityLength(1.0, LengthUnit.Inch);

            Assert.IsTrue(first.Equals(second));
        }

        //Test different values in same unit (feet)
        [TestMethod]
        public void TestEquality_FeetToFeet_DifferentValue_ReturnsFalse()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength second = new QuantityLength(2.0, LengthUnit.Feet);

            Assert.IsFalse(first.Equals(second));
        }

        //Test different values in same unit (inch)
        [TestMethod]
        public void TestEquality_InchToInch_DifferentValue_ReturnsFalse()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Inch);
            QuantityLength second = new QuantityLength(2.0, LengthUnit.Inch);

            Assert.IsFalse(first.Equals(second));
        }

        //Test equality feet to inch (1 feet=12 inches )
        [TestMethod]
        public void TestEquality_FeetToInch_EquivalentValue_ReturnsTrue()
        {
            QuantityLength feet = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength inches = new QuantityLength(12.0, LengthUnit.Inch);

            Assert.IsTrue(feet.Equals(inches));
        }

        //Test equality feet to inch (1 feet=12 inches )
        [TestMethod]
        public void TestEquality_InchToFeet_EquivalentValue_ReturnsTrue()
        {
            QuantityLength inches = new QuantityLength(12.0, LengthUnit.Inch);
            QuantityLength feet = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsTrue(inches.Equals(feet));
        }

        //Test equality feet to inch (1 feet=12 inches )
        [TestMethod]
        public void TestEquality_NullComparison_ReturnsFalse()
        {
            QuantityLength first = new QuantityLength(12.0, LengthUnit.Feet);
            QuantityLength second = null;

            bool result = first.Equals(second);

            Assert.IsFalse(result);
        }

        //Test comparison with different object type
        [TestMethod]
        public void TestEquality_ComparedWithDifferentType_ReturnsFalse()
        {
            QuantityLength quantity = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsFalse(quantity.Equals("Invalid"));
        }

        //Test Invalid Unit
        [TestMethod]
        public void TestEquality_InvalidUnit_ThrowsException()
        {
            Assert.Throws<Exception>(() =>
            {
                var invalid = new QuantityLength(5.0, (LengthUnit)10);
            });
        }

        //Test Same reference
        [TestMethod]
        public void TestEquality_SameReference_ReturnsTrue()
        {
            QuantityLength quantity = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsTrue(quantity.Equals(quantity));
        }


        //Test same unit (Yard) with same value
        [TestMethod]
        public void TestEquality_YardToYard_SameValue()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength second = new QuantityLength(1.0, LengthUnit.Yard);

            Assert.IsTrue(first.Equals(second));
        }

        //Test same unit (Yard) with different value
        [TestMethod]
        public void TestEquality_YardToYard_DifferentValue()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength second = new QuantityLength(2.0, LengthUnit.Yard);

            Assert.IsFalse(first.Equals(second));
        }

        //Test Equality Yard To Feet 
        [TestMethod]
        public void TestEquality_YardToFeet_EquivalentValue()
        {
            QuantityLength yard = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength feet = new QuantityLength(3.0, LengthUnit.Feet);

            Assert.IsTrue(yard.Equals(feet));
        }

        //Test Equality Feet To Yard
        [TestMethod]
        public void TestEquality_FeetToYard_EquivalentValue()
        {
            QuantityLength feet = new QuantityLength(3.0, LengthUnit.Feet);
            QuantityLength yard = new QuantityLength(1.0, LengthUnit.Yard);

            Assert.IsTrue(feet.Equals(yard));
        }

        //Test Equality Yard To Inches
        [TestMethod]
        public void TestEquality_YardToInches_EquivalentValue()
        {
            QuantityLength yard = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength inches = new QuantityLength(36.0, LengthUnit.Inch);

            Assert.IsTrue(yard.Equals(inches));
        }

        //Test Equality Inches To Yard
        [TestMethod]
        public void TestEquality_InchesToYard_EquivalentValue()
        {
            QuantityLength inches = new QuantityLength(36.0, LengthUnit.Inch);
            QuantityLength yard = new QuantityLength(1.0, LengthUnit.Yard);

            Assert.IsTrue(inches.Equals(yard));
        }

        //Test Non-equivalent conversion (should return false)
        [TestMethod]
        public void TestEquality_YardToFeet_NonEquivalentValue()
        {
            QuantityLength yard = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength feet = new QuantityLength(2.0, LengthUnit.Feet);

            Assert.IsFalse(yard.Equals(feet));
        }


        //Test same unit (Centimeters) with same value
        [TestMethod]
        public void TestEquality_CentimetersToCentimeters_SameValue()
        {
            QuantityLength cm1 = new QuantityLength(2.0, LengthUnit.Centimeter);
            QuantityLength cm2 = new QuantityLength(2.0, LengthUnit.Centimeter);

            Assert.IsTrue(cm1.Equals(cm2));
        }

        //Test Equality Centimeters to Inches
        [TestMethod]
        public void TestEquality_CentimetersToInches_EquivalentValue()
        {
            QuantityLength cm = new QuantityLength(1.0, LengthUnit.Centimeter);
            QuantityLength inch = new QuantityLength(0.393701, LengthUnit.Inch);

            Assert.IsTrue(cm.Equals(inch));
        }

        //Non-equivalent centimeter to feet comparison
        [TestMethod]
        public void TestEquality_CentimetersToFeet_NonEquivalentValue()
        {
            QuantityLength cm = new QuantityLength(1.0, LengthUnit.Centimeter);
            QuantityLength feet = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsFalse(cm.Equals(feet));
        }


        //Test Transitive Property
        [TestMethod]
        public void TestEquality_MultiUnit_TransitiveProperty()
        {
            QuantityLength yard = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength feet = new QuantityLength(3.0, LengthUnit.Feet);
            QuantityLength inches = new QuantityLength(36.0, LengthUnit.Inch);

            Assert.IsTrue(yard.Equals(feet));
            Assert.IsTrue(feet.Equals(inches));
            Assert.IsTrue(yard.Equals(inches));
        }

        //Test Same Reference
        [TestMethod]
        public void TestEquality_YardSameReference()
        {
            QuantityLength yard = new QuantityLength(2.0, LengthUnit.Yard);

            Assert.IsTrue(yard.Equals(yard));
        }

        //Test Null Comparison
        [TestMethod]
        public void TestEquality_YardNullComparison()
        {
            QuantityLength yard = new QuantityLength(2.0, LengthUnit.Yard);

            Assert.IsFalse(yard.Equals(null));
        }

        //Test All Units 
        [TestMethod]
        public void TestEquality_AllUnits_ComplexScenario()
        {
            QuantityLength yard = new QuantityLength(2.0, LengthUnit.Yard);
            QuantityLength feet = new QuantityLength(6.0, LengthUnit.Feet);
            QuantityLength inches = new QuantityLength(72.0, LengthUnit.Inch);

            Assert.IsTrue(yard.Equals(feet));
            Assert.IsTrue(feet.Equals(inches));
            Assert.IsTrue(yard.Equals(inches));
        }


        //Test Conversion Feet To Inches
        [TestMethod]
        public void testConversion_FeetToInches()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.Feet, LengthUnit.Inch);
            Assert.AreEqual(12.0, result, epsilon);
        }

        //Test Conversion Inches To Feet
        [TestMethod]
        public void testConversion_InchesToFeet()
        {
            double result = QuantityLength.Convert(24.0, LengthUnit.Inch, LengthUnit.Feet);
            Assert.AreEqual(2.0, result, epsilon);
        }
        //Test Conversion Yards To Inches
        [TestMethod]
        public void testConversion_YardsToInches()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.Yard, LengthUnit.Inch);
            Assert.AreEqual(36.0, result, epsilon);
        }

        //Test Conversion Inches To Yards
        [TestMethod]
        public void testConversion_InchesToYards()
        {
            double result = QuantityLength.Convert(72.0, LengthUnit.Inch, LengthUnit.Yard);
            Assert.AreEqual(2.0, result, epsilon);
        }

        //Test Conversion Centimeters To Inches
        [TestMethod]
        public void testConversion_CentimetersToInches()
        {
            double result = QuantityLength.Convert(2.54, LengthUnit.Centimeter, LengthUnit.Inch);
            Assert.AreEqual(1.0, result, epsilon);
        }

        //Test Conversion Feet To Yard
        [TestMethod]
        public void testConversion_FeetToYard()
        {
            double result = QuantityLength.Convert(6.0, LengthUnit.Feet, LengthUnit.Yard);
            Assert.AreEqual(2.0, result, epsilon);
        }

        //Test Round Trip Conversion
        [TestMethod]
        public void testConversion_RoundTrip_PreservesValue()
        {
            double original = 5.5;

            double toInch = QuantityLength.Convert(original, LengthUnit.Feet, LengthUnit.Inch);
            double backToFeet = QuantityLength.Convert(toInch, LengthUnit.Inch, LengthUnit.Feet);

            Assert.AreEqual(original, backToFeet, epsilon);
        }

        //Test Zero Value Conversion
        [TestMethod]
        public void testConversion_ZeroValue()
        {
            double result = QuantityLength.Convert(0.0, LengthUnit.Feet, LengthUnit.Inch);
            Assert.AreEqual(0.0, result, epsilon);
        }

        //Test Negative Value Conversion
        [TestMethod]
        public void testConversion_NegativeValue()
        {
            double result = QuantityLength.Convert(-1.0, LengthUnit.Feet, LengthUnit.Inch);
            Assert.AreEqual(-12.0, result, epsilon);
        }

        //Test Invalid Unit Throws Exception
        [TestMethod]
        public void testConversion_InvalidUnit_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                QuantityLength.Convert(5.0, (LengthUnit)100, LengthUnit.Feet));
        }

        //Test NaN Or Infinite Value Throws Exception
        [TestMethod]
        public void testConversion_NaNOrInfinite_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                QuantityLength.Convert(double.NaN, LengthUnit.Feet, LengthUnit.Inch));

            Assert.Throws<ArgumentException>(() =>
                QuantityLength.Convert(double.PositiveInfinity, LengthUnit.Feet, LengthUnit.Inch));

            Assert.Throws<ArgumentException>(() =>
                QuantityLength.Convert(double.NegativeInfinity, LengthUnit.Feet, LengthUnit.Inch));
        }

        //Test Precision Tolerance
        [TestMethod]
        public void testConversion_PrecisionTolerance()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.Centimeter, LengthUnit.Inch);
            double expected = 0.393700787;

            Assert.AreEqual(expected, result, epsilon);
        }
    }
}