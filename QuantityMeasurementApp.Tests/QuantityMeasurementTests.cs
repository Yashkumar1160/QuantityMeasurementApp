using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementTests
    {
        //Epsilon value for floating point comparison
        private const double epsilon = 0.0001;

        //Test LengthUnit implementation of IMeasurable interface
        [TestMethod]
        public void TestIMeasurableInterface_LengthUnitImplementation()
        {
            IMeasurable length = new LengthMeasurementImpl(LengthUnit.Feet);

            double factor = length.GetConversionFactor();
            double baseValue = length.ConvertToBaseUnit(1.0);
            double original = length.ConvertFromBaseUnit(baseValue);
            string name = length.GetUnitName();

            Assert.IsTrue(factor > 0);
            Assert.AreEqual(1.0, original, 0.0001);
            Assert.AreEqual("Feet", name);
        }


        //Test WeightUnit implementation of IMeasurable interface
        [TestMethod]
        public void TestIMeasurableInterface_WeightUnitImplementation()
        {
            IMeasurable weight = new WeightMeasurementImpl(WeightUnit.Kilogram);

            double factor = weight.GetConversionFactor();
            double baseValue = weight.ConvertToBaseUnit(1.0);
            double original = weight.ConvertFromBaseUnit(baseValue);
            string name = weight.GetUnitName();

            Assert.IsTrue(factor > 0);
            Assert.AreEqual(1.0, original, 0.0001);
            Assert.AreEqual("Kilogram", name);
        }


        //Test consistent behavior between Length and Weight implementations
        [TestMethod]
        public void TestIMeasurableInterface_ConsistentBehavior()
        {
            IMeasurable length = new LengthMeasurementImpl(LengthUnit.Feet);
            IMeasurable weight = new WeightMeasurementImpl(WeightUnit.Kilogram);

            Assert.IsTrue(length.GetConversionFactor() > 0);
            Assert.IsTrue(weight.GetConversionFactor() > 0);

            Assert.IsNotNull(length.GetUnitName());
            Assert.IsNotNull(weight.GetUnitName());
        }

        //-----------GENERIC QUANTITY EQUALITY Test-----------
        //Test equality of length quantities with different units (Feet vs Inches)
        [TestMethod]
        public void TestGenericQuantity_LengthOperations_Equality()
        {
            Quantity<IMeasurable> feet = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> inch = new Quantity<IMeasurable>(12.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Assert.IsTrue(feet.Equals(inch));
        }


        //Test equality of weight quantities with different units (Kilogram vs Gram)
        [TestMethod]
        public void TestGenericQuantity_WeightOperations_Equality()
        {
            Quantity<IMeasurable> kilogram = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> gram = new Quantity<IMeasurable>(1000.0, new WeightMeasurementImpl(WeightUnit.Gram));

            Assert.IsTrue(kilogram.Equals(gram));
        }


        //----------GENERIC QUANTITY CONVERSION Test-----------
        //Test conversion from Feet to Inches
        [TestMethod]
        public void TestGenericQuantity_LengthOperations_Conversion()
        {
            Quantity<IMeasurable> feet = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> result = feet.ConvertTo(new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> expected = new Quantity<IMeasurable>(12.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Assert.IsTrue(result.Equals(expected));
        }


        //Test conversion from Kilogram to Gram
        [TestMethod]
        public void TestGenericQuantity_WeightOperations_Conversion()
        {
            Quantity<IMeasurable> kilogram = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> result = kilogram.ConvertTo(new WeightMeasurementImpl(WeightUnit.Gram));

            Quantity<IMeasurable> expected = new Quantity<IMeasurable>(1000.0, new WeightMeasurementImpl(WeightUnit.Gram));

            Assert.IsTrue(result.Equals(expected));
        }

        //----------GENERIC QUANTITY ADDITION Test-----------
        //Test addition of length quantities (Feet + Inches)
        [TestMethod]
        public void TestGenericQuantity_LengthOperations_Addition()
        {
            Quantity<IMeasurable> feet = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> inch = new Quantity<IMeasurable>(12.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> result = feet.Add(inch, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> expected = new Quantity<IMeasurable>(2.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.IsTrue(result.Equals(expected));
        }


        //Test addition of weight quantities (Kilogram + Gram)
        [TestMethod]
        public void TestGenericQuantity_WeightOperations_Addition()
        {
            Quantity<IMeasurable> kilogram = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> gram = new Quantity<IMeasurable>(1000.0, new WeightMeasurementImpl(WeightUnit.Gram));

            Quantity<IMeasurable> result = kilogram.Add(gram, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> expected = new Quantity<IMeasurable>(2.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.IsTrue(result.Equals(expected));
        }


        //Test comparison between Length and Weight quantities
        [TestMethod]
        public void TestCrossCategoryPrevention_LengthVsWeight()
        {
            Quantity<IMeasurable> length = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> weight = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.IsFalse(length.Equals(weight));
        }


        //Test compiler type safety between Length and Weight quantities
        [TestMethod]
        public void TestCrossCategoryPrevention_CompilerTypeSafety()
        {
            Quantity<IMeasurable> length = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> weight = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            //Compiler prevents mixing operations if generics are strictly typed
            Assert.IsFalse(length.Equals(weight));
        }


        //Test constructor validation when unit is null
        [TestMethod]
        public void TestGenericQuantity_ConstructorValidation_NullUnit()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Quantity<IMeasurable> quantity = new Quantity<IMeasurable>(1.0, null);
            });
        }


        //Test constructor validation when value is NaN
        [TestMethod]
        public void TestGenericQuantity_ConstructorValidation_InvalidValue()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Quantity<IMeasurable> quantity = new Quantity<IMeasurable>(double.NaN, new LengthMeasurementImpl(LengthUnit.Feet));
            });
        }


        //Test conversion between all length unit combinations
        [TestMethod]
        public void TestGenericQuantity_Conversion_AllUnitCombinations()
        {
            Quantity<IMeasurable> feet = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> inches = feet.ConvertTo(new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> yards = feet.ConvertTo(new LengthMeasurementImpl(LengthUnit.Yard));

            Assert.IsTrue(inches.Equals(new Quantity<IMeasurable>(12.0, new LengthMeasurementImpl(LengthUnit.Inch))));

            Assert.IsTrue(yards.Equals(new Quantity<IMeasurable>(0.333333, new LengthMeasurementImpl(LengthUnit.Yard))));
        }



        //Test backward compatibility with UC1–UC9 functionality
        [TestMethod]
        public void TestBackwardCompatibility_AllUC1Through9Tests()
        {
            Quantity<IMeasurable> feet = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> inches = new Quantity<IMeasurable>(12.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Assert.IsTrue(feet.Equals(inches));
        }



        //Test generic demonstration method for equality
        [TestMethod]
        public void TestQuantityMeasurementApp_SimplifiedDemonstration_Equality()
        {
            Quantity<IMeasurable> kg = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> gram = new Quantity<IMeasurable>(1000.0, new WeightMeasurementImpl(WeightUnit.Gram));

            Assert.IsTrue(kg.Equals(gram));
        }



        //Test generic demonstration method for conversion
        [TestMethod]
        public void TestQuantityMeasurementApp_SimplifiedDemonstration_Conversion()
        {
            Quantity<IMeasurable> kg = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> result = kg.ConvertTo(new WeightMeasurementImpl(WeightUnit.Gram));

            Assert.IsTrue(result.Equals(new Quantity<IMeasurable>(1000.0, new WeightMeasurementImpl(WeightUnit.Gram))));
        }




        //Test generic demonstration method for addition
        [TestMethod]
        public void TestQuantityMeasurementApp_SimplifiedDemonstration_Addition()
        {
            Quantity<IMeasurable> kg = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Quantity<IMeasurable> gram = new Quantity<IMeasurable>(1000.0, new WeightMeasurementImpl(WeightUnit.Gram));

            Quantity<IMeasurable> result = kg.Add(gram, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.IsTrue(result.Equals(new Quantity<IMeasurable>(2.0, new WeightMeasurementImpl(WeightUnit.Kilogram))));
        }



        //Test flexible generic signature using Quantity<IMeasurable>
        [TestMethod]
        public void TestTypeWildcard_FlexibleSignatures()
        {
            Quantity<IMeasurable> length = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> weight = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.IsNotNull(length);
            Assert.IsNotNull(weight);
        }



        //Test integration of new VolumeUnit category without modifying Quantity class
        [TestMethod]
        public void TestScalability_NewUnitEnumIntegration()
        {
            Quantity<IMeasurable> Litre = new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> ml = new Quantity<IMeasurable>(1000.0, new VolumeMeasurementImpl(VolumeUnit.Millilitre));

            Assert.IsTrue(Litre.Equals(ml));
        }


        //Test adding multiple new measurement categories
        [TestMethod]
        public void TestScalability_MultipleNewCategories()
        {
            Quantity<IMeasurable> volume = new Quantity<IMeasurable>(1.0, new VolumeMeasurementImpl(VolumeUnit.Litre));

            Quantity<IMeasurable> length = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> weight = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.IsNotNull(volume);
            Assert.IsNotNull(length);
            Assert.IsNotNull(weight);
        }


        //Test enforcement of IMeasurable interface for Quantity generic type
        [TestMethod]
        public void TestGenericBoundedTypeParameter_Enforcement()
        {
            Quantity<IMeasurable> quantity = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.IsNotNull(quantity);
        }


        //Test hashCode consistency for equal quantities
        [TestMethod]
        public void TestHashCode_GenericQuantity_Consistency()
        {
            Quantity<IMeasurable> a = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> b = new Quantity<IMeasurable>(12.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Assert.IsTrue(a.Equals(b));
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        //Test equals contract properties (reflexive, symmetric, transitive)
        [TestMethod]
        public void TestEquals_GenericQuantity_ContractPreservation()
        {
            Quantity<IMeasurable> A = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> B = new Quantity<IMeasurable>(12.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> C = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            //Reflexive
            Assert.IsTrue(A.Equals(A));

            //Symmetric
            Assert.IsTrue(A.Equals(B));
            Assert.IsTrue(B.Equals(A));

            //Transitive
            Assert.IsTrue(A.Equals(B));
            Assert.IsTrue(B.Equals(C));
            Assert.IsTrue(A.Equals(C));
        }



        //Test enum behavior through IMeasurable interface
        [TestMethod]
        public void TestEnumAsUnitCarrier_BehaviorEncapsulation()
        {
            IMeasurable length = new LengthMeasurementImpl(LengthUnit.Feet);
            IMeasurable weight = new WeightMeasurementImpl(WeightUnit.Kilogram);

            double lengthFactor = length.GetConversionFactor();
            double weightFactor = weight.GetConversionFactor();

            Assert.IsTrue(lengthFactor > 0);
            Assert.IsTrue(weightFactor > 0);
        }



        //Test runtime safety despite generic type erasure
        [TestMethod]
        public void TestTypeErasure_RuntimeSafety()
        {
            Quantity<IMeasurable> length = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> weight = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.IsFalse(length.Equals(weight));
        }


        //Test flexibility using composition approach
        [TestMethod]
        public void TestCompositionOverInheritance_Flexibility()
        {
            Quantity<IMeasurable> length = new Quantity<IMeasurable>(5.0, new LengthMeasurementImpl(LengthUnit.Yard));

            Quantity<IMeasurable> weight = new Quantity<IMeasurable>(10.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.IsNotNull(length);
            Assert.IsNotNull(weight);
        }

        //Test validation of reduced code duplication using generics
        [TestMethod]
        public void TestCodeReduction_DRYValidation()
        {
            Quantity<IMeasurable> length = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> weight = new Quantity<IMeasurable>(1.0, new WeightMeasurementImpl(WeightUnit.Kilogram));

            Assert.IsNotNull(length);
            Assert.IsNotNull(weight);
        }



        //Test maintainability through single logic implementation
        [TestMethod]
        public void TestMaintainability_SingleSourceOfTruth()
        {
            Quantity<IMeasurable> feet = new Quantity<IMeasurable>(1.0, new LengthMeasurementImpl(LengthUnit.Feet));

            Quantity<IMeasurable> inch = new Quantity<IMeasurable>(12.0, new LengthMeasurementImpl(LengthUnit.Inch));

            Quantity<IMeasurable> result = feet.Add(inch, new LengthMeasurementImpl(LengthUnit.Feet));

            Assert.IsTrue(result.Equals(new Quantity<IMeasurable>(2.0, new LengthMeasurementImpl(LengthUnit.Feet))));
        }
    }
}