using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]

    public class InchesTests
    {
        //Test if two Inches objects with same values are equal 
        [TestMethod]
        public void TestInchesEquality_WithSameValues_ReturnsTrue()
        {   //Arrange
            Inches first = new Inches(1.0);
            Inches second = new Inches(1.0);

            //Act
            bool result = first.Equals(second);

            //Assert
            Assert.IsTrue(result);
        }

        //Test if two Inches objects with different values are not equal 
        [TestMethod]
        public void TestInchesEquality_WithDifferentValues_ReturnsFalse()
        {
            //Arrange
            Inches first = new Inches(1.0);
            Inches second = new Inches(2.0);

            //Act
            bool result = first.Equals(second);

            //Assert
            Assert.IsFalse(result);
        }


        //Test if comparing with null returns false
        [TestMethod]
        public void TestInchesEquality_WhenComparedWithNull_ReturnsFalse()
        {
            //Arrange
            Inches first = new Inches(1.0);

            //Act
            bool result = first.Equals(null);

            //Assert
            Assert.IsFalse(result);
        }

        //Test if comparing with a different data type returns false
        [TestMethod]
        public void TestInchesEquality_WhenComparedWithNonNumericInput_ReturnsFalse()
        {
            //Arrange
            Inches first = new Inches(1.0);

            //Act
            bool result = first.Equals("One");

            //Assert
            Assert.IsFalse(result);
        }

        //Test if comparing with same reference returns true
        [TestMethod]
        public void TestInchesEquality_WithSameReference_ReturnsTrue()
        {
            //Arrange
            Inches first = new Inches(1.0);

            //Act
            bool result = first.Equals(first);

            //Assert
            Assert.IsTrue(result);
        }
    }
}