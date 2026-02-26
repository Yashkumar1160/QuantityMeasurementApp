using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityWeightTests
    {
        //Epsilon value for floating point comparison
        private const double epsilon = 0.0001;


        //-----------EQUALITY TEST CASES-------------
        //Test same unit (Kilogram) with same value
        [TestMethod]
        public void TestEquality_KilogramToKilogram_SameValue_ReturnsTrue()
        {
            QuantityWeight first = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight second = new QuantityWeight(1.0, WeightUnit.Kilogram);

            Assert.IsTrue(first.Equals(second));
        }

        //Test same unit (Kilogram) with different value
        [TestMethod]
        public void TestEquality_KilogramToKilogram_DifferentValue_ReturnsFalse()
        {
            QuantityWeight first = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight second = new QuantityWeight(2.0, WeightUnit.Kilogram);

            Assert.IsFalse(first.Equals(second));
        }

        //Test kilogram to gram equivalent value
        [TestMethod]
        public void TestEquality_KilogramToGram_EquivalentValue_ReturnsTrue()
        {
            QuantityWeight kilogram = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight gram = new QuantityWeight(1000.0, WeightUnit.Gram);

            Assert.IsTrue(kilogram.Equals(gram));
        }

        //Test gram to kilogram equivalent value (symmetry)
        [TestMethod]
        public void TestEquality_GramToKilogram_EquivalentValue_ReturnsTrue()
        {
            QuantityWeight gram = new QuantityWeight(1000.0, WeightUnit.Gram);
            QuantityWeight kilogram = new QuantityWeight(1.0, WeightUnit.Kilogram);

            Assert.IsTrue(gram.Equals(kilogram));
        }

        //Test incompatible comparison (Weight vs Length)
        [TestMethod]
        public void TestEquality_WeightVsLength_Incompatible_ReturnsFalse()
        {
            QuantityWeight weight = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityLength length = new QuantityLength(1.0, LengthUnit.Feet);

            Assert.IsFalse(weight.Equals(length));
        }

        //Test equality with null comparison
        [TestMethod]
        public void TestEquality_NullComparison_ReturnsFalse()
        {
            QuantityWeight weight = new QuantityWeight(1.0, WeightUnit.Kilogram);

            Assert.IsFalse(weight.Equals(null));
        }

        //Test reflexive property (same reference)
        [TestMethod]
        public void TestEquality_SameReference_ReturnsTrue()
        {
            QuantityWeight weight = new QuantityWeight(1.0, WeightUnit.Kilogram);

            Assert.IsTrue(weight.Equals(weight));
        }

        //Test null unit in constructor
        [TestMethod]
        public void TestEquality_InvalidUnit_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                QuantityWeight weight = new QuantityWeight(1.0, (WeightUnit)999);
            });
        }

        //Test transitive property
        [TestMethod]
        public void TestEquality_TransitiveProperty_ReturnsTrue()
        {
            QuantityWeight A = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight B = new QuantityWeight(1000.0, WeightUnit.Gram);
            QuantityWeight C = new QuantityWeight(1.0, WeightUnit.Kilogram);

            Assert.IsTrue(A.Equals(B));
            Assert.IsTrue(B.Equals(C));
            Assert.IsTrue(A.Equals(C));
        }

        //Test zero value equality
        [TestMethod]
        public void TestEquality_ZeroValue_ReturnsTrue()
        {
            QuantityWeight kilogram = new QuantityWeight(0.0, WeightUnit.Kilogram);
            QuantityWeight gram = new QuantityWeight(0.0, WeightUnit.Gram);

            Assert.IsTrue(kilogram.Equals(gram));
        }

        //Test negative weight equality
        [TestMethod]
        public void TestEquality_NegativeWeight_ReturnsTrue()
        {
            QuantityWeight kilogram = new QuantityWeight(-1.0, WeightUnit.Kilogram);
            QuantityWeight gram = new QuantityWeight(-1000.0, WeightUnit.Gram);

            Assert.IsTrue(kilogram.Equals(gram));
        }

        //Test large weight value equality
        [TestMethod]
        public void TestEquality_LargeWeightValue_ReturnsTrue()
        {
            QuantityWeight gram = new QuantityWeight(1000000.0, WeightUnit.Gram);
            QuantityWeight kilogram = new QuantityWeight(1000.0, WeightUnit.Kilogram);

            Assert.IsTrue(gram.Equals(kilogram));
        }

        //Test small weight value equality
        [TestMethod]
        public void TestEquality_SmallWeightValue_ReturnsTrue()
        {
            QuantityWeight kilogram = new QuantityWeight(0.001, WeightUnit.Kilogram);
            QuantityWeight gram = new QuantityWeight(1.0, WeightUnit.Gram);

            Assert.IsTrue(kilogram.Equals(gram));
        }


        //-----------CONVERSION TEST CASES-------------
        //Test conversion from Pound to Kilogram
        [TestMethod]
        public void TestConversion_PoundToKilogram_ReturnsExpectedValue()
        {
            QuantityWeight pound = new QuantityWeight(2.20462, WeightUnit.Pound);

            QuantityWeight expected = new QuantityWeight(1.0, WeightUnit.Kilogram);

            QuantityWeight result = pound.ConvertTo(WeightUnit.Kilogram);

            Assert.IsTrue(result.Equals(expected));
        }

        //Test conversion from Kilogram to Pound
        [TestMethod]
        public void TestConversion_KilogramToPound_ReturnsExpectedValue()
        {
            QuantityWeight kilogram = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight expected = new QuantityWeight(2.20462, WeightUnit.Pound);

            QuantityWeight result = kilogram.ConvertTo(WeightUnit.Pound);

            Assert.IsTrue(result.Equals(expected));
        }

        //Test conversion to same unit (no change)
        [TestMethod]
        public void TestConversion_SameUnit_ReturnsSameValue()
        {
            QuantityWeight kilogram = new QuantityWeight(5.0, WeightUnit.Kilogram);

            QuantityWeight result = kilogram.ConvertTo(WeightUnit.Kilogram);

            Assert.IsTrue(result.Equals(kilogram));
        }

        //Test zero value conversion
        [TestMethod]
        public void TestConversion_ZeroValue_ReturnsZero()
        {
            QuantityWeight kilogram = new QuantityWeight(0.0, WeightUnit.Kilogram);
            QuantityWeight expected = new QuantityWeight(0.0, WeightUnit.Gram);

            QuantityWeight result = kilogram.ConvertTo(WeightUnit.Gram);

            Assert.IsTrue(result.Equals(expected));
        }

        //Test negative value conversion
        [TestMethod]
        public void TestConversion_NegativeValue_PreservesSign()
        {
            QuantityWeight kilogram = new QuantityWeight(-1.0, WeightUnit.Kilogram);
            QuantityWeight expected = new QuantityWeight(-1000.0, WeightUnit.Gram);

            QuantityWeight result = kilogram.ConvertTo(WeightUnit.Gram);

            Assert.IsTrue(result.Equals(expected));
        }

        //Test round-trip conversion (Kg → Gram → Kg)
        [TestMethod]
        public void TestConversion_RoundTrip_PreservesValue()
        {
            QuantityWeight original = new QuantityWeight(1.5, WeightUnit.Kilogram);

            QuantityWeight converted = original
                                        .ConvertTo(WeightUnit.Gram)
                                        .ConvertTo(WeightUnit.Kilogram);

            Assert.IsTrue(original.Equals(converted));
        }


        //-----------ADDITION TEST CASES-------------
        //Test same unit addition (Kilogram + Kilogram)
        [TestMethod]
        public void TestAddition_SameUnit_KilogramPlusKilogram_ReturnsSum()
        {
            QuantityWeight first = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight second = new QuantityWeight(2.0, WeightUnit.Kilogram);
            QuantityWeight expected = new QuantityWeight(3.0, WeightUnit.Kilogram);

            QuantityWeight result = first.Add(second);

            Assert.IsTrue(result.Equals(expected));
        }

        //Test cross unit addition (Kilogram + Gram)
        [TestMethod]
        public void TestAddition_CrossUnit_KilogramPlusGram_ReturnsKilogram()
        {
            QuantityWeight kilogram = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight gram = new QuantityWeight(1000.0, WeightUnit.Gram);
            QuantityWeight expected = new QuantityWeight(2.0, WeightUnit.Kilogram);

            QuantityWeight result = kilogram.Add(gram);

            Assert.IsTrue(result.Equals(expected));
        }

        //Test cross unit addition (Pound + Kilogram)
        [TestMethod]
        public void TestAddition_CrossUnit_PoundPlusKilogram_ReturnsPound()
        {
            QuantityWeight pound = new QuantityWeight(2.20462, WeightUnit.Pound);
            QuantityWeight kilogram = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight expected = new QuantityWeight(4.40924, WeightUnit.Pound);

            QuantityWeight result = pound.Add(kilogram);

            Assert.IsTrue(result.Equals(expected));
        }

        //Test addition with explicit target unit
        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_ReturnsGram()
        {
            QuantityWeight kilogram = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight gram = new QuantityWeight(1000.0, WeightUnit.Gram);
            QuantityWeight expected = new QuantityWeight(2000.0, WeightUnit.Gram);

            QuantityWeight result = kilogram.Add(gram, WeightUnit.Gram);

            Assert.IsTrue(result.Equals(expected));
        }

        //Test commutativity of addition
        [TestMethod]
        public void TestAddition_Commutativity_ReturnsEqualResults()
        {
            QuantityWeight first = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight second = new QuantityWeight(1000.0, WeightUnit.Gram);

            QuantityWeight result1 = first.Add(second);
            QuantityWeight result2 = second.Add(first);

            Assert.IsTrue(result1.Equals(result2));
        }

        //Test addition with zero
        [TestMethod]
        public void TestAddition_WithZero_ReturnsSameValue()
        {
            QuantityWeight value = new QuantityWeight(5.0, WeightUnit.Kilogram);
            QuantityWeight zero = new QuantityWeight(0.0, WeightUnit.Gram);

            QuantityWeight result = value.Add(zero);

            Assert.IsTrue(result.Equals(value));
        }

        //Test addition with negative values
        [TestMethod]
        public void TestAddition_NegativeValues_ReturnsCorrectResult()
        {
            QuantityWeight positive = new QuantityWeight(5.0, WeightUnit.Kilogram);
            QuantityWeight negative = new QuantityWeight(-2000.0, WeightUnit.Gram);
            QuantityWeight expected = new QuantityWeight(3.0, WeightUnit.Kilogram);

            QuantityWeight result = positive.Add(negative);

            Assert.IsTrue(result.Equals(expected));
        }

        //Test addition with large values
        [TestMethod]
        public void TestAddition_LargeValues_ReturnsCorrectSum()
        {
            QuantityWeight first = new QuantityWeight(1e6, WeightUnit.Kilogram);
            QuantityWeight second = new QuantityWeight(1e6, WeightUnit.Kilogram);
            QuantityWeight expected = new QuantityWeight(2e6, WeightUnit.Kilogram);

            QuantityWeight result = first.Add(second);

            Assert.IsTrue(result.Equals(expected));
        }
    }
}