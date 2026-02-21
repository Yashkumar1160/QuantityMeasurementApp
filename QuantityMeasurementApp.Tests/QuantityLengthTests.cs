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
    }
}