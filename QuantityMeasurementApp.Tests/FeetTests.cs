    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using QuantityMeasurementApp;

    namespace QuantityMeasurementApp.Tests
    {
        [TestClass]
        public class FeetTests
        {
            //Test if two Feet objects with same values are equal 
            [TestMethod]
            public void TestFeetEquality_WithSameValues_ReturnsTrue()
            {   //Arrange
                Feet first = new Feet(1.0);
                Feet second = new Feet(1.0);

                //Act
                bool result = first.Equals(second);

                //Assert
                Assert.IsTrue(result);
            }

            //Test if two Feet objects with different values are not equal 
            [TestMethod]
            public void TestFeetEquality_WithDifferentValues_ReturnsFalse()
            {
                //Arrange
                Feet first = new Feet(1.0);
                Feet second = new Feet(2.0);

                //Act
                bool result = first.Equals(second);

                //Assert
                Assert.IsFalse(result);
            }


            //Test if comparing with null returns false
            [TestMethod]
            public void TestFeetEquality_WhenComparedWithNull_ReturnsFalse()
            {
                //Arrange
                Feet first = new Feet(1.0);

                //Act
                bool result = first.Equals(null);

                //Assert
                Assert.IsFalse(result);
            }

            //Test if comparing with a different data type returns false
            [TestMethod]
            public void TestFeetEquality_WhenComparedWithNonNumericInput_ReturnsFalse()
            {
                //Arrange
                Feet first = new Feet(1.0);

                //Act
                bool result = first.Equals("One");

                //Assert
                Assert.IsFalse(result);
            }

            //Test if comparing with same reference returns true
            [TestMethod]
            public void TestFeetEquality_WithSameReference_ReturnsTrue()
            {
                //Arrange
                Feet first = new Feet(1.0);

                //Act
                bool result = first.Equals(first);

                //Assert
                Assert.IsTrue(result);
            }
        }
    }