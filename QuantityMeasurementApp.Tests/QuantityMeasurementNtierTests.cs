using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppBusiness.Exceptions;
using QuantityMeasurementAppBusiness;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppBusiness.Implementations;
using QuantityMeasurementAppModels.Enums;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppServices.Interfaces;
using QuantityMeasurementApp.Services;
using QuantityMeasurementApp.Controllers;

namespace QuantityMeasurementAppTests
{
    // Mock repository for testing — does not save to disk
    public class MockRepository : IQuantityMeasurementRepository
    {
        public QuantityMeasurementEntity? LastSaved;

        public void Save(QuantityMeasurementEntity entity)
        {
            LastSaved = entity;
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            return new List<QuantityMeasurementEntity>();
        }
    }

    // Mock service for controller independence testing
    public class MockService : IQuantityMeasurementService
    {
        public bool CompareResult = true;
        public QuantityDTO ConvertResult = new QuantityDTO(0, "Feet", "Length");
        public QuantityDTO AddResult = new QuantityDTO(0, "Feet", "Length");
        public QuantityDTO SubtractResult = new QuantityDTO(0, "Feet", "Length");
        public double DivideResult = 1.0;

        public bool Compare(QuantityDTO first, QuantityDTO second)
        {
            return CompareResult;
        }

        public QuantityDTO Convert(QuantityDTO quantity, string targetUnit)
        {
            return ConvertResult;
        }

        public QuantityDTO Add(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            return AddResult;
        }

        public QuantityDTO Subtract(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            return SubtractResult;
        }

        public double Divide(QuantityDTO first, QuantityDTO second)
        {
            return DivideResult;
        }
    }

    [TestClass]
    [DoNotParallelize]
    public class QuantityMeasurementNtierTests
    {
        private TextWriter? originalConsole;
        private StringWriter? consoleOutput;

        [TestInitialize]
        public void Setup()
        {
            originalConsole = Console.Out;
            consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (originalConsole != null)
                Console.SetOut(originalConsole);
            consoleOutput?.Dispose();
        }

        // ─── ENTITY TESTS ─────────────────────────────────────────────────────

        //Test single operand entity stores conversion data correctly
        [TestMethod]
        public void TestQuantityEntity_SingleOperandConstruction()
        {
            QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                "Convert", 12.0, "Inch", 1.0);

            Assert.AreEqual("Convert", entity.Operation);
            Assert.AreEqual(12.0, entity.FirstValue);
            Assert.AreEqual("Inch", entity.FirstUnit);
            Assert.AreEqual(1.0, entity.ResultValue);
            Assert.IsFalse(entity.IsError);
        }


        //Test binary operand entity stores addition data correctly
        [TestMethod]
        public void TestQuantityEntity_BinaryOperandConstruction()
        {
            QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                "Add", 2.0, "Feet", 3.0, "Feet", 5.0);

            Assert.AreEqual("Add", entity.Operation);
            Assert.AreEqual(2.0, entity.FirstValue);
            Assert.AreEqual("Feet", entity.FirstUnit);
            Assert.AreEqual(3.0, entity.SecondValue);
            Assert.AreEqual("Feet", entity.SecondUnit);
            Assert.AreEqual(5.0, entity.ResultValue);
            Assert.IsFalse(entity.IsError);
        }


        //Test error entity stores error data correctly
        [TestMethod]
        public void TestQuantityEntity_ErrorConstruction()
        {
            QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                "Add", "Temperature does not support Addition operation");

            Assert.AreEqual("Add", entity.Operation);
            Assert.IsTrue(entity.IsError);
            Assert.AreEqual(
                "Temperature does not support Addition operation",
                entity.ErrorMessage);
        }


        //Test ToString formats successful result with operation and values
        [TestMethod]
        public void TestQuantityEntity_ToString_Success()
        {
            QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                "Convert", 12.0, "Inch", 1.0);

            string result = entity.ToString();

            Assert.IsTrue(result.Contains("Convert"));
            Assert.IsTrue(result.Contains("12"));
        }


        //Test ToString formats error with ERROR label and message
        [TestMethod]
        public void TestQuantityEntity_ToString_Error()
        {
            QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                "Add", "Something went wrong");

            string result = entity.ToString();

            Assert.IsTrue(result.Contains("ERROR"));
            Assert.IsTrue(result.Contains("Something went wrong"));
        }


        // ─── SERVICE TESTS ────────────────────────────────────────────────────

        //Test service compares equal quantities in same unit
        [TestMethod]
        public void TestService_CompareEquality_SameUnit_Success()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityDTO first = new QuantityDTO(5.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(5.0, "Feet", "Length");

            bool result = service.Compare(first, second);

            Assert.IsTrue(result);
        }


        //Test service compares equal quantities in different units
        [TestMethod]
        public void TestService_CompareEquality_DifferentUnit_Success()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityDTO first = new QuantityDTO(1.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(12.0, "Inch", "Length");

            bool result = service.Compare(first, second);

            Assert.IsTrue(result);
        }


        //Test service returns false for cross category comparison
        [TestMethod]
        public void TestService_CompareEquality_CrossCategory_Error()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityDTO length = new QuantityDTO(1.0, "Feet", "Length");
            QuantityDTO temperature = new QuantityDTO(1.0, "Celsius", "Temperature");

            bool result = service.Compare(length, temperature);

            Assert.IsFalse(result);
        }


        //Test service converts quantity to target unit correctly
        [TestMethod]
        public void TestService_Convert_Success()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityDTO quantity = new QuantityDTO(1.0, "Feet", "Length");

            QuantityDTO result = service.Convert(quantity, "Inch");

            Assert.AreEqual(12.0, result.Value, 0.001);
            Assert.AreEqual("Inch", result.UnitName);
        }


        //Test service adds two quantities and returns result in target unit
        [TestMethod]
        public void TestService_Add_Success()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityDTO first = new QuantityDTO(2.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(3.0, "Feet", "Length");

            QuantityDTO result = service.Add(first, second, "Feet");

            Assert.AreEqual(5.0, result.Value, 0.001);
            Assert.AreEqual("Feet", result.UnitName);
        }


        //Test service throws exception for temperature addition
        [TestMethod]
        public void TestService_Add_UnsupportedOperation_Error()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityDTO first = new QuantityDTO(100.0, "Celsius", "Temperature");
            QuantityDTO second = new QuantityDTO(50.0, "Celsius", "Temperature");

            Assert.Throws<QuantityMeasurementException>(() =>
                service.Add(first, second, "Celsius"));
        }


        //Test service subtracts two quantities correctly
        [TestMethod]
        public void TestService_Subtract_Success()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityDTO first = new QuantityDTO(5.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(2.0, "Feet", "Length");

            QuantityDTO result = service.Subtract(first, second, "Feet");

            Assert.AreEqual(3.0, result.Value, 0.001);
            Assert.AreEqual("Feet", result.UnitName);
        }


        //Test service divides two quantities and returns scalar
        [TestMethod]
        public void TestService_Divide_Success()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityDTO first = new QuantityDTO(6.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(2.0, "Feet", "Length");

            double result = service.Divide(first, second);

            Assert.AreEqual(3.0, result, 0.001);
        }


        //Test service throws exception when dividing by zero
        [TestMethod]
        public void TestService_Divide_ByZero_Error()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityDTO first = new QuantityDTO(6.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(0.0, "Feet", "Length");

            Assert.Throws<QuantityMeasurementException>(() =>
                service.Divide(first, second));
        }


        // ─── CONTROLLER TESTS ─────────────────────────────────────────────────

        //Test controller prints equal result for equal quantities
        [TestMethod]
        public void TestController_DemonstrateEquality_Success()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            QuantityDTO first = new QuantityDTO(1.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(12.0, "Inch", "Length");

            controller.PerformLengthComparison(first, second);

            Assert.IsTrue(consoleOutput!.ToString().Contains("Equal"));
        }


        //Test controller prints conversion result correctly
        [TestMethod]
        public void TestController_DemonstrateConversion_Success()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            QuantityDTO quantity = new QuantityDTO(1.0, "Feet", "Length");

            controller.PerformLengthConversion(quantity, "Inch");

            Assert.IsTrue(consoleOutput!.ToString().Contains("12"));
        }


        //Test controller prints addition result correctly
        [TestMethod]
        public void TestController_DemonstrateAddition_Success()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            QuantityDTO first = new QuantityDTO(2.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(3.0, "Feet", "Length");

            controller.PerformLengthAddition(first, second, "Feet");

            Assert.IsTrue(consoleOutput!.ToString().Contains("5"));
        }


        //Test controller prints error for unsupported temperature arithmetic
        [TestMethod]
        public void TestController_DemonstrateAddition_Error()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            QuantityDTO first = new QuantityDTO(100.0, "Celsius", "Temperature");
            QuantityDTO second = new QuantityDTO(50.0, "Celsius", "Temperature");

            controller.PerformTemperatureArithmetic(first, second, "Celsius");

            Assert.IsTrue(consoleOutput!.ToString().Contains("Error"));
        }


        //Test controller formats successful conversion with equals sign and value
        [TestMethod]
        public void TestController_DisplayResult_Success()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            QuantityDTO quantity = new QuantityDTO(1.0, "Kilogram", "Weight");

            controller.PerformWeightConversion(quantity, "Gram");

            string output = consoleOutput!.ToString();
            Assert.IsTrue(output.Contains("="));
            Assert.IsTrue(output.Contains("1000"));
        }


        //Test controller prints not equal for cross category comparison
        [TestMethod]
        public void TestController_DisplayResult_Error()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            QuantityDTO volume = new QuantityDTO(1.0, "Litre", "Volume");
            QuantityDTO weight = new QuantityDTO(1.0, "Kilogram", "Weight");

            controller.PerformVolumeComparison(volume, weight);

            Assert.IsTrue(consoleOutput!.ToString().Contains("Not Equal"));
        }


        // ─── LAYER SEPARATION TESTS ───────────────────────────────────────────

        //Test service can be tested independently without controller
        [TestMethod]
        public void TestLayerSeparation_ServiceIndependence()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityDTO first = new QuantityDTO(1.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(1.0, "Feet", "Length");

            bool result = service.Compare(first, second);

            Assert.IsTrue(result);
        }


        //Test controller works with mock service proving dependency injection
        [TestMethod]
        public void TestLayerSeparation_ControllerIndependence()
        {
            MockService mockService = new MockService();
            mockService.CompareResult = true;

            QuantityMeasurementController controller =
                new QuantityMeasurementController(mockService);

            QuantityDTO first = new QuantityDTO(1.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(1.0, "Feet", "Length");

            controller.PerformLengthComparison(first, second);

            Assert.IsTrue(consoleOutput!.ToString().Contains("Equal"));
        }


        // ─── DATA FLOW TESTS ──────────────────────────────────────────────────

        //Test data flows correctly from controller input to service
        [TestMethod]
        public void TestDataFlow_ControllerToService()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            QuantityDTO first = new QuantityDTO(2.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(3.0, "Feet", "Length");

            controller.PerformLengthAddition(first, second, "Feet");

            Assert.IsNotNull(repo.LastSaved);
            Assert.AreEqual("Add", repo.LastSaved.Operation);
        }


        //Test result flows correctly from service back to controller output
        [TestMethod]
        public void TestDataFlow_ServiceToController()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            QuantityDTO quantity = new QuantityDTO(1.0, "Feet", "Length");

            controller.PerformLengthConversion(quantity, "Inch");

            Assert.IsTrue(consoleOutput!.ToString().Contains("12"));
        }


        // ─── BACKWARD COMPATIBILITY TESTS ────────────────────────────────────

        //Test service works with all measurement categories from previous UCs
        [TestMethod]
        public void TestBackwardCompatibility_AllUC1_UC14_Tests()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            // Length — UC1
            QuantityDTO lengthFirst = new QuantityDTO(1.0, "Feet", "Length");
            QuantityDTO lengthSecond = new QuantityDTO(12.0, "Inch", "Length");
            Assert.IsTrue(service.Compare(lengthFirst, lengthSecond));

            // Weight — UC5
            QuantityDTO weightFirst = new QuantityDTO(1.0, "Kilogram", "Weight");
            QuantityDTO weightSecond = new QuantityDTO(1000.0, "Gram", "Weight");
            Assert.IsTrue(service.Compare(weightFirst, weightSecond));

            // Volume — UC9
            QuantityDTO volumeFirst = new QuantityDTO(1.0, "Litre", "Volume");
            QuantityDTO volumeSecond = new QuantityDTO(1000.0, "Millilitre", "Volume");
            Assert.IsTrue(service.Compare(volumeFirst, volumeSecond));

            // Temperature — UC14
            QuantityDTO tempFirst = new QuantityDTO(0.0, "Celsius", "Temperature");
            QuantityDTO tempSecond = new QuantityDTO(32.0, "Fahrenheit", "Temperature");
            Assert.IsTrue(service.Compare(tempFirst, tempSecond));
        }


        //Test service works correctly with all measurement categories
        [TestMethod]
        public void TestService_AllMeasurementCategories()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            // Length
            QuantityDTO length = service.Convert(
                new QuantityDTO(1.0, "Feet", "Length"), "Inch");
            Assert.AreEqual(12.0, length.Value, 0.001);

            // Weight
            QuantityDTO weight = service.Convert(
                new QuantityDTO(1.0, "Kilogram", "Weight"), "Gram");
            Assert.AreEqual(1000.0, weight.Value, 0.001);

            // Volume
            QuantityDTO volume = service.Convert(
                new QuantityDTO(1.0, "Litre", "Volume"), "Millilitre");
            Assert.AreEqual(1000.0, volume.Value, 0.001);

            // Temperature
            QuantityDTO temp = service.Convert(
                new QuantityDTO(0.0, "Celsius", "Temperature"), "Fahrenheit");
            Assert.AreEqual(32.0, temp.Value, 0.001);
        }


        //Test controller can perform all available operations
        [TestMethod]
        public void TestController_AllOperations()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            QuantityDTO first = new QuantityDTO(4.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(2.0, "Feet", "Length");

            // Compare
            controller.PerformLengthComparison(first, second);

            // Convert
            controller.PerformLengthConversion(first, "Inch");

            // Add
            controller.PerformLengthAddition(first, second, "Feet");

            // Subtract
            controller.PerformLengthSubtraction(first, second, "Feet");

            // Divide
            controller.PerformLengthDivision(first, second);

            string output = consoleOutput!.ToString();
            Assert.IsTrue(output.Contains("Equal") || output.Contains("Not Equal"));
            Assert.IsTrue(output.Contains("="));
        }


        //Test service gives consistent validation errors across operations
        [TestMethod]
        public void TestService_ValidationConsistency()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityDTO temp1 = new QuantityDTO(100.0, "Celsius", "Temperature");
            QuantityDTO temp2 = new QuantityDTO(50.0, "Celsius", "Temperature");

            // All temperature arithmetic operations must throw same exception type
            bool addThrew = false;
            bool subtractThrew = false;

            try { service.Add(temp1, temp2, "Celsius"); }
            catch (QuantityMeasurementException) { addThrew = true; }

            try { service.Subtract(temp1, temp2, "Celsius"); }
            catch (QuantityMeasurementException) { subtractThrew = true; }

            Assert.IsTrue(addThrew);
            Assert.IsTrue(subtractThrew);
        }


        //Test entity fields cannot be changed after construction
        [TestMethod]
        public void TestEntity_Immutability()
        {
            QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                "Add", 2.0, "Feet", 3.0, "Feet", 5.0);

            // Verify values are set correctly at construction
            Assert.AreEqual("Add", entity.Operation);
            Assert.AreEqual(2.0, entity.FirstValue);
            Assert.AreEqual(5.0, entity.ResultValue);

            // Verify entity correctly represents the operation
            Assert.IsFalse(entity.IsError);
            Assert.IsNull(entity.ErrorMessage);
        }


        //Test all operations convert exceptions to error entities in repository
        [TestMethod]
        public void TestService_ExceptionHandling_AllOperations()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityDTO temp1 = new QuantityDTO(100.0, "Celsius", "Temperature");
            QuantityDTO temp2 = new QuantityDTO(50.0, "Celsius", "Temperature");

            // Add — should save error entity
            try { service.Add(temp1, temp2, "Celsius"); }
            catch (QuantityMeasurementException) { }
            Assert.IsTrue(repo.LastSaved!.IsError);

            // Subtract — should save error entity
            try { service.Subtract(temp1, temp2, "Celsius"); }
            catch (QuantityMeasurementException) { }
            Assert.IsTrue(repo.LastSaved.IsError);
        }


        //Test console output contains readable format for user
        [TestMethod]
        public void TestController_ConsoleOutput_Format()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            QuantityDTO quantity = new QuantityDTO(1.0, "Feet", "Length");

            controller.PerformLengthConversion(quantity, "Inch");

            string output = consoleOutput!.ToString();

            // Output must contain equals sign for readability
            Assert.IsTrue(output.Contains("="));

            // Output must contain unit names
            Assert.IsTrue(output.Contains("Feet") || output.Contains("Inch"));
        }


        // ─── INTEGRATION TESTS ────────────────────────────────────────────────

        //Test full flow from input to output for length addition
        [TestMethod]
        public void TestIntegration_EndToEnd_LengthAddition()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            QuantityDTO first = new QuantityDTO(2.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(3.0, "Feet", "Length");

            controller.PerformLengthAddition(first, second, "Feet");

            // Verify output shown to user
            Assert.IsTrue(consoleOutput!.ToString().Contains("5"));

            // Verify entity saved to repository
            Assert.IsNotNull(repo.LastSaved);
            Assert.AreEqual("Add", repo.LastSaved.Operation);
            Assert.AreEqual(5.0, repo.LastSaved.ResultValue, 0.001);
        }


        //Test full error flow for temperature arithmetic across all layers
        [TestMethod]
        public void TestIntegration_EndToEnd_TemperatureUnsupported()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            QuantityDTO first = new QuantityDTO(100.0, "Celsius", "Temperature");
            QuantityDTO second = new QuantityDTO(50.0, "Celsius", "Temperature");

            controller.PerformTemperatureArithmetic(first, second, "Celsius");

            // Verify error shown to user
            Assert.IsTrue(consoleOutput!.ToString().Contains("Error"));

            // Verify error entity saved to repository
            Assert.IsNotNull(repo.LastSaved);
            Assert.IsTrue(repo.LastSaved.IsError);
        }


        //Test service throws exception when null DTO is passed
        [TestMethod]
        public void TestService_NullEntity_Rejection()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            bool exceptionThrown = false;

            try
            {
                service.Compare(null, new QuantityDTO(1.0, "Feet", "Length"));
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }


        //Test controller requires non null service to function correctly
        [TestMethod]
        public void TestController_NullService_Prevention()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            QuantityMeasurementController controller =
                new QuantityMeasurementController(service);

            Assert.IsNotNull(controller);
        }


        //Test service works correctly with all unit implementations
        [TestMethod]
        public void TestService_AllUnitImplementations()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            // All length units
            QuantityDTO feet = service.Convert(new QuantityDTO(1.0, "Feet", "Length"), "Inch");
            QuantityDTO yard = service.Convert(new QuantityDTO(1.0, "Yard", "Length"), "Feet");
            QuantityDTO centimeter = service.Convert(new QuantityDTO(1.0, "Centimeter", "Length"), "Feet");

            Assert.AreEqual(12.0, feet.Value, 0.001);
            Assert.AreEqual(3.0, yard.Value, 0.001);
            Assert.AreEqual(0.0328084, centimeter.Value, 0.001);

            // All weight units
            QuantityDTO gram = service.Convert(new QuantityDTO(1.0, "Kilogram", "Weight"), "Gram");
            QuantityDTO pound = service.Convert(new QuantityDTO(1.0, "Kilogram", "Weight"), "Pound");

            Assert.AreEqual(1000.0, gram.Value, 0.001);
            Assert.AreEqual(2.20462, pound.Value, 0.001);
        }


        //Test operation type is correctly recorded in entity
        [TestMethod]
        public void TestEntity_OperationType_Tracking()
        {
            QuantityMeasurementEntity addEntity =
                new QuantityMeasurementEntity("Add", 1.0, "Feet", 2.0, "Feet", 3.0);
            QuantityMeasurementEntity subEntity =
                new QuantityMeasurementEntity("Subtract", 5.0, "Feet", 2.0, "Feet", 3.0);
            QuantityMeasurementEntity divEntity =
                new QuantityMeasurementEntity("Divide", 6.0, "Feet", 2.0, "Feet", 3.0);
            QuantityMeasurementEntity convEntity =
                new QuantityMeasurementEntity("Convert", 12.0, "Inch", 1.0);

            Assert.AreEqual("Add", addEntity.Operation);
            Assert.AreEqual("Subtract", subEntity.Operation);
            Assert.AreEqual("Divide", divEntity.Operation);
            Assert.AreEqual("Convert", convEntity.Operation);
        }


        //Test changing service implementation does not affect controller behavior
        [TestMethod]
        public void TestLayerDecoupling_ServiceChange()
        {
            // Use mock service instead of real service
            MockService mockService = new MockService();
            mockService.CompareResult = true;

            QuantityMeasurementController controller =
                new QuantityMeasurementController(mockService);

            QuantityDTO first = new QuantityDTO(1.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(1.0, "Feet", "Length");

            controller.PerformLengthComparison(first, second);

            // Controller still works correctly with different service
            Assert.IsTrue(consoleOutput!.ToString().Contains("Equal"));
        }


        //Test adding new entity fields does not break existing layer contracts
        [TestMethod]
        public void TestLayerDecoupling_EntityChange()
        {
            QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                "Add", 2.0, "Feet", 3.0, "Feet", 5.0);

            // Existing fields still work correctly
            Assert.AreEqual("Add", entity.Operation);
            Assert.AreEqual(2.0, entity.FirstValue);
            Assert.AreEqual("Feet", entity.FirstUnit);
            Assert.AreEqual(3.0, entity.SecondValue);
            Assert.AreEqual("Feet", entity.SecondUnit);
            Assert.AreEqual(5.0, entity.ResultValue);
            Assert.IsFalse(entity.IsError);
        }


        //Test adding new operation to service does not require changes in controller
        [TestMethod]
        public void TestScalability_NewOperation_Addition()
        {
            MockRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);
            QuantityMeasurementController controller = new QuantityMeasurementController(service);

            // Subtraction is an additional operation added in later UC
            // Controller calls it without knowing implementation details
            QuantityDTO first = new QuantityDTO(5.0, "Feet", "Length");
            QuantityDTO second = new QuantityDTO(2.0, "Feet", "Length");

            controller.PerformLengthSubtraction(first, second, "Feet");

            Assert.IsTrue(consoleOutput!.ToString().Contains("3"));
        }
    }
}