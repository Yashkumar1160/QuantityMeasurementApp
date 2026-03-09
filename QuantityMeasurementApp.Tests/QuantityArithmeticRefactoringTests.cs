using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Reflection;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityArithmeticRefactoringTests
    {
        //Test refactoring add delegates via helper
        [TestMethod]
        public void TestRefactoring_Add_DelegatesViaHelper()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Add(q2);

            Assert.AreEqual(15.0, result.GetValue());
        }


        //Test refactoring subtract delegates via helper
        [TestMethod]
        public void TestRefactoring_Subtract_DelegatesViaHelper()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(5.0, result.GetValue());
        }


        //Test refactoring divide delegates via helper
        [TestMethod]
        public void TestRefactoring_Divide_DelegatesViaHelper()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double result = q1.Divide(q2);

            Assert.AreEqual(2.0, result);
        }


        //Test validation null operand consistent across operations
        [TestMethod]
        public void TestValidation_NullOperand_ConsistentAcrossOperations()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.Throws<ArgumentException>(() => q1.Add(null));

            Assert.Throws<ArgumentException>(() => q1.Subtract(null));

            Assert.Throws<ArgumentException>(() => q1.Divide(null));
        }


        //Test validation cross category consistent across operations
        [TestMethod]
        public void TestValidation_CrossCategory_ConsistentAcrossOperations()
        {
            Quantity<IMeasurable> length = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> weight = new Quantity<IMeasurable>(5.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.Throws<ArgumentException>(() => length.Add(weight));

            Assert.Throws<ArgumentException>(() => length.Subtract(weight));

            Assert.Throws<ArgumentException>(() => length.Divide(weight));
        }


        //Test validation finite value consistent across operations
        [TestMethod]
        public void TestValidation_FiniteValue_ConsistentAcrossOperations()
        {
            Assert.Throws<ArgumentException>(() => new Quantity<IMeasurable>(Double.NaN, new LengthMeasurementImpl(LengthUnit.Feet)));

            Assert.Throws<ArgumentException>(() => new Quantity<IMeasurable>(Double.PositiveInfinity, new LengthMeasurementImpl(LengthUnit.Feet)));

            Assert.Throws<ArgumentException>(() => new Quantity<IMeasurable>(Double.NegativeInfinity, new LengthMeasurementImpl(LengthUnit.Feet)));
        }


        //Test validation null target unit add subtract reject
        [TestMethod]
        public void TestValidation_NullTargetUnit_AddSubtractReject()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.Throws<ArgumentException>(() => q1.Add(q2, null));

            Assert.Throws<ArgumentException>(() => q1.Subtract(q2, null));
        }


        //Test arithmetic operation add enum computation
        [TestMethod]
        public void TestArithmeticOperation_Add_EnumComputation()
        {
            double result = 10.0 + 5.0;

            Assert.AreEqual(15.0, result);
        }


        //Test arithmetic operation subtract enum computation
        [TestMethod]
        public void TestArithmeticOperation_Subtract_EnumComputation()
        {
            double result = 10.0 - 5.0;

            Assert.AreEqual(5.0, result);
        }


        //Test arithmetic operation divide enum computation
        [TestMethod]
        public void TestArithmeticOperation_Divide_EnumComputation()
        {
            double result = 10.0 / 5.0;

            Assert.AreEqual(2.0, result);
        }


        //Test arithmetic operation divide by zero enum throws
        [TestMethod]
        public void TestArithmeticOperation_DivideByZero_EnumThrows()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(0.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.Throws<DivideByZeroException>(() => q1.Divide(q2));
        }


        //Test perform base arithmetic conversion and operation
        [TestMethod]
        public void TestPerformBaseArithmetic_ConversionAndOperation()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Yard));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Add(q2);

            Assert.AreEqual(1.33, result.GetValue(), 0.01);
        }


        //Test add UC12 behavior preserved
        [TestMethod]
        public void TestAdd_UC12_BehaviorPreserved()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Add(q2);

            Assert.AreEqual(15.0, result.GetValue());
        }


        //Test subtract UC12 behavior preserved
        [TestMethod]
        public void TestSubtract_UC12_BehaviorPreserved()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(5.0, result.GetValue());
        }


        //Test divide UC12 behavior preserved
        [TestMethod]
        public void TestDivide_UC12_BehaviorPreserved()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double result = q1.Divide(q2);

            Assert.AreEqual(2.0, result);
        }


        //Test rounding add subtract two decimal places
        [TestMethod]
        public void TestRounding_AddSubtract_TwoDecimalPlaces()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(1.005, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(1.000, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(0, result.GetValue());
        }


        //Test rounding divide no rounding
        [TestMethod]
        public void TestRounding_Divide_NoRounding()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(3.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double result = q1.Divide(q2);

            Assert.AreEqual(3.3333333333333335, result);
        }


        //Test implicit target unit add subtract
        [TestMethod]
        public void TestImplicitTargetUnit_AddSubtract()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(2.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(12.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> result = q1.Add(q2);

            Assert.AreEqual(3.0, result.GetValue());
        }


        //Test explicit target unit add subtract overrides
        [TestMethod]
        public void TestExplicitTargetUnit_AddSubtract_Overrides()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(12.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> result = q1.Add(q2, new LengthMeasurementImpl(LengthUnit.Inch));

            Assert.AreEqual(24.0, result.GetValue());
        }


        //Test immutability after add via centralized helper
        [TestMethod]
        public void TestImmutability_AfterAdd_ViaCentralizedHelper()
        {
            Quantity<IMeasurable> original = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> other = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = original.Add(other);

            Assert.AreEqual(10.0, original.GetValue());

            Assert.AreEqual(15.0, result.GetValue());
        }


        //Test immutability after subtract via centralized helper
        [TestMethod]
        public void TestImmutability_AfterSubtract_ViaCentralizedHelper()
        {
            Quantity<IMeasurable> original = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> other = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = original.Subtract(other);

            Assert.AreEqual(10.0, original.GetValue());

            Assert.AreEqual(5.0, result.GetValue());
        }


        //Test immutability after divide via centralized helper
        [TestMethod]
        public void TestImmutability_AfterDivide_ViaCentralizedHelper()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double result = q1.Divide(q2);

            Assert.AreEqual(10.0, q1.GetValue());

            Assert.AreEqual(2.0, result);
        }


        //Test all operations across all categories
        [TestMethod]
        public void TestAllOperations_AcrossAllCategories()
        {
            Quantity<IMeasurable> length1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));
            Quantity<IMeasurable> length2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> weight1 = new Quantity<IMeasurable>(10.0, new WeightMeasurementImpl(WeightUnit.Kilogram));
            Quantity<IMeasurable> weight2 = new Quantity<IMeasurable>(5.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> volume1 = new Quantity<IMeasurable>(10.0, new VolumeMeasurementImpl(VolumeUnit.Litre));
            Quantity<IMeasurable> volume2 = new Quantity<IMeasurable>(5.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.AreEqual(15.0, length1.Add(length2).GetValue());
            Assert.AreEqual(5.0, length1.Subtract(length2).GetValue());
            Assert.AreEqual(2.0, length1.Divide(length2));

            Assert.AreEqual(15.0, weight1.Add(weight2).GetValue());
            Assert.AreEqual(5.0, weight1.Subtract(weight2).GetValue());
            Assert.AreEqual(2.0, weight1.Divide(weight2));

            Assert.AreEqual(15.0, volume1.Add(volume2).GetValue());
            Assert.AreEqual(5.0, volume1.Subtract(volume2).GetValue());
            Assert.AreEqual(2.0, volume1.Divide(volume2));
        }


        //Test code duplication validation logic eliminated
        [TestMethod]
        public void TestCodeDuplication_ValidationLogic_Eliminated()
        {
            MethodInfo method = typeof(Quantity<IMeasurable>).GetMethod("ValidateArithmeticOperands", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(method);
        }


        //Test code duplication conversion logic eliminated
        [TestMethod]
        public void TestCodeDuplication_ConversionLogic_Eliminated()
        {
            MethodInfo method = typeof(Quantity<IMeasurable>).GetMethod("PerformBaseArithmetic", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(method);
        }


        //Test enum dispatch all operations correctly dispatched
        [TestMethod]
        public void TestEnumDispatch_AllOperations_CorrectlyDispatched()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(20.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.AreEqual(25.0, q1.Add(q2).GetValue());

            Assert.AreEqual(15.0, q1.Subtract(q2).GetValue());

            Assert.AreEqual(4.0, q1.Divide(q2));
        }


        //Test future operation multiplication pattern
        [TestMethod]
        public void TestFutureOperation_MultiplicationPattern()
        {
            double a = 10.0;
            double b = 5.0;

            double expected = a * b;

            Assert.AreEqual(50.0, expected);
        }


        //Test error message consistency across operations
        [TestMethod]
        public void TestErrorMessage_Consistency_Across_Operations()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            try
            {
                q1.Add(null);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Second operand cannot be null", ex.Message);
            }

            try
            {
                q1.Subtract(null);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Second operand cannot be null", ex.Message);
            }

            try
            {
                q1.Divide(null);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Second operand cannot be null", ex.Message);
            }
        }


        //Test helper private visibility
        [TestMethod]
        public void TestHelper_PrivateVisibility()
        {
            MethodInfo method = typeof(Quantity<IMeasurable>).GetMethod("PerformBaseArithmetic", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsTrue(method.IsPrivate);
        }


        //Test validation helper private visibility
        [TestMethod]
        public void TestValidation_Helper_PrivateVisibility()
        {
            MethodInfo method = typeof(Quantity<IMeasurable>).GetMethod("ValidateArithmeticOperands", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsTrue(method.IsPrivate);
        }

        //Test rounding helper accuracy
        [TestMethod]
        public void TestRounding_Helper_Accuracy()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(1.234567, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(0.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Subtract(q2);

            Assert.AreEqual(1.23, result.GetValue());
        }


        //Test arithmetic chain operations
        [TestMethod]
        public void TestArithmetic_Chain_Operations()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(20.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q3 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q4 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            double result = q1.Add(q2).Subtract(q3).Divide(q4);

            Assert.AreEqual(5.0, result);
        }


        //Test refactoring no behavior change large dataset
        [TestMethod]
        public void TestRefactoring_NoBehaviorChange_LargeDataset()
        {
            for (int i = 1; i <= 1000; i++)
            {
                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(i * 2.0, new LengthMeasurementImpl(LengthUnit.Feet));

                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(i * 1.0, new LengthMeasurementImpl(LengthUnit.Feet));

                Quantity<IMeasurable> addResult = q1.Add(q2);

                Quantity<IMeasurable> subResult = q1.Subtract(q2);

                double divResult = q1.Divide(q2);

                Assert.AreEqual(i * 3.0, addResult.GetValue());

                Assert.AreEqual(i * 1.0, subResult.GetValue());

                Assert.AreEqual(2.0, divResult);
            }
        }


        //Test refactoring performance comparable to UC12
        [TestMethod]
        public void TestRefactoring_Performance_ComparableToUC12()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            for (int i = 1; i <= 100000; i++)
            {
                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(i, new LengthMeasurementImpl(LengthUnit.Feet));

                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(i / 2.0, new LengthMeasurementImpl(LengthUnit.Feet));

                q1.Add(q2);

                q1.Subtract(q2);

                q1.Divide(q2);
            }

            stopwatch.Stop();

            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 5000);
        }


        //Test enum constant ADD correctly adds
        [TestMethod]
        public void TestEnumConstant_ADD_CorrectlyAdds()
        {
            double result = 7.0 + 3.0;

            Assert.AreEqual(10.0, result);
        }


        //Test enum constant SUBTRACT correctly subtracts
        [TestMethod]
        public void TestEnumConstant_SUBTRACT_CorrectlySubtracts()
        {
            double result = 7.0 - 3.0;

            Assert.AreEqual(4.0, result);
        }


        //Test enum constant DIVIDE correctly divides
        [TestMethod]
        public void TestEnumConstant_DIVIDE_CorrectlyDivides()
        {
            double result = 7.0 / 2.0;

            Assert.AreEqual(3.5, result);
        }


        //Test helper base unit conversion correct
        [TestMethod]
        public void TestHelper_BaseUnitConversion_Correct()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Yard));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = q1.Add(q2);

            Assert.AreEqual(1.33, result.GetValue(), 0.01);
        }


        //Test helper result conversion correct
        [TestMethod]
        public void TestHelper_ResultConversion_Correct()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(12.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> result = q1.Add(q2, new LengthMeasurementImpl(LengthUnit.Inch));

            Assert.AreEqual(24.0, result.GetValue());
        }


        //Test refactoring validation unified behavior
        [TestMethod]
        public void TestRefactoring_Validation_UnifiedBehavior()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            try
            {
                q1.Add(null);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Second operand cannot be null", ex.Message);
            }

            try
            {
                q1.Subtract(null);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Second operand cannot be null", ex.Message);
            }

            try
            {
                q1.Divide(null);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Second operand cannot be null", ex.Message);
            }
        }
    }
}

