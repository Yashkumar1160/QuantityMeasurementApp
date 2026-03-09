using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class SubtractionDivisionTests
    {

        //-----------SUBTRACTION TestS-----------

        //Test subtraction of feet minus feet
        [TestMethod]
        public void TestSubtraction_SameUnit_FeetMinusFeet()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(5.0, result.GetValue());
        }


        //Test subtraction of litre minus litre
        [TestMethod]
        public void TestSubtraction_SameUnit_LitreMinusLitre()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(3.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(7.0, result.GetValue());
        }


        //Test subtraction of feet minus inches
        [TestMethod]
        public void TestSubtraction_CrossUnit_FeetMinusInches()
        {
            Quantity<IMeasurable> feet = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> inch = new Quantity<IMeasurable>(6.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> result = feet.Subtract(inch);

            Assert.AreEqual(9.5, result.GetValue());
        }


        //Test subtraction of inches minus feet
        [TestMethod]
        public void TestSubtraction_CrossUnit_InchesMinusFeet()
        {
            Quantity<IMeasurable> inch = new Quantity<IMeasurable>(120.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> feet = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = inch.Subtract(feet);

            Assert.AreEqual(60.0, result.GetValue());
        }


        //Test subtraction with explicit target unit feet
        [TestMethod]
        public void TestSubtraction_ExplicitTargetUnit_Feet()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(6.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> result = q1.Subtract(q2, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.AreEqual(9.5, result.GetValue());
        }


        //Test subtraction with explicit target unit inches
        [TestMethod]
        public void TestSubtraction_ExplicitTargetUnit_Inches()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(6.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> result = q1.Subtract(q2, new LengthMeasurementImpl(LengthUnit.Inch));

            Assert.AreEqual(114.0, result.GetValue());
        }


        //Test subtraction with explicit target unit millilitre
        [TestMethod]
        public void TestSubtraction_ExplicitTargetUnit_Millilitre()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(5.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(2.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> result = q1.Subtract(q2, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Assert.AreEqual(3000.0, result.GetValue());
        }


        //Test subtraction resulting in negative value
        [TestMethod]
        public void TestSubtraction_ResultingInNegative()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(-5.0, result.GetValue());
        }


        //Test subtraction resulting in zero
        [TestMethod]
        public void TestSubtraction_ResultingInZero()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(120.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(0.0, result.GetValue());
        }


        //Test subtraction with zero operand
        [TestMethod]
        public void TestSubtraction_WithZeroOperand()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(0.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(5.0, result.GetValue());
        }


        //Test subtraction with negative values
        [TestMethod]
        public void TestSubtraction_WithNegativeValues()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(-2.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(7.0, result.GetValue());
        }


        //Test subtraction non commutative property
        [TestMethod]
        public void TestSubtraction_NonCommutative()
        {
            Quantity<IMeasurable> a = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> b = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result1 = a.Subtract(b);

            Quantity<IMeasurable> result2 = b.Subtract(a);

            Assert.AreEqual(5.0, result1.GetValue());

            Assert.AreEqual(-5.0, result2.GetValue());
        }


        //Test subtraction with large values
        [TestMethod]
        public void TestSubtraction_WithLargeValues()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(1000000.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(500000.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(500000.0, result.GetValue());
        }


        //Test subtraction with small values
        [TestMethod]
        public void TestSubtraction_WithSmallValues()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(0.001, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(0.0005, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(0, result.GetValue());
        }


        //Test subtraction null operand
        [TestMethod]
        public void TestSubtraction_NullOperand()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.Throws<ArgumentException>(() => q1.Subtract(null));
        }


        //Test subtraction null target unit
        [TestMethod]
        public void TestSubtraction_NullTargetUnit()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.Throws<ArgumentException>(() => q1.Subtract(q2, null));
        }


        //-----------DIVISION TestS-----------

        //Test division of feet divided by feet
        [TestMethod]
        public void TestDivision_SameUnit_FeetDividedByFeet()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(2.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double result = q1.Divide(q2);

            Assert.AreEqual(5.0, result);
        }

        //Test division of litre divided by litre
        [TestMethod]
        public void TestDivision_SameUnit_LitreDividedByLitre()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            double result = q1.Divide(q2);

            Assert.AreEqual(2.0, result);
        }


        //Test division of inches divided by feet
        [TestMethod]
        public void TestDivision_CrossUnit_FeetDividedByInches()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(24.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(2.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double result = q1.Divide(q2);

            Assert.AreEqual(1.0, result);
        }


        //Test division of kilogram divided by gram
        [TestMethod]
        public void TestDivision_CrossUnit_KilogramDividedByGram()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(2.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(2000.0, new WeightMeasurementImpl(WeightUnit.Gram));

            double result = q1.Divide(q2);

            Assert.AreEqual(1.0, result);
        }


        //Test division ratio greater than one
        [TestMethod]
        public void TestDivision_RatioGreaterThanOne()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(2.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double result = q1.Divide(q2);

            Assert.AreEqual(5.0, result);
        }


        //Test division ratio less than one
        [TestMethod]
        public void TestDivision_RatioLessThanOne()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double result = q1.Divide(q2);

            Assert.AreEqual(0.5, result);
        }


        //Test division ratio equal to one
        [TestMethod]
        public void TestDivision_RatioEqualToOne()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double result = q1.Divide(q2);

            Assert.AreEqual(1.0, result);
        }


        //Test division non commutative property
        [TestMethod]
        public void TestDivision_NonCommutative()
        {
            Quantity<IMeasurable> a = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> b = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double result1 = a.Divide(b);

            double result2 = b.Divide(a);

            Assert.AreEqual(2.0, result1);

            Assert.AreEqual(0.5, result2);
        }


        //Test division by zero
        [TestMethod]
        public void TestDivision_ByZero()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(0.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.Throws<DivideByZeroException>(() => q1.Divide(q2));
        }


        //Test division with large ratio
        [TestMethod]
        public void TestDivision_WithLargeRatio()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(1000000.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            double result = q1.Divide(q2);

            Assert.AreEqual(1000000.0, result);
        }


        //Test division with small ratio
        [TestMethod]
        public void TestDivision_WithSmallRatio()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(1000000.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            double result = q1.Divide(q2);

            Assert.AreEqual(0.000001, result);
        }


        //Test division null operand
        [TestMethod]
        public void TestDivision_NullOperand()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.Throws<ArgumentException>(() => q1.Divide(null));
        }

        //Test division cross category
        [TestMethod]
        public void TestDivision_CrossCategory()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.Throws<ArgumentException>(() => q1.Divide(q2));
        }


        //Test division works for all measurement categories
        [TestMethod]
        public void TestDivision_AllMeasurementCategories()
        {
            Quantity<IMeasurable> length1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));
            Quantity<IMeasurable> length2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> weight1 = new Quantity<IMeasurable>(10.0, new WeightMeasurementImpl(WeightUnit.Kilogram));
            Quantity<IMeasurable> weight2 = new Quantity<IMeasurable>(5.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> volume1 = new Quantity<IMeasurable>(10.0, new VolumeMeasurementImpl(VolumeUnit.Litre));
            Quantity<IMeasurable> volume2 = new Quantity<IMeasurable>(5.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.AreEqual(2.0, length1.Divide(length2));
            Assert.AreEqual(2.0, weight1.Divide(weight2));
            Assert.AreEqual(2.0, volume1.Divide(volume2));
        }


        //Test division associativity property
        [TestMethod]
        public void TestDivision_Associativity()
        {
            Quantity<IMeasurable> A = new Quantity<IMeasurable>(20.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> B = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> C = new Quantity<IMeasurable>(2.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double left = (A.Divide(B)) / C.GetValue();

            double right = A.Divide(new Quantity<IMeasurable>(B.Divide(C), new LengthMeasurementImpl(LengthUnit.Feet)));

            Assert.AreNotEqual(left, right);
        }


        //Test subtraction and division integration
        [TestMethod]
        public void TestSubtractionAndDivision_Integration()
        {
            Quantity<IMeasurable> A = new Quantity<IMeasurable>(20.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> B = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> C = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = A.Subtract(B);

            double finalResult = result.Divide(C);

            Assert.AreEqual(3.0, finalResult);
        }


        //Test subtraction and addition inverse relationship
        [TestMethod]
        public void TestSubtractionAddition_Inverse()
        {
            Quantity<IMeasurable> A = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> B = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> added = A.Add(B);

            Quantity<IMeasurable> result = added.Subtract(B);

            Assert.AreEqual(A.GetValue(), result.GetValue());
        }


        //Test subtraction immutability
        [TestMethod]
        public void TestSubtraction_Immutability()
        {
            Quantity<IMeasurable> original = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> other = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = original.Subtract(other);

            Assert.AreEqual(10.0, original.GetValue());

            Assert.AreEqual(5.0, result.GetValue());
        }


        //Test division immutability
        [TestMethod]
        public void TestDivision_Immutability()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double result = q1.Divide(q2);

            Assert.AreEqual(10.0, q1.GetValue());

            Assert.AreEqual(2.0, result);
        }


        //Test subtraction precision and rounding
        [TestMethod]
        public void TestSubtraction_PrecisionAndRounding()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(1.005, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(1.000, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(0, result.GetValue());
        }


        //Test division precision handling
        [TestMethod]
        public void TestDivision_PrecisionHandling()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(3.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            double result = q1.Divide(q2);

            Assert.AreEqual(0.3333333333333333, result);
        }


        //Test subtraction cross category
        [TestMethod]
        public void TestSubtraction_CrossCategory()
        {
            Quantity<IMeasurable> length =
                new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> weight =
                new Quantity<IMeasurable>(5.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.Throws<ArgumentException>(() => length.Subtract(weight));
        }


        //Test subtraction works for all measurement categories
        [TestMethod]
        public void TestSubtraction_AllMeasurementCategories()
        {
            Quantity<IMeasurable> length1 =
                new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> length2 =
                new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> weight1 =
                new Quantity<IMeasurable>(10.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> weight2 =
                new Quantity<IMeasurable>(5.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> volume1 =
                new Quantity<IMeasurable>(10.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> volume2 =
                new Quantity<IMeasurable>(5.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.AreEqual(5.0, length1.Subtract(length2).GetValue());
            Assert.AreEqual(5.0, weight1.Subtract(weight2).GetValue());
            Assert.AreEqual(5.0, volume1.Subtract(volume2).GetValue());
        }

        //Test subtraction chained operations
        [TestMethod]
        public void TestSubtraction_ChainedOperations()
        {
            //Create initial quantity
            Quantity<IMeasurable> q =
                new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            //Perform chained subtraction operations
            Quantity<IMeasurable> result =q.Subtract(new Quantity<IMeasurable>(2.0, new LengthMeasurementImpl(LengthUnit.Feet))).Subtract(new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet)));

            //Verify result
            Assert.AreEqual(7.0, result.GetValue());
        }

    }
}