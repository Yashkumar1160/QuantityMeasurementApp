using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using QuantityMeasurementAppModels.DTOs;

namespace QuantityMeasurementWebApi.Tests
{
    // WebApi Tests
    [TestClass]
    [DoNotParallelize]
    public class QuantityMeasurementWebApiTests
    {
        // Factory object
        private static WebApplicationFactory<Program> _factory;

        // Http client
        private static HttpClient _client;

        // Json options
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Base URL
        private const string BASE_URL = "/api/v1/quantities";


        // Setup

        // Initialize objects
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        // Cleanup

        // Dispose objects
        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (_client != null)
            {
                _client.Dispose();
            }

            if (_factory != null)
            {
                _factory.Dispose();
            }
        }


        // Helper methods

        // Convert object to JSON
        private static StringContent ToJson(object obj)
        {
            string json = JsonSerializer.Serialize(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        // Create Quantity object
        private static QuantityDTO MakeQuantity(double value, string unit, string type)
        {
            QuantityDTO dto = new QuantityDTO();
            dto.Value = value;
            dto.UnitName = unit;
            dto.MeasurementType = type;
            return dto;
        }


        // Startup tests

        // Test application start
        [TestMethod]
        public async Task TestWebApiApplicationStarts()
        {
            HttpResponseMessage response = await _client.GetAsync("/health");

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        // Test server working
        [TestMethod]
        public async Task TestServerIsReachable()
        {
            HttpResponseMessage response = await _client.GetAsync("/swagger/index.html");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        // Compare tests

        // Test compare API
        [TestMethod]
        public async Task TestRestEndpointCompareQuantities()
        {
            // create request
            QuantityInputRequest request = new QuantityInputRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Feet", "Length"),
                ThatQuantityDTO = MakeQuantity(12.0, "Inch", "Length")
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/compare", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();
            QuantityMeasurementResponseDTO result = JsonSerializer.Deserialize<QuantityMeasurementResponseDTO>(body, _jsonOptions);

            // check operation
            Assert.AreEqual("Compare", result.Operation);

            // check error
            Assert.IsFalse(result.IsError);
        }


        // Test compare equal values
        [TestMethod]
        public async Task TestRestEndpointCompareQuantities_EqualResult()
        {
            // create request
            QuantityInputRequest request = new QuantityInputRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Kilogram", "Weight"),
                ThatQuantityDTO = MakeQuantity(1000.0, "Gram", "Weight")
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/compare", ToJson(request));

            // get response
            string body = await response.Content.ReadAsStringAsync();
            QuantityMeasurementResponseDTO result = JsonSerializer.Deserialize<QuantityMeasurementResponseDTO>(body, _jsonOptions);

            // check result
            Assert.AreEqual("True", result.ResultString);
        }


        // Convert tests

        // Test convert API
        [TestMethod]
        public async Task TestRestEndpointConvertQuantities()
        {
            // create request
            ConvertRequest request = new ConvertRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Feet", "Length"),
                TargetUnit = "Inch"
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/convert", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();
            QuantityMeasurementResponseDTO result = JsonSerializer.Deserialize<QuantityMeasurementResponseDTO>(body, _jsonOptions);

            // check value
            Assert.AreEqual(12.0, result.ResultValue, 0.001);

            // check unit
            Assert.AreEqual("Inch", result.ResultUnit);
        }


        // Test temperature conversion
        [TestMethod]
        public async Task TestRestEndpointConvertTemperature()
        {
            // create request
            ConvertRequest request = new ConvertRequest
            {
                ThisQuantityDTO = MakeQuantity(0.0, "Celsius", "Temperature"),
                TargetUnit = "Fahrenheit"
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/convert", ToJson(request));

            // get response
            string body = await response.Content.ReadAsStringAsync();
            QuantityMeasurementResponseDTO result = JsonSerializer.Deserialize<QuantityMeasurementResponseDTO>(body, _jsonOptions);

            // check value
            Assert.AreEqual(32.0, result.ResultValue, 0.001);
        }



        // Add tests

        // Test add API
        [TestMethod]
        public async Task TestRestEndpointAddQuantities()
        {
            // create request
            ArithmeticRequest request = new ArithmeticRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Feet", "Length"),
                ThatQuantityDTO = MakeQuantity(1.0, "Feet", "Length"),
                TargetUnit = "Feet"
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/add", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();
            QuantityMeasurementResponseDTO result = JsonSerializer.Deserialize<QuantityMeasurementResponseDTO>(body, _jsonOptions);

            // check value
            Assert.AreEqual(2.0, result.ResultValue, 0.001);

            // check operation
            Assert.AreEqual("Add", result.Operation);
        }


        // Test add with different units
        [TestMethod]
        public async Task TestRestEndpointAddQuantities_MixedUnits()
        {
            // create request
            ArithmeticRequest request = new ArithmeticRequest
            {
                ThisQuantityDTO = MakeQuantity(12.0, "Inch", "Length"),
                ThatQuantityDTO = MakeQuantity(1.0, "Feet", "Length"),
                TargetUnit = "Feet"
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/add", ToJson(request));

            // get response
            string body = await response.Content.ReadAsStringAsync();
            QuantityMeasurementResponseDTO result = JsonSerializer.Deserialize<QuantityMeasurementResponseDTO>(body, _jsonOptions);

            // check value
            Assert.AreEqual(2.0, result.ResultValue, 0.001);
        }


        // Subtract tests

        // Test subtract API
        [TestMethod]
        public async Task TestRestEndpointSubtractQuantities()
        {
            // create request
            ArithmeticRequest request = new ArithmeticRequest
            {
                ThisQuantityDTO = MakeQuantity(2.0, "Feet", "Length"),
                ThatQuantityDTO = MakeQuantity(1.0, "Feet", "Length"),
                TargetUnit = "Feet"
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/subtract", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();
            QuantityMeasurementResponseDTO result = JsonSerializer.Deserialize<QuantityMeasurementResponseDTO>(body, _jsonOptions);

            // check value
            Assert.AreEqual(1.0, result.ResultValue, 0.001);

            // check operation
            Assert.AreEqual("Subtract", result.Operation);
        }


        // Divide tests

        // Test divide API
        [TestMethod]
        public async Task TestRestEndpointDivideQuantities()
        {
            // create request
            QuantityInputRequest request = new QuantityInputRequest
            {
                ThisQuantityDTO = MakeQuantity(2.0, "Feet", "Length"),
                ThatQuantityDTO = MakeQuantity(1.0, "Feet", "Length")
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/divide", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();
            QuantityMeasurementResponseDTO result = JsonSerializer.Deserialize<QuantityMeasurementResponseDTO>(body, _jsonOptions);

            // check value
            Assert.AreEqual(2.0, result.ResultValue, 0.001);

            // check operation
            Assert.AreEqual("Divide", result.Operation);
        }


        // Invalid input tests

        // Test invalid JSON
        [TestMethod]
        public async Task TestRestEndpointInvalidInput_Returns400()
        {
            // create bad json
            StringContent badJson = new StringContent("{ this is not valid json }", Encoding.UTF8, "application/json");

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/compare", badJson);

            // check status
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        // Test missing fields
        [TestMethod]
        public async Task TestRestEndpointMissingFields_Returns400()
        {
            // create empty body
            StringContent emptyBody = new StringContent("{}", Encoding.UTF8, "application/json");

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/add", emptyBody);

            // check status
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        // Test invalid measurement type
        [TestMethod]
        public async Task TestRestEndpointInvalidMeasurementType_Returns400()
        {
            // create request
            QuantityInputRequest request = new QuantityInputRequest
            {
                ThisQuantityDTO = new QuantityDTO { Value = 10.0, UnitName = "KmPerHour", MeasurementType = "Speed" },
                ThatQuantityDTO = MakeQuantity(5.0, "Feet", "Length")
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/compare", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        // Test invalid unit
        [TestMethod]
        public async Task TestRestEndpointInvalidUnit_Returns400()
        {
            // create request
            QuantityInputRequest request = new QuantityInputRequest
            {
                ThisQuantityDTO = new QuantityDTO { Value = 1.0, UnitName = "Gallon", MeasurementType = "Length" },
                ThatQuantityDTO = MakeQuantity(1.0, "Feet", "Length")
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/compare", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        // Test negative value
        [TestMethod]
        public async Task TestRestEndpointNegativeValue_Returns400()
        {
            // create request
            QuantityInputRequest request = new QuantityInputRequest
            {
                ThisQuantityDTO = new QuantityDTO { Value = -5.0, UnitName = "Feet", MeasurementType = "Length" },
                ThatQuantityDTO = MakeQuantity(1.0, "Feet", "Length")
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/compare", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        // Swagger tests

        // Test swagger UI
        [TestMethod]
        public async Task TestSwaggerUILoads()
        {
            // call API
            HttpResponseMessage response = await _client.GetAsync("/swagger/index.html");

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();

            // check content
            Assert.IsTrue(body.Contains("<html") || body.Contains("<!DOCTYPE"));
        }


        // Test swagger JSON
        [TestMethod]
        public async Task TestOpenAPIDocumentation()
        {
            // call API
            HttpResponseMessage response = await _client.GetAsync("/swagger/v1/swagger.json");

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();

            // check endpoints
            Assert.IsTrue(body.Contains("/api/v1/quantities/compare"));
            Assert.IsTrue(body.Contains("/api/v1/quantities/convert"));
            Assert.IsTrue(body.Contains("/api/v1/quantities/add"));
            Assert.IsTrue(body.Contains("/api/v1/quantities/subtract"));
            Assert.IsTrue(body.Contains("/api/v1/quantities/divide"));
        }


        // Health tests

        // Test health API
        [TestMethod]
        public async Task TestActuatorHealthEndpoint()
        {
            // call API
            HttpResponseMessage response = await _client.GetAsync("/health");

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();

            // check result
            Assert.IsTrue(body.Contains("Healthy") || body.Contains("UP"));
        }


        // Content tests

        // Test JSON request
        [TestMethod]
        public async Task TestContentNegotiation_JSON()
        {
            // create request
            ConvertRequest request = new ConvertRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Feet", "Length"),
                TargetUnit = "Inch"
            };

            // create content
            StringContent content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/convert", content);

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // check content type
            string contentType = response.Content.Headers.ContentType?.MediaType;
            Assert.AreEqual("application/json", contentType);
        }


        // Test response JSON
        [TestMethod]
        public async Task TestResponseSerialization_Object()
        {
            // create request
            QuantityInputRequest request = new QuantityInputRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Kilogram", "Weight"),
                ThatQuantityDTO = MakeQuantity(1000.0, "Gram", "Weight")
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/compare", ToJson(request));

            // parse JSON
            string body = await response.Content.ReadAsStringAsync();
            JsonDocument doc = JsonDocument.Parse(body);

            // check fields
            Assert.IsTrue(doc.RootElement.TryGetProperty("operation", out _));
            Assert.IsTrue(doc.RootElement.TryGetProperty("isError", out _));
            Assert.IsTrue(doc.RootElement.TryGetProperty("resultString", out _));
        }


        // Test JSON to object
        [TestMethod]
        public async Task TestMessageConverter_JSONToObject()
        {
            // create JSON
            string json = @"{
        ""thisQuantityDTO"": { ""value"": 5.0, ""unitName"": ""Litre"", ""measurementType"": ""Volume"" },
        ""targetUnit"": ""Millilitre""
    }";

            // create content
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/convert", content);

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();
            QuantityMeasurementResponseDTO result = JsonSerializer.Deserialize<QuantityMeasurementResponseDTO>(body, _jsonOptions);

            // check value
            Assert.AreEqual(5000.0, result.ResultValue, 0.01);
        }


        // Test object to JSON
        [TestMethod]
        public async Task TestMessageConverter_ObjectToJSON()
        {
            // create request
            ConvertRequest request = new ConvertRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Kilogram", "Weight"),
                TargetUnit = "Gram"
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/convert", ToJson(request));

            // parse JSON
            string body = await response.Content.ReadAsStringAsync();
            JsonDocument doc = JsonDocument.Parse(body);

            // check values
            Assert.AreEqual(1.0, doc.RootElement.GetProperty("thisValue").GetDouble(), 0.001);
            Assert.AreEqual(1000.0, doc.RootElement.GetProperty("resultValue").GetDouble(), 0.001);
            Assert.AreEqual("Gram", doc.RootElement.GetProperty("resultUnit").GetString());
        }


        // Path variable tests

        // Test path variable
        [TestMethod]
        public async Task TestRequestPathVariable_Extraction()
        {
            // call API
            HttpResponseMessage response = await _client.GetAsync(BASE_URL + "/count/Compare");

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();

            // check integer
            bool isInt = int.TryParse(body, out _);
            Assert.IsTrue(isInt);
        }

        // Path variable tests

        // Test history by operation
        [TestMethod]
        public async Task TestRequestPathVariable_HistoryByOperation()
        {
            // call API
            HttpResponseMessage response = await _client.GetAsync(BASE_URL + "/history/operation/Add");

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();
            JsonDocument doc = JsonDocument.Parse(body);

            // check array
            Assert.AreEqual(JsonValueKind.Array, doc.RootElement.ValueKind);
        }


        // Test history by type
        [TestMethod]
        public async Task TestRequestPathVariable_HistoryByType()
        {
            // call API
            HttpResponseMessage response = await _client.GetAsync(BASE_URL + "/history/type/Length");

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();
            JsonDocument doc = JsonDocument.Parse(body);

            // check array
            Assert.AreEqual(JsonValueKind.Array, doc.RootElement.ValueKind);
        }


        // HTTP status tests

        // Test success
        [TestMethod]
        public async Task TestHttpStatusCodes_Success()
        {
            // create request
            QuantityInputRequest request = new QuantityInputRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Feet", "Length"),
                ThatQuantityDTO = MakeQuantity(12.0, "Inch", "Length")
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/compare", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(200, (int)response.StatusCode);
        }


        // Test bad request
        [TestMethod]
        public async Task TestHttpStatusCodes_ClientErrors()
        {
            // create empty body
            StringContent empty = new StringContent("{}", Encoding.UTF8, "application/json");

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/compare", empty);

            // check status
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(400, (int)response.StatusCode);
        }


        // Test divide by zero
        [TestMethod]
        public async Task TestHttpStatusCodes_ServerErrors()
        {
            // create request
            QuantityInputRequest request = new QuantityInputRequest
            {
                ThisQuantityDTO = MakeQuantity(10.0, "Feet", "Length"),
                ThatQuantityDTO = MakeQuantity(0.0, "Feet", "Length")
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/divide", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        // Exception tests

        // Test global exception
        [TestMethod]
        public async Task TestExceptionHandling_GlobalHandler()
        {
            // create request
            ArithmeticRequest request = new ArithmeticRequest
            {
                ThisQuantityDTO = MakeQuantity(100.0, "Celsius", "Temperature"),
                ThatQuantityDTO = MakeQuantity(50.0, "Celsius", "Temperature"),
                TargetUnit = "Celsius"
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/add", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            // get response
            string body = await response.Content.ReadAsStringAsync();
            JsonDocument doc = JsonDocument.Parse(body);

            // check fields
            Assert.IsTrue(doc.RootElement.TryGetProperty("status", out _));
            Assert.IsTrue(doc.RootElement.TryGetProperty("error", out _));
            Assert.IsTrue(doc.RootElement.TryGetProperty("message", out _));
            Assert.IsTrue(doc.RootElement.TryGetProperty("path", out _));
            Assert.IsTrue(doc.RootElement.TryGetProperty("timestamp", out _));
        }

        // Test error response
        [TestMethod]
        public async Task TestExceptionHandling_ErrorResponseFormat()
        {
            // create request
            QuantityInputRequest request = new QuantityInputRequest
            {
                ThisQuantityDTO = new QuantityDTO { Value = 1.0, UnitName = "Gallon", MeasurementType = "Length" },
                ThatQuantityDTO = MakeQuantity(1.0, "Feet", "Length")
            };

            // call API
            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/compare", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            // check content type
            string contentType = response.Content.Headers.ContentType?.MediaType;
            Assert.AreEqual("application/json", contentType);
        }


        // Database tests

        // Test data saved
        [TestMethod]
        public async Task TestDatabasePersistence_AfterOperation()
        {
            // create request
            ArithmeticRequest request = new ArithmeticRequest
            {
                ThisQuantityDTO = MakeQuantity(2.0, "Kilogram", "Weight"),
                ThatQuantityDTO = MakeQuantity(500.0, "Gram", "Weight"),
                TargetUnit = "Kilogram"
            };

            // call API
            HttpResponseMessage opResponse = await _client.PostAsync(BASE_URL + "/add", ToJson(request));

            // check status
            Assert.AreEqual(HttpStatusCode.OK, opResponse.StatusCode);

            // get count
            HttpResponseMessage countResponse = await _client.GetAsync(BASE_URL + "/count/Add");
            string countBody = await countResponse.Content.ReadAsStringAsync();
            int count = int.Parse(countBody);

            // check value
            Assert.IsTrue(count >= 1);
        }


        // Test error saved
        [TestMethod]
        public async Task TestDatabasePersistence_ErrorRecordSaved()
        {
            // create request
            ArithmeticRequest request = new ArithmeticRequest
            {
                ThisQuantityDTO = MakeQuantity(100.0, "Celsius", "Temperature"),
                ThatQuantityDTO = MakeQuantity(50.0, "Celsius", "Temperature"),
                TargetUnit = "Celsius"
            };

            // call API
            await _client.PostAsync(BASE_URL + "/add", ToJson(request));

            // get history
            HttpResponseMessage historyResponse = await _client.GetAsync(BASE_URL + "/history/errored");

            // check status
            Assert.AreEqual(HttpStatusCode.OK, historyResponse.StatusCode);

            // check data
            string body = await historyResponse.Content.ReadAsStringAsync();
            JsonDocument doc = JsonDocument.Parse(body);
            Assert.IsTrue(doc.RootElement.GetArrayLength() >= 1);
        }


        // Repository tests

        // Test history by operation
        [TestMethod]
        public async Task TestJPARepositoryFindByOperation()
        {
            // create request
            ConvertRequest convertRequest = new ConvertRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Gallon", "Volume"),
                TargetUnit = "Litre"
            };

            // call API
            await _client.PostAsync(BASE_URL + "/convert", ToJson(convertRequest));

            // get data
            HttpResponseMessage response = await _client.GetAsync(BASE_URL + "/history/operation/Convert");

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // read data
            string body = await response.Content.ReadAsStringAsync();
            List<QuantityMeasurementResponseDTO> results =
                JsonSerializer.Deserialize<List<QuantityMeasurementResponseDTO>>(body, _jsonOptions);

            // check values
            foreach (QuantityMeasurementResponseDTO record in results)
            {
                Assert.AreEqual("Convert", record.Operation);
            }
        }


        // Test history by type
        [TestMethod]
        public async Task TestJPARepositoryCustomQuery()
        {
            // create request
            QuantityInputRequest request = new QuantityInputRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Kilogram", "Weight"),
                ThatQuantityDTO = MakeQuantity(1.0, "Kilogram", "Weight")
            };

            // call API
            await _client.PostAsync(BASE_URL + "/compare", ToJson(request));

            // get data
            HttpResponseMessage response = await _client.GetAsync(BASE_URL + "/history/type/Weight");

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // read data
            string body = await response.Content.ReadAsStringAsync();
            List<QuantityMeasurementResponseDTO> results =
                JsonSerializer.Deserialize<List<QuantityMeasurementResponseDTO>>(body, _jsonOptions);

            // check values
            foreach (QuantityMeasurementResponseDTO record in results)
            {
                Assert.AreEqual("Weight", record.ThisMeasurementType);
            }
        }


        // Test error history
        [TestMethod]
        public async Task TestJPARepositoryErrorHistory()
        {
            // call API
            HttpResponseMessage response = await _client.GetAsync(BASE_URL + "/history/errored");

            // check status
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // read data
            string body = await response.Content.ReadAsStringAsync();
            List<QuantityMeasurementResponseDTO> results =
                JsonSerializer.Deserialize<List<QuantityMeasurementResponseDTO>>(body, _jsonOptions);

            // check values
            foreach (QuantityMeasurementResponseDTO record in results)
            {
                Assert.IsTrue(record.IsError);
            }
        }


        // -------------------------------------------------------
        // Integration Tests — Multiple Operations
        // -------------------------------------------------------

        // Test multiple operations one by one
        [TestMethod]
        public async Task TestIntegration_MultipleOperations()
        {
            // Step 1 — Compare
            QuantityInputRequest compareRequest = new QuantityInputRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Feet", "Length"),
                ThatQuantityDTO = MakeQuantity(12.0, "Inch", "Length")
            };

            HttpResponseMessage compareResponse = await _client.PostAsync(BASE_URL + "/compare", ToJson(compareRequest));

            // verify 200 OK
            Assert.AreEqual(HttpStatusCode.OK, compareResponse.StatusCode);


            // Step 2 — Convert
            ConvertRequest convertRequest = new ConvertRequest
            {
                ThisQuantityDTO = MakeQuantity(3.0, "Feet", "Length"),
                TargetUnit = "Yard"
            };

            HttpResponseMessage convertResponse = await _client.PostAsync(BASE_URL + "/convert", ToJson(convertRequest));

            // verify 200 OK
            Assert.AreEqual(HttpStatusCode.OK, convertResponse.StatusCode);


            // Step 3 — Add
            ArithmeticRequest addRequest = new ArithmeticRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Kilogram", "Weight"),
                ThatQuantityDTO = MakeQuantity(500.0, "Gram", "Weight"),
                TargetUnit = "Kilogram"
            };

            HttpResponseMessage addResponse = await _client.PostAsync(BASE_URL + "/add", ToJson(addRequest));

            // verify 200 OK
            Assert.AreEqual(HttpStatusCode.OK, addResponse.StatusCode);


            // Step 4 — check count
            HttpResponseMessage countResponse = await _client.GetAsync(BASE_URL + "/count/Add");

            string countBody = await countResponse.Content.ReadAsStringAsync();
            int count = int.Parse(countBody);

            // verify at least 1 record exists
            Assert.IsTrue(count >= 1);
        }


        // -------------------------------------------------------
        // Database Initialization Test
        // -------------------------------------------------------

        // Test DB schema is working
        [TestMethod]
        public async Task TestDatabaseInitialization()
        {
            // send convert request
            ConvertRequest request = new ConvertRequest
            {
                ThisQuantityDTO = MakeQuantity(1.0, "Litre", "Volume"),
                TargetUnit = "Millilitre"
            };

            HttpResponseMessage response = await _client.PostAsync(BASE_URL + "/convert", ToJson(request));

            // verify 200 OK
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);


            // get history
            HttpResponseMessage historyResponse = await _client.GetAsync(BASE_URL + "/history/operation/Convert");

            string body = await historyResponse.Content.ReadAsStringAsync();
            List<QuantityMeasurementResponseDTO> results =
                JsonSerializer.Deserialize<List<QuantityMeasurementResponseDTO>>(body, _jsonOptions);

            // verify data exists
            Assert.IsTrue(results.Count >= 1);

            // verify fields are not null
            Assert.IsNotNull(results[0].Operation);
            Assert.IsNotNull(results[0].ThisUnit);
        }


        // -------------------------------------------------------
        // Configuration Tests
        // -------------------------------------------------------

        // Test Development config
        [TestMethod]
        public async Task TestConfig_Development()
        {
            // create scope
            IServiceScope scope = _factory.Services.CreateScope();

            IConfiguration config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            // get log level
            string logLevel = config["Logging:LogLevel:Default"];

            // verify Debug
            Assert.AreEqual("Debug", logLevel);
        }


        // Test Production config
        [TestMethod]
        public async Task TestConfig_Production()
        {
            // create new factory with custom config
            WebApplicationFactory<Program> prodFactory =
                _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((ctx, cfg) =>
                    {
                        cfg.AddInMemoryCollection(new Dictionary<string, string>
                        {
                    { "Logging:LogLevel:Default", "Warning" }
                        });
                    });
                });

            // create client
            HttpClient client = prodFactory.CreateClient();

            HttpResponseMessage response = await client.GetAsync("/health");

            // verify server runs
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);


            // check config value
            IServiceScope scope = prodFactory.Services.CreateScope();
            IConfiguration config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            string logLevel = config["Logging:LogLevel:Default"];

            // verify Warning
            Assert.AreEqual("Warning", logLevel);

            // cleanup
            client.Dispose();
            prodFactory.Dispose();
        }


        // -------------------------------------------------------
        // Swagger Documentation Test
        // -------------------------------------------------------

        // Test swagger JSON content
        [TestMethod]
        public async Task TestSwaggerDocumentation()
        {
            HttpResponseMessage response = await _client.GetAsync("/swagger/v1/swagger.json");

            // verify 200 OK
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            string body = await response.Content.ReadAsStringAsync();

            // check endpoints exist
            Assert.IsTrue(body.Contains("compare"));
            Assert.IsTrue(body.Contains("convert"));
            Assert.IsTrue(body.Contains("add"));
            Assert.IsTrue(body.Contains("subtract"));
            Assert.IsTrue(body.Contains("divide"));

            // check status codes
            Assert.IsTrue(body.Contains("200"));
            Assert.IsTrue(body.Contains("400"));
        }


        // -------------------------------------------------------
        // Security Tests (Future)
        // -------------------------------------------------------

        // Test without authentication
        [TestMethod]
        public async Task TestSecurity_WithoutAuth()
        {
            // call endpoint without token
            HttpResponseMessage response = await _client.GetAsync("/health");

            // verify response exists
            Assert.IsNotNull(response);

            // currently 200 OK expected
            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Unauthorized
            );
        }


        // Test with authentication (future use)
        [TestMethod]
        public async Task TestSecurity_WithAuth()
        {
            // TODO: add token here later

            HttpResponseMessage response = await _client.GetAsync("/health");

            // verify 200 OK
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}