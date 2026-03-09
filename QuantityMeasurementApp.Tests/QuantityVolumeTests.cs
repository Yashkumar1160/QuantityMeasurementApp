using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityVolumeTests
    {
        //-----------VOLUME QUANTITY EQUALITY Test-----------

        //Test equality of litre to litre with same value
        [TestMethod]
        public void TestEquality_LitreToLitre_SameValue()
        {
            Quantity<IMeasurable> v1 = new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> v2 = new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.IsTrue(v1.Equals(v2));
        }


        //Test equality of litre to litre with different value
        [TestMethod]
        public void TestEquality_LitreToLitre_DifferentValue()
        {
            Quantity<IMeasurable> v1 = new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> v2 = new Quantity<IMeasurable>(2.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.IsFalse(v1.Equals(v2));
        }


        //Test equality of litre to millilitre conversion
        [TestMethod]
        public void TestEquality_LitreToMillilitre_EquivalentValue()
        {
            Quantity<IMeasurable> litre = new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> ml = new Quantity<IMeasurable>(1000.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Assert.IsTrue(litre.Equals(ml));
        }


        //Test symmetry of millilitre to litre conversion
        [TestMethod]
        public void TestEquality_MillilitreToLitre_EquivalentValue()
        {
            Quantity<IMeasurable> ml = new Quantity<IMeasurable>(1000.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> litre = new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.IsTrue(ml.Equals(litre));
        }


        //----------GALLON EQUALITY TestS-----------

        //Test equality of litre to gallon conversion
        [TestMethod]
        public void TestEquality_LitreToGallon_EquivalentValue()
        {
            Quantity<IMeasurable> litre =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> gallon =
                new Quantity<IMeasurable>(0.264172, new VolumeMeasurementImpl(VolumeUnit.Gallon));

            Assert.IsTrue(litre.Equals(gallon));
        }


        //Test symmetry of gallon to litre conversion
        [TestMethod]
        public void TestEquality_GallonToLitre_EquivalentValue()
        {
            Quantity<IMeasurable> gallon =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Gallon));

            Quantity<IMeasurable> litre =
                new Quantity<IMeasurable>(3.78541, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.IsTrue(gallon.Equals(litre));
        }

        //----------CROSS CATEGORY TestS------------

        //Test comparison between volume and length (should be incompatible)
        [TestMethod]
        public void TestEquality_VolumeVsLength_Incompatible()
        {
            Quantity<IMeasurable> volume =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> length =
                new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.IsFalse(volume.Equals(length));
        }


        //Test comparison between volume and weight (should be incompatible)
        [TestMethod]
        public void TestEquality_VolumeVsWeight_Incompatible()
        {
            Quantity<IMeasurable> volume =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> weight =
                new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.IsFalse(volume.Equals(weight));
        }


        //-----------EDGE CASE TestS------------
        //Test equality comparison with null
        [TestMethod]
        public void TestEquality_NullComparison()
        {
            Quantity<IMeasurable> volume =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.IsFalse(volume.Equals(null));
        }


        //Test reflexive property (same reference)
        [TestMethod]
        public void TestEquality_SameReference()
        {
            Quantity<IMeasurable> volume =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.IsTrue(volume.Equals(volume));
        }


        // Test constructor with null unit
        [TestMethod]
        public void TestEquality_NullUnit()
        {
            // Act + Assert -> check if ArgumentException is thrown
            Assert.Throws<ArgumentException>(() =>
            {
                Quantity<IMeasurable> volume =
                    new Quantity<IMeasurable>(1.0, null);
            });
        }


        //Test zero value equality
        [TestMethod]
        public void TestEquality_ZeroValue()
        {
            Quantity<IMeasurable> v1 =
                new Quantity<IMeasurable>(0.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> v2 =
                new Quantity<IMeasurable>(0.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Assert.IsTrue(v1.Equals(v2));
        }


        //Test negative volume values
        [TestMethod]
        public void TestEquality_NegativeVolume()
        {
            Quantity<IMeasurable> v1 =
                new Quantity<IMeasurable>(-1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> v2 =
                new Quantity<IMeasurable>(-1000.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Assert.IsTrue(v1.Equals(v2));
        }


        //Test large volume values
        [TestMethod]
        public void TestEquality_LargeVolumeValue()
        {
            Quantity<IMeasurable> v1 =
                new Quantity<IMeasurable>(1000000.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> v2 =
                new Quantity<IMeasurable>(1000.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.IsTrue(v1.Equals(v2));
        }


        //Test small volume values
        [TestMethod]
        public void TestEquality_SmallVolumeValue()
        {
            Quantity<IMeasurable> v1 =
                new Quantity<IMeasurable>(0.001, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> v2 =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Assert.IsTrue(v1.Equals(v2));
        }


        //-----------VOLUME CONVERSION Test-----------


        //Test conversion from litre to millilitre
        [TestMethod]
        public void TestConversion_LitreToMillilitre()
        {
            Quantity<IMeasurable> litre =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> result =
                litre.ConvertTo(new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Assert.AreEqual(1000.0, result.ToString().Contains("1000") ? 1000.0 : 0.0);
        }


        //Test conversion from millilitre to litre
        [TestMethod]
        public void TestConversion_MillilitreToLitre()
        {
            Quantity<IMeasurable> ml =
                new Quantity<IMeasurable>(1000.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> result =
                ml.ConvertTo(new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.IsTrue(result.ToString().Contains("1"));
        }


        //Test conversion from gallon to litre
        [TestMethod]
        public void TestConversion_GallonToLitre()
        {
            Quantity<IMeasurable> gallon =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Gallon));

            Quantity<IMeasurable> result =
                gallon.ConvertTo(new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.IsTrue(result.ToString().Contains("3.785"));
        }


        //Test conversion from litre to gallon
        [TestMethod]
        public void TestConversion_LitreToGallon()
        {
            Quantity<IMeasurable> litre =
                new Quantity<IMeasurable>(3.78541, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> result =
                litre.ConvertTo(new VolumeMeasurementImpl(VolumeUnit.Gallon));

            Assert.IsTrue(result.ToString().Contains("1"));
        }


        //Test conversion from millilitre to gallon
        [TestMethod]
        public void TestConversion_MillilitreToGallon()
        {
            Quantity<IMeasurable> ml =
                new Quantity<IMeasurable>(1000.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> result =
                ml.ConvertTo(new VolumeMeasurementImpl(VolumeUnit.Gallon));

            Assert.IsTrue(result.ToString().Contains("0.264"));
        }


        //Test conversion to same unit
        [TestMethod]
        public void TestConversion_SameUnit()
        {
            Quantity<IMeasurable> litre =
                new Quantity<IMeasurable>(5.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> result =
                litre.ConvertTo(new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.IsTrue(result.ToString().Contains("5"));
        }


        //Test conversion of zero value
        [TestMethod]
        public void TestConversion_ZeroValue()
        {
            Quantity<IMeasurable> litre =
                new Quantity<IMeasurable>(0.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> result =
                litre.ConvertTo(new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Assert.IsTrue(result.ToString().Contains("0"));
        }


        //Test conversion of negative value
        [TestMethod]
        public void TestConversion_NegativeValue()
        {
            Quantity<IMeasurable> litre =
                new Quantity<IMeasurable>(-1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> result =
                litre.ConvertTo(new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Assert.IsTrue(result.ToString().Contains("-1000"));
        }


        //Test round-trip conversion (Litre → Millilitre → Litre)
        [TestMethod]
        public void TestConversion_RoundTrip()
        {
            Quantity<IMeasurable> litre =
                new Quantity<IMeasurable>(1.5, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> ml =
                litre.ConvertTo(new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> back =
                ml.ConvertTo(new VolumeMeasurementImpl(VolumeUnit.Litre));

            Assert.IsTrue(back.ToString().Contains("1.5"));
        }


        //--------VOLUME ADDITION TestS-----------

        //Test addition of litre + litre
        [TestMethod]
        public void TestAddition_SameUnit_LitrePlusLitre()
        {
            Quantity<IMeasurable> a =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> b =
                new Quantity<IMeasurable>(2.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> result = a.Add(b);

            Assert.IsTrue(result.ToString().Contains("3"));
        }


        //Test addition of millilitre + millilitre
        [TestMethod]
        public void TestAddition_SameUnit_MillilitrePlusMillilitre()
        {
            Quantity<IMeasurable> a =
                new Quantity<IMeasurable>(500.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> b =
                new Quantity<IMeasurable>(500.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> result = a.Add(b);

            Assert.IsTrue(result.ToString().Contains("1000"));
        }


        //Test addition of litre + millilitre
        [TestMethod]
        public void TestAddition_CrossUnit_LitrePlusMillilitre()
        {
            Quantity<IMeasurable> litre =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> ml =
                new Quantity<IMeasurable>(1000.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> result = litre.Add(ml);

            Assert.IsTrue(result.ToString().Contains("2"));
        }


        //Test addition of millilitre + litre
        [TestMethod]
        public void TestAddition_CrossUnit_MillilitrePlusLitre()
        {
            Quantity<IMeasurable> ml =
                new Quantity<IMeasurable>(1000.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> litre =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> result = ml.Add(litre);

            Assert.IsTrue(result.ToString().Contains("2000"));
        }


        //Test addition of gallon + litre
        [TestMethod]
        public void TestAddition_CrossUnit_GallonPlusLitre()
        {
            Quantity<IMeasurable> gallon =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Gallon));

            Quantity<IMeasurable> litre =
                new Quantity<IMeasurable>(3.78541, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> result = gallon.Add(litre);

            Assert.IsTrue(result.ToString().Contains("2"));
        }


        //--------ADDITION PROPERTY TestS-------------

        //Test commutativity of addition
        [TestMethod]
        public void TestAddition_Commutativity()
        {
            Quantity<IMeasurable> a =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> b =
                new Quantity<IMeasurable>(1000.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> r1 = a.Add(b);
            Quantity<IMeasurable> r2 = b.Add(a);

            Assert.IsTrue(r1.Equals(r2));
        }


        //Test addition with zero
        [TestMethod]
        public void TestAddition_WithZero()
        {
            Quantity<IMeasurable> a =
                new Quantity<IMeasurable>(5.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> zero =
                new Quantity<IMeasurable>(0.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> result = a.Add(zero);

            Assert.IsTrue(result.ToString().Contains("5"));
        }


        //Test addition with negative values
        [TestMethod]
        public void TestAddition_NegativeValues()
        {
            Quantity<IMeasurable> a =
                new Quantity<IMeasurable>(5.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> b =
                new Quantity<IMeasurable>(-2000.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> result = a.Add(b);

            Assert.IsTrue(result.ToString().Contains("3"));
        }


        //--------LARGE AND SMALL VALUES TestS----------

        //Test addition with large values
        [TestMethod]
        public void TestAddition_LargeValues()
        {
            Quantity<IMeasurable> a =
                new Quantity<IMeasurable>(1000000.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> b =
                new Quantity<IMeasurable>(1000000.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> result = a.Add(b);

            Assert.IsTrue(result.ToString().Contains("2000000"));
        }


        //Test addition with small values
        [TestMethod]
        public void TestAddition_SmallValues()
        {
            Quantity<IMeasurable> a =
                new Quantity<IMeasurable>(0.001, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> b =
                new Quantity<IMeasurable>(0.002, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> result = a.Add(b);

            Assert.IsTrue(result.ToString().Contains("0.003"));
        }


        //----------VOLUME ENUM TestS-------------

        //Test VolumeUnit Litre constant
        [TestMethod]
        public void TestVolumeUnitEnum_LitreConstant()
        {
            IMeasurable litre = new VolumeMeasurementImpl(VolumeUnit.Litre);

            double factor = litre.GetConversionFactor();

            Assert.AreEqual(1.0, factor);
        }


        //Test VolumeUnit Millilitre constant
        [TestMethod]
        public void TestVolumeUnitEnum_MillilitreConstant()
        {
            IMeasurable ml = new VolumeMeasurementImpl(VolumeUnit.Millilitre);

            double factor = ml.GetConversionFactor();

            Assert.AreEqual(0.001, factor);
        }


        //Test VolumeUnit Gallon constant
        [TestMethod]
        public void TestVolumeUnitEnum_GallonConstant()
        {
            IMeasurable gallon = new VolumeMeasurementImpl(VolumeUnit.Gallon);

            double factor = gallon.GetConversionFactor();

            Assert.AreEqual(3.78541, factor);
        }


        //----------CONVERT TO BASE UNIT TestS-------------

        //Test Convert Litre to base unit when already base unit
        [TestMethod]
        public void TestConvertToBaseUnit_LitreToLitre()
        {
            IMeasurable litre = new VolumeMeasurementImpl(VolumeUnit.Litre);

            double result = litre.ConvertToBaseUnit(5.0);

            Assert.AreEqual(5.0, result);
        }


        //Test Convert Millilitre to base unit (Litre)
        [TestMethod]
        public void TestConvertToBaseUnit_MillilitreToLitre()
        {
            IMeasurable ml = new VolumeMeasurementImpl(VolumeUnit.Millilitre);

            double result = ml.ConvertToBaseUnit(1000.0);

            Assert.AreEqual(1.0, result);
        }


        //Test Convert Gallon to base unit (Litre)
        [TestMethod]
        public void TestConvertToBaseUnit_GallonToLitre()
        {
            IMeasurable gallon = new VolumeMeasurementImpl(VolumeUnit.Gallon);

            double result = gallon.ConvertToBaseUnit(1.0);

            Assert.AreEqual(3.78541, result);
        }


        //----------CONVERT FROM BASE UNIT TestS-------------

        //Test Convert from base unit (Litre) to Litre
        [TestMethod]
        public void TestConvertFromBaseUnit_LitreToLitre()
        {
            IMeasurable litre = new VolumeMeasurementImpl(VolumeUnit.Litre);

            double result = litre.ConvertFromBaseUnit(2.0);

            Assert.AreEqual(2.0, result);
        }


        //Test Convert from base unit (Litre) to Millilitre
        [TestMethod]
        public void TestConvertFromBaseUnit_LitreToMillilitre()
        {
            IMeasurable ml = new VolumeMeasurementImpl(VolumeUnit.Millilitre);

            double result = ml.ConvertFromBaseUnit(1.0);

            Assert.AreEqual(1000.0, result);
        }


        //Test Convert from base unit (Litre) to Gallon
        [TestMethod]
        public void TestConvertFromBaseUnit_LitreToGallon()
        {
            IMeasurable gallon = new VolumeMeasurementImpl(VolumeUnit.Gallon);

            double result = gallon.ConvertFromBaseUnit(3.78541);

            Assert.AreEqual(1.0, result, 0.0001);
        }


        //----------SYSTEM Tests-------------

        //Test backward compatibility of UC1 to UC10
        [TestMethod]
        public void TestBackwardCompatibility_AllUC1Through10Tests()
        {
            //create two quantities with different units
            Quantity<IMeasurable> length1 =
                new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> length2 =
                new Quantity<IMeasurable>(100.0, new LengthMeasurementImpl(LengthUnit.Centimeter));

            //add the quantities
            Quantity<IMeasurable> result = length1.Add(length2);

            //check result
            Assert.AreEqual(4.28084, result.GetValue(), 0.0001);
        }


        //Test generic Quantity operations for volume
        [TestMethod]
        public void TestGenericQuantity_VolumeOperations_Consistency()
        {
            Quantity<IMeasurable> v1 =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> v2 =
                new Quantity<IMeasurable>(1000.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Quantity<IMeasurable> result = v1.Add(v2);

            Assert.IsTrue(result.Equals(
                new Quantity<IMeasurable>(2.0, new VolumeMeasurementImpl(VolumeUnit.Litre))));
        }


        //Test scalability after adding volume measurements
        [TestMethod]
        public void TestScalability_VolumeIntegration()
        {
            Quantity<IMeasurable> v1 =
                new Quantity<IMeasurable>(3.78541, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> v2 =
                new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Gallon));

            Quantity<IMeasurable> result = v1.Add(v2);

            Quantity<IMeasurable> gallons =
                result.ConvertTo(new VolumeMeasurementImpl(VolumeUnit.Gallon));

            Assert.IsTrue(gallons.ToString().Contains("2"));
        }
    }
}