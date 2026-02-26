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


        //-----------EQUALITY TEST CASES-------------
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


        //-----------CONVERSION TEST CASES-------------
        //Test Conversion Feet To Inches
        [TestMethod]
        public void TestConversion_FeetToInches()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.Feet, LengthUnit.Inch);
            Assert.AreEqual(12.0, result, epsilon);
        }

        //Test Conversion Inches To Feet
        [TestMethod]
        public void TestConversion_InchesToFeet()
        {
            double result = QuantityLength.Convert(24.0, LengthUnit.Inch, LengthUnit.Feet);
            Assert.AreEqual(2.0, result, epsilon);
        }
        //Test Conversion Yards To Inches
        [TestMethod]
        public void TestConversion_YardsToInches()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.Yard, LengthUnit.Inch);
            Assert.AreEqual(36.0, result, epsilon);
        }

        //Test Conversion Inches To Yards
        [TestMethod]
        public void TestConversion_InchesToYards()
        {
            double result = QuantityLength.Convert(72.0, LengthUnit.Inch, LengthUnit.Yard);
            Assert.AreEqual(2.0, result, epsilon);
        }

        //Test Conversion Centimeters To Inches
        [TestMethod]
        public void TestConversion_CentimetersToInches()
        {
            double result = QuantityLength.Convert(2.54, LengthUnit.Centimeter, LengthUnit.Inch);
            Assert.AreEqual(1.0, result, epsilon);
        }

        //Test Conversion Feet To Yard
        [TestMethod]
        public void TestConversion_FeetToYard()
        {
            double result = QuantityLength.Convert(6.0, LengthUnit.Feet, LengthUnit.Yard);
            Assert.AreEqual(2.0, result, epsilon);
        }

        //Test Round Trip Conversion
        [TestMethod]
        public void TestConversion_RoundTrip_PreservesValue()
        {
            double original = 5.5;

            double toInch = QuantityLength.Convert(original, LengthUnit.Feet, LengthUnit.Inch);
            double backToFeet = QuantityLength.Convert(toInch, LengthUnit.Inch, LengthUnit.Feet);

            Assert.AreEqual(original, backToFeet, epsilon);
        }

        //Test Zero Value Conversion
        [TestMethod]
        public void TestConversion_ZeroValue()
        {
            double result = QuantityLength.Convert(0.0, LengthUnit.Feet, LengthUnit.Inch);
            Assert.AreEqual(0.0, result, epsilon);
        }

        //Test Negative Value Conversion
        [TestMethod]
        public void TestConversion_NegativeValue()
        {
            double result = QuantityLength.Convert(-1.0, LengthUnit.Feet, LengthUnit.Inch);
            Assert.AreEqual(-12.0, result, epsilon);
        }

        //Test Invalid Unit Throws Exception
        [TestMethod]
        public void TestConversion_InvalidUnit_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                QuantityLength.Convert(5.0, (LengthUnit)100, LengthUnit.Feet));
        }

        //Test NaN Or Infinite Value Throws Exception
        [TestMethod]
        public void TestConversion_NaNOrInfinite_Throws()
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
        public void TestConversion_PrecisionTolerance()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.Centimeter, LengthUnit.Inch);
            double expected = 0.393700787;

            Assert.AreEqual(expected, result, epsilon);
        }


        //-----------ADDITION TEST CASES-------------
        //Test Same Unit Addition (Feet + Feet)
        [TestMethod]
        public void TestAddition_SameUnit_FeetPlusFeet()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(2.0, LengthUnit.Feet);

            QuantityLength result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new QuantityLength(3.0, LengthUnit.Feet)));
        }

        //Test Same Unit Addition (Inch + Inch)
        [TestMethod]
        public void TestAddition_SameUnit_InchPlusInch()
        {
            QuantityLength q1 = new QuantityLength(6.0, LengthUnit.Inch);
            QuantityLength q2 = new QuantityLength(6.0, LengthUnit.Inch);

            QuantityLength result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new QuantityLength(12.0, LengthUnit.Inch)));
        }

        //Test Cross Unit Addition (Feet + Inches)
        [TestMethod]
        public void TestAddition_CrossUnit_FeetPlusInches()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);

            QuantityLength result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new QuantityLength(2.0, LengthUnit.Feet)));
        }

        //Test Cross Unit Addition (Inch + Feet)
        [TestMethod]
        public void TestAddition_CrossUnit_InchPlusFeet()
        {
            QuantityLength q1 = new QuantityLength(12.0, LengthUnit.Inch);
            QuantityLength q2 = new QuantityLength(1.0, LengthUnit.Feet);

            QuantityLength result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new QuantityLength(24.0, LengthUnit.Inch)));
        }

        //Test Cross Unit Addition (Yard + Feet)
        [TestMethod]
        public void TestAddition_CrossUnit_YardPlusFeet()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength q2 = new QuantityLength(3.0, LengthUnit.Feet);

            QuantityLength result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new QuantityLength(2.0, LengthUnit.Yard)));
        }

        //Test Cross Unit Addition (Centimeter + Inch)
        [TestMethod]
        public void TestAddition_CrossUnit_CentimeterPlusInch()
        {
            QuantityLength q1 = new QuantityLength(2.54, LengthUnit.Centimeter);
            QuantityLength q2 = new QuantityLength(1.0, LengthUnit.Inch);

            QuantityLength result = q1.Add(q2);

            QuantityLength expected = new QuantityLength(5.08, LengthUnit.Centimeter);

            Assert.IsTrue(result.Equals(expected));
        }

        //Test Commutativity of Addition
        [TestMethod]
        public void TestAddition_Commutativity()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);

            QuantityLength result1 = q1.Add(q2);
            QuantityLength result2 = q2.Add(q1);

            Assert.IsTrue(result1.Equals(result2));
        }

        //Test Addition With Zero
        [TestMethod]
        public void TestAddition_WithZero()
        {
            QuantityLength q1 = new QuantityLength(5.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(0.0, LengthUnit.Inch);

            QuantityLength result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new QuantityLength(5.0, LengthUnit.Feet)));
        }

        //Test Addition With Negative Values
        [TestMethod]
        public void TestAddition_NegativeValues()
        {
            QuantityLength q1 = new QuantityLength(5.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(-2.0, LengthUnit.Feet);

            QuantityLength result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new QuantityLength(3.0, LengthUnit.Feet)));
        }

        //Test Null Second Operand
        [TestMethod]
        public void TestAddition_NullSecondOperand()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.Throws<ArgumentException>(() => q1.Add(null));
        }

        //Test Large Values Addition
        [TestMethod]
        public void TestAddition_LargeValues()
        {
            QuantityLength q1 = new QuantityLength(1e6, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(1e6, LengthUnit.Feet);

            QuantityLength result = q1.Add(q2);

            Assert.IsTrue(result.Equals(new QuantityLength(2e6, LengthUnit.Feet)));
        }

        //Test Small Values Addition
        [TestMethod]
        public void TestAddition_SmallValues()
        {
            QuantityLength q1 = new QuantityLength(0.001, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(0.002, LengthUnit.Feet);

            QuantityLength result = q1.Add(q2);

            QuantityLength expected = new QuantityLength(0.003, LengthUnit.Feet);

            Assert.IsTrue(result.Equals(expected));
        }


        //Test Explicit Target Unit Addition (Feet)
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_Feet()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);

            QuantityLength result = q1.Add(q2, LengthUnit.Feet);

            Assert.IsTrue(result.Equals(new QuantityLength(2.0, LengthUnit.Feet)));
        }

        //Test Explicit Target Unit Addition (Inches)
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_Inches()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);

            QuantityLength result = q1.Add(q2, LengthUnit.Inch);

            Assert.IsTrue(result.Equals(new QuantityLength(24.0, LengthUnit.Inch)));
        }

        //Test Explicit Target Unit Addition (Yards)
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_Yards()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);

            QuantityLength result = q1.Add(q2, LengthUnit.Yard);

            Assert.AreEqual(0.6667,
                QuantityLength.Convert(2.0, LengthUnit.Feet, LengthUnit.Yard),
                epsilon);
        }

        //Test Explicit Target Unit Addition (Centimeters)
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_Centimeters()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Inch);
            QuantityLength q2 = new QuantityLength(1.0, LengthUnit.Inch);

            QuantityLength result = q1.Add(q2, LengthUnit.Centimeter);

            Assert.AreEqual(5.08,
                QuantityLength.Convert(2.0, LengthUnit.Inch, LengthUnit.Centimeter),
                epsilon);
        }

        //Test Explicit Target Unit Same As First Operand
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_SameAsFirstOperand()
        {
            QuantityLength q1 = new QuantityLength(2.0, LengthUnit.Yard);
            QuantityLength q2 = new QuantityLength(3.0, LengthUnit.Feet);

            QuantityLength result = q1.Add(q2, LengthUnit.Yard);

            Assert.IsTrue(result.Equals(new QuantityLength(3.0, LengthUnit.Yard)));
        }

        //Test Explicit Target Unit Same As Second Operand
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_SameAsSecondOperand()
        {
            QuantityLength q1 = new QuantityLength(2.0, LengthUnit.Yard);
            QuantityLength q2 = new QuantityLength(3.0, LengthUnit.Feet);

            QuantityLength result = q1.Add(q2, LengthUnit.Feet);

            Assert.IsTrue(result.Equals(new QuantityLength(9.0, LengthUnit.Feet)));
        }

        //Test Commutativity With Explicit Target Unit
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_Commutativity()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);

            QuantityLength result1 = q1.Add(q2, LengthUnit.Yard);
            QuantityLength result2 = q2.Add(q1, LengthUnit.Yard);

            Assert.IsTrue(result1.Equals(result2));
        }

        //Test Addition With Zero (Explicit Target Unit)
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_WithZero()
        {
            QuantityLength q1 = new QuantityLength(5.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(0.0, LengthUnit.Inch);

            QuantityLength result = q1.Add(q2, LengthUnit.Yard);

            Assert.AreEqual(1.6667,
                QuantityLength.Convert(5.0, LengthUnit.Feet, LengthUnit.Yard),
                epsilon);
        }

        //Test Negative Values (Explicit Target Unit)
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_NegativeValues()
        {
            QuantityLength q1 = new QuantityLength(5.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(-2.0, LengthUnit.Feet);

            QuantityLength result = q1.Add(q2, LengthUnit.Inch);

            Assert.IsTrue(result.Equals(new QuantityLength(36.0, LengthUnit.Inch)));
        }

        //Test Null Target Unit Throws Exception
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_InvalidTargetUnit()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);

            Assert.Throws<ArgumentException>(() =>
                q1.Add(q2, (LengthUnit)100));
        }

        //Test Large To Small Scale Conversion
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_LargeToSmallScale()
        {
            QuantityLength q1 = new QuantityLength(1000.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(500.0, LengthUnit.Feet);

            QuantityLength result = q1.Add(q2, LengthUnit.Inch);

            Assert.IsTrue(result.Equals(new QuantityLength(18000.0, LengthUnit.Inch)));
        }

        //Test Small To Large Scale Conversion
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_SmallToLargeScale()
        {
            QuantityLength q1 = new QuantityLength(12.0, LengthUnit.Inch);
            QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);

            QuantityLength result = q1.Add(q2, LengthUnit.Yard);

            Assert.AreEqual(0.6667,
                QuantityLength.Convert(24.0, LengthUnit.Inch, LengthUnit.Yard),
                epsilon);
        }

        //Test All Unit Combinations
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_AllUnitCombinations()
        {
            foreach (LengthUnit u1 in Enum.GetValues(typeof(LengthUnit)))
            {
                foreach (LengthUnit u2 in Enum.GetValues(typeof(LengthUnit)))
                {
                    foreach (LengthUnit target in Enum.GetValues(typeof(LengthUnit)))
                    {
                        QuantityLength q1 = new QuantityLength(1.0, u1);
                        QuantityLength q2 = new QuantityLength(1.0, u2);

                        QuantityLength result = q1.Add(q2, target);

                        Assert.IsNotNull(result);
                    }
                }
            }
        }

        //Test Precision Tolerance With Explicit Target Unit
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_PrecisionTolerance()
        {
            QuantityLength q1 = new QuantityLength(2.54, LengthUnit.Centimeter);
            QuantityLength q2 = new QuantityLength(1.0, LengthUnit.Inch);

            QuantityLength result = q1.Add(q2, LengthUnit.Inch);

            double expected = 2.0;

            Assert.AreEqual(expected,
                QuantityLength.Convert(5.08, LengthUnit.Centimeter, LengthUnit.Inch),
                epsilon);
        }


        //UC-7 TEST CASES
        //Test LengthUnit.Feet is accessible and has correct conversion factor
        [TestMethod]
        public void TestLengthUnitEnum_FeetConstant()
        {
            Assert.AreEqual(1.0, ExtendedLengthUnit.GetConversionFactor(LengthUnit.Feet), epsilon);
        }

        //Test LengthUnit.Inch is accessible and has correct conversion factor
        [TestMethod]
        public void TestLengthUnitEnum_InchesConstant()
        {
            Assert.AreEqual(1.0 / 12.0, ExtendedLengthUnit.GetConversionFactor(LengthUnit.Inch), epsilon);
        }

        //Test LengthUnit.Yard is accessible and has correct conversion factor
        [TestMethod]
        public void TestLengthUnitEnum_YardsConstant()
        {
            Assert.AreEqual(3.0, ExtendedLengthUnit.GetConversionFactor(LengthUnit.Yard), epsilon);
        }

        //Test LengthUnit.Inch is accessible and has correct conversion factor
        [TestMethod]
        public void TestLengthUnitEnum_CentimetersConstant()
        {
            Assert.AreEqual(0.0328084, ExtendedLengthUnit.GetConversionFactor(LengthUnit.Centimeter), epsilon);
        }


        //Test conversion from Feet to base unit (Feet)
        [TestMethod]
        public void TestConvertToBaseUnit_FeetToFeet()
        {
            double result = ExtendedLengthUnit.ConvertToBase(5.0, LengthUnit.Feet);
            Assert.AreEqual(5.0, result, epsilon);
        }

        //Test conversion from Inches to base unit (Feet)
        [TestMethod]
        public void TestConvertToBaseUnit_InchesToFeet()
        {
            double result = ExtendedLengthUnit.ConvertToBase(12.0, LengthUnit.Inch);
            Assert.AreEqual(1.0, result, epsilon);
        }

        //Test conversion from Yards to base unit (Feet)
        [TestMethod]
        public void TestConvertToBaseUnit_YardsToFeet()
        {
            double result = ExtendedLengthUnit.ConvertToBase(1.0, LengthUnit.Yard);
            Assert.AreEqual(3.0, result, epsilon);
        }

        //Test conversion from Centimeters to base unit (Feet)
        [TestMethod]
        public void TestConvertToBaseUnit_CentimetersToFeet()
        {
            double result = ExtendedLengthUnit.ConvertToBase(30.48, LengthUnit.Centimeter);
            Assert.AreEqual(1.0, result, epsilon);
        }


        //Test conversion from base unit (Feet) to Feet
        [TestMethod]
        public void TestConvertFromBaseUnit_FeetToFeet()
        {
            double result = ExtendedLengthUnit.ConvertFromBase(2.0, LengthUnit.Feet);
            Assert.AreEqual(2.0, result, epsilon);
        }

        //Test conversion from base unit (Feet) to Inches
        [TestMethod]
        public void TestConvertFromBaseUnit_FeetToInches()
        {
            double result = ExtendedLengthUnit.ConvertFromBase(1.0, LengthUnit.Inch);
            Assert.AreEqual(12.0, result, epsilon);
        }

        //Test conversion from base unit (Feet) to Yards
        [TestMethod]
        public void TestConvertFromBaseUnit_FeetToYards()
        {
            double result = ExtendedLengthUnit.ConvertFromBase(3.0, LengthUnit.Yard);
            Assert.AreEqual(1.0, result, epsilon);
        }

        //Test conversion from base unit (Feet) to Centimeters
        [TestMethod]
        public void TestConvertFromBaseUnit_FeetToCentimeters()
        {
            double result = ExtendedLengthUnit.ConvertFromBase(1.0, LengthUnit.Centimeter);
            Assert.AreEqual(30.48, result, epsilon);
        }


        //Test equality between QuantityLength in Feet and Inches
        [TestMethod]
        public void TestQuantityLengthRefactored_Equality()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);
            Assert.IsTrue(q1.Equals(q2));
        }

        //Test conversion of QuantityLength from Feet to Inches
        [TestMethod]
        public void TestQuantityLengthRefactored_ConvertTo()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength result = q1.ConvertTo(LengthUnit.Inch);
            Assert.IsTrue(result.Equals(new QuantityLength(12.0, LengthUnit.Inch)));
        }

        //Test addition of QuantityLength with cross-unit conversion (Feet + Inches)
        [TestMethod]
        public void TestQuantityLengthRefactored_Add()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);
            QuantityLength result = q1.Add(q2, LengthUnit.Feet);
            Assert.IsTrue(result.Equals(new QuantityLength(2.0, LengthUnit.Feet)));
        }

        //Test addition with explicit target unit (Feet + Inches to Yard)
        [TestMethod]
        public void TestQuantityLengthRefactored_AddWithTargetUnit()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(12.0, LengthUnit.Inch);
            QuantityLength result = q1.Add(q2, LengthUnit.Yard);
            Assert.AreEqual(0.6667, ExtendedLengthUnit.ConvertFromBase(ExtendedLengthUnit.ConvertToBase(2.0, LengthUnit.Feet), LengthUnit.Yard), epsilon);
        }

        //Test null unit validation in QuantityLength constructor
        [TestMethod]
        public void TestQuantityLengthRefactored_NullUnit()
        {
            Assert.Throws<ArgumentException>(() => new QuantityLength(1.0, (LengthUnit)100));
        }

        //Test invalid value validation in QuantityLength constructor
        [TestMethod]
        public void TestQuantityLengthRefactored_InvalidValue()
        {
            Assert.Throws<ArgumentException>(() => new QuantityLength(double.NaN, LengthUnit.Feet));
        }

        //Test Round Trip Conversion
        [TestMethod]
        public void TestRoundTripConversion_RefactoredDesign()
        {
            double original = 5.5;
            double converted = ExtendedLengthUnit.ConvertFromBase(
                ExtendedLengthUnit.ConvertToBase(original, LengthUnit.Feet), LengthUnit.Feet);
            Assert.AreEqual(original, converted, epsilon);
        }

        //Test LengthUnit enum constants are immutable and thread-safe.
        [TestMethod]
        public void TestUnitImmutability()
        {
            double factor = ExtendedLengthUnit.GetConversionFactor(LengthUnit.Feet);
            Assert.AreEqual(1.0, factor, epsilon);
        }
    }
}