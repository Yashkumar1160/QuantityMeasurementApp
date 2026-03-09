using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class TemperatureQuantityTests
    {
        //Test temperature equality Celsius to Celsius same value
        [TestMethod]
        public void TestTemperatureEquality_CelsiusToCelsius_SameValue()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(0.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(0.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Assert.IsTrue(q1.Equals(q2));
        }


        //Test temperature equality Fahrenheit to Fahrenheit same value
        [TestMethod]
        public void TestTemperatureEquality_FahrenheitToFahrenheit_SameValue()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(32.0, new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(32.0, new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Assert.IsTrue(q1.Equals(q2));
        }


        //Test temperature equality Celsius to Fahrenheit 0C equals 32F
        [TestMethod]
        public void TestTemperatureEquality_CelsiusToFahrenheit_0Celsius32Fahrenheit()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(0.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(32.0, new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Assert.IsTrue(q1.Equals(q2));
        }


        //Test temperature equality Celsius to Fahrenheit 100C equals 212F
        [TestMethod]
        public void TestTemperatureEquality_CelsiusToFahrenheit_100Celsius212Fahrenheit()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(100.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(212.0, new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Assert.IsTrue(q1.Equals(q2));
        }


        //Test temperature equality negative 40 Celsius equals negative 40 Fahrenheit
        [TestMethod]
        public void TestTemperatureEquality_CelsiusToFahrenheit_Negative40Equal()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(-40.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(-40.0, new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Assert.IsTrue(q1.Equals(q2));
        }


        //Test temperature equality symmetric property
        [TestMethod]
        public void TestTemperatureEquality_SymmetricProperty()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(0.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(32.0, new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Assert.IsTrue(q1.Equals(q2));

            Assert.IsTrue(q2.Equals(q1));
        }


        //Test temperature equality reflexive property
        [TestMethod]
        public void TestTemperatureEquality_ReflexiveProperty()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(50.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Assert.IsTrue(q1.Equals(q1));
        }


        //Test temperature conversion Celsius to Fahrenheit various values
        [TestMethod]
        public void TestTemperatureConversion_CelsiusToFahrenheit_VariousValues()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(50.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> result1 = q1.ConvertTo(new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Assert.AreEqual(122.0, result1.GetValue(), 0.001);


            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(-20.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> result2 = q2.ConvertTo(new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Assert.AreEqual(-4.0, result2.GetValue(), 0.001);
        }


        //Test temperature conversion Fahrenheit to Celsius various values
        [TestMethod]
        public void TestTemperatureConversion_FahrenheitToCelsius_VariousValues()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(122.0, new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Quantity<IMeasurable> result1 = q1.ConvertTo(new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Assert.AreEqual(50.0, result1.GetValue(), 0.001);


            Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(-4.0, new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Quantity<IMeasurable> result2 = q2.ConvertTo(new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Assert.AreEqual(-20.0, result2.GetValue(), 0.001);
        }


        //Test temperature conversion round trip preserves value
        [TestMethod]
        public void TestTemperatureConversion_RoundTrip_PreservesValue()
        {
            Quantity<IMeasurable> original = new Quantity<IMeasurable>(25.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> converted = original.ConvertTo(new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Quantity<IMeasurable> back = converted.ConvertTo(new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Assert.AreEqual(original.GetValue(), back.GetValue(), 0.001);
        }


        //Test temperature conversion same unit
        [TestMethod]
        public void TestTemperatureConversion_SameUnit()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(25.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> result = q1.ConvertTo(new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Assert.AreEqual(25.0, result.GetValue());
        }


        //Test temperature conversion zero value
        [TestMethod]
        public void TestTemperatureConversion_ZeroValue()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(0.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> result = q1.ConvertTo(new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Assert.AreEqual(32.0, result.GetValue(), 0.001);
        }


        //Test temperature conversion negative values
        [TestMethod]
        public void TestTemperatureConversion_NegativeValues()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(-10.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> result = q1.ConvertTo(new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Assert.AreEqual(14.0, result.GetValue(), 0.001);
        }


        //Test temperature conversion large values
        [TestMethod]
        public void TestTemperatureConversion_LargeValues()
        {
            Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(1000.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> result = q1.ConvertTo(new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Assert.AreEqual(1832.0, result.GetValue(), 0.001);
        }


        //Test temperature unsupported operation add
        [TestMethod]
        public void TestTemperatureUnsupportedOperation_Add()
        {
            try
            {
                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(100.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(50.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

                q1.Add(q2);

                Assert.Fail("Expected NotSupportedException for temperature addition");
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Length > 0);
            }
        }


        //Test temperature unsupported operation subtract
        [TestMethod]
        public void TestTemperatureUnsupportedOperation_Subtract()
        {
            try
            {
                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(100.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(50.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

                q1.Subtract(q2);

                Assert.Fail("Expected NotSupportedException for temperature subtraction");
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Length > 0);
            }
        }


        //Test temperature unsupported operation divide
        [TestMethod]
        public void TestTemperatureUnsupportedOperation_Divide()
        {
            try
            {
                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(100.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(50.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

                q1.Divide(q2);

                Assert.Fail("Expected NotSupportedException for temperature division");
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Length > 0);
            }
        }


        //Test temperature unsupported operation error message
        [TestMethod]
        public void TestTemperatureUnsupportedOperation_ErrorMessage()
        {
            try
            {
                Quantity<IMeasurable> q1 = new Quantity<IMeasurable>(100.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

                Quantity<IMeasurable> q2 = new Quantity<IMeasurable>(10.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

                q1.Add(q2);

                Assert.Fail("Expected NotSupportedException");
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("not supported") || ex.Message.Length > 0);
            }
        }


        //Test temperature vs length incompatibility
        [TestMethod]
        public void TestTemperatureVsLengthIncompatibility()
        {
            Quantity<IMeasurable> temperature = new Quantity<IMeasurable>(100.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> length = new Quantity<IMeasurable>(100.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.IsFalse(temperature.Equals(length));
        }


        //Test temperature vs weight incompatibility
        [TestMethod]
        public void TestTemperatureVsWeightIncompatibility()
        {
            Quantity<IMeasurable> temperature = new Quantity<IMeasurable>(50.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> weight = new Quantity<IMeasurable>(50.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.IsFalse(temperature.Equals(weight));
        }


        //Test temperature vs volume incompatibility
        [TestMethod]
        public void TestTemperatureVsVolumeIncompatibility()
        {
            Quantity<IMeasurable> temperature = new Quantity<IMeasurable>(25.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> volume = new Quantity<IMeasurable>(25.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.IsFalse(temperature.Equals(volume));
        }


        //Test operation support methods temperature unit addition
        [TestMethod]
        public void TestOperationSupportMethods_TemperatureUnitAddition()
        {
            TemperatureMeasurementImpl temp = new TemperatureMeasurementImpl(TemperatureUnit.Celsius);

            bool result = temp.SupportsArithmetic();

            Assert.IsFalse(result);
        }

        //Test operation support methods temperature unit division
        [TestMethod]
        public void TestOperationSupportMethods_TemperatureUnitDivision()
        {
            TemperatureMeasurementImpl temp = new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit);

            bool result = temp.SupportsArithmetic();

            Assert.IsFalse(result);
        }


        //Test operation support methods length unit addition
        [TestMethod]
        public void TestOperationSupportMethods_LengthUnitAddition()
        {
            LengthMeasurementImpl length = new LengthMeasurementImpl(LengthUnit.Feet);

            bool result = length.SupportsArithmetic();

            Assert.IsTrue(result);
        }


        //Test operation support methods weight unit division
        [TestMethod]
        public void TestOperationSupportMethods_WeightUnitDivision()
        {
            WeightMeasurementImpl weight = new WeightMeasurementImpl(WeightUnit.Kilogram);

            bool result = weight.SupportsArithmetic();

            Assert.IsTrue(result);
        }


        //Test IMeasurable interface evolution backward compatible
        [TestMethod]
        public void TestIMeasurableInterface_Evolution_BackwardCompatible()
        {
            Quantity<IMeasurable> length = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = length.ConvertTo(new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.AreEqual(10.0, result.GetValue());
        }


        //Test temperature unit non linear conversion
        [TestMethod]
        public void TestTemperatureUnit_NonLinearConversion()
        {
            Quantity<IMeasurable> temp = new Quantity<IMeasurable>(10.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> result = temp.ConvertTo(new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            //10°C should equal 50°F if formula used
            Assert.AreEqual(50.0, result.GetValue(), 0.001);
        }


        //Test temperature unit all constants
        [TestMethod]
        public void TestTemperatureUnit_AllConstants()
        {
            TemperatureUnit celsius = TemperatureUnit.Celsius;

            TemperatureUnit fahrenheit = TemperatureUnit.Fahrenheit;

            Assert.IsNotNull(celsius);

            Assert.IsNotNull(fahrenheit);
        }


        //Test temperature unit name method
        [TestMethod]
        public void TestTemperatureUnit_NameMethod()
        {
            TemperatureMeasurementImpl temp = new TemperatureMeasurementImpl(TemperatureUnit.Celsius);

            string name = temp.GetUnitName();

            Assert.IsTrue(name.Contains("Celsius") || name.Contains("CELSIUS"));
        }


        //Test temperature unit conversion factor
        [TestMethod]
        public void TestTemperatureUnit_ConversionFactor()
        {
            TemperatureMeasurementImpl temp = new TemperatureMeasurementImpl(TemperatureUnit.Celsius);

            double factor = temp.GetConversionFactor();

            Assert.AreEqual(1.0, factor);
        }


        //Test temperature null unit validation
        [TestMethod]
        public void TestTemperatureNullUnitValidation()
        {
            try
            {
                Quantity<IMeasurable> q = new Quantity<IMeasurable>(100.0, null);

                Assert.Fail("Expected ArgumentException for null unit");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }


        //Test temperature null operand validation in comparison
        [TestMethod]
        public void TestTemperatureNullOperandValidation_InComparison()
        {
            Quantity<IMeasurable> temperature = new Quantity<IMeasurable>(25.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Assert.IsFalse(temperature.Equals(null));
        }


        //Test temperature different values inequality
        [TestMethod]
        public void TestTemperatureDifferentValuesInequality()
        {
            Quantity<IMeasurable> temp1 = new Quantity<IMeasurable>(50.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> temp2 = new Quantity<IMeasurable>(100.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Assert.IsFalse(temp1.Equals(temp2));
        }


        //Test temperature backward compatibility UC1 through UC13
        [TestMethod]
        public void TestTemperatureBackwardCompatibility_UC1_Through_UC13()
        {
            Quantity<IMeasurable> length1 = new Quantity<IMeasurable>(10.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> length2 = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = length1.Add(length2);

            Assert.AreEqual(15.0, result.GetValue());
        }


        //Test temperature conversion precision epsilon
        [TestMethod]
        public void TestTemperatureConversionPrecision_Epsilon()
        {
            Quantity<IMeasurable> celsius = new Quantity<IMeasurable>(0.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> fahrenheit = new Quantity<IMeasurable>(32.0000001, new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Assert.IsTrue(celsius.Equals(fahrenheit));
        }


        //Test temperature conversion edge case very small difference
        [TestMethod]
        public void TestTemperatureConversionEdgeCase_VerySmallDifference()
        {
            Quantity<IMeasurable> temp1 = new Quantity<IMeasurable>(25.0000001, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> temp2 = new Quantity<IMeasurable>(25.0000002, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Assert.IsTrue(temp1.Equals(temp2));
        }


        //Test temperature enum implements IMeasurable
        [TestMethod]
        public void TestTemperatureEnumImplementsIMeasurable()
        {
            TemperatureMeasurementImpl temp = new TemperatureMeasurementImpl(TemperatureUnit.Celsius);

            Assert.IsInstanceOfType(temp, typeof(IMeasurable));
        }


        //Test temperature default method inheritance
        [TestMethod]
        public void TestTemperatureDefaultMethodInheritance()
        {
            LengthMeasurementImpl length = new LengthMeasurementImpl(LengthUnit.Feet);

            WeightMeasurementImpl weight = new WeightMeasurementImpl(WeightUnit.Kilogram);

            bool lengthSupportsAdd = length.SupportsArithmetic();

            bool weightSupportsDivide = weight.SupportsArithmetic();

            Assert.IsTrue(lengthSupportsAdd);

            Assert.IsTrue(weightSupportsDivide);
        }


        //Test temperature cross unit addition attempt
        [TestMethod]
        public void TestTemperatureCrossUnitAdditionAttempt()
        {
            try
            {
                Quantity<IMeasurable> celsius = new Quantity<IMeasurable>(100.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

                Quantity<IMeasurable> fahrenheit = new Quantity<IMeasurable>(212.0, new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

                celsius.Add(fahrenheit);

                Assert.Fail("Expected NotSupportedException for temperature addition");
            }
            catch (NotSupportedException)
            {
                Assert.IsTrue(true);
            }
        }


        //Test temperature validate operation support method behavior
        [TestMethod]
        public void TestTemperatureValidateOperationSupport_MethodBehavior()
        {
            try
            {
                TemperatureMeasurementImpl temp = new TemperatureMeasurementImpl(TemperatureUnit.Celsius);

                temp.ValidateOperationSupport("addition");

                Assert.Fail("Expected NotSupportedException for unsupported operation");
            }
            catch (NotSupportedException)
            {
                Assert.IsTrue(true);
            }
        }


        //Test temperature integration with generic quantity
        [TestMethod]
        public void TestTemperatureIntegrationWithGenericQuantity()
        {
            Quantity<IMeasurable> temperature = new Quantity<IMeasurable>(30.0, new TemperatureMeasurementImpl(TemperatureUnit.Celsius));

            Quantity<IMeasurable> converted = temperature.ConvertTo(new TemperatureMeasurementImpl(TemperatureUnit.Fahrenheit));

            Assert.AreEqual(86.0, converted.GetValue(), 0.001);
        }
    }
}