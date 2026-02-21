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
    }
}