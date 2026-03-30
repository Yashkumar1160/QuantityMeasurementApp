using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppServices.Services;

namespace QuantityMeasurementApp.Tests
{
    // UC-18 Security Tests
    [TestClass]
    public class UC18SecurityTests
    {

        // Helper methods

        // Build fake config
        private IConfiguration BuildConfig()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("Jwt:SecretKey",     "btOUxwrOAwpVRLmc*#RfL8sdI#vYAZlgRuvO%qpRzxFHKq1V!");
            values.Add("Jwt:Issuer",        "QuantityMeasurementAPI");
            values.Add("Jwt:Audience",      "QuantityMeasurementClient");
            values.Add("Jwt:ExpiryMinutes", "480");
            values.Add("Encryption:Key",    "QMApp@AES256EncryptionKey#2026!XY");

            return new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();
        }

        // Build dummy user
        private UserEntity BuildUser()
        {
            UserEntity user = new UserEntity
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };
            return user;
        }


        // JWT tests

        // Test token is not null or empty
        [TestMethod]
        public void JwtService_GenerateToken_ReturnsNonEmptyString()
        {
            IConfiguration config = BuildConfig();
            JwtService jwtService = new JwtService(config);
            UserEntity user       = BuildUser();

            string token = jwtService.GenerateToken(user);

            Assert.IsNotNull(token);
            Assert.AreNotEqual("", token);
        }

        // Test token has three parts
        [TestMethod]
        public void JwtService_GenerateToken_HasThreeParts()
        {
            IConfiguration config = BuildConfig();
            JwtService jwtService = new JwtService(config);
            UserEntity user       = BuildUser();

            string token   = jwtService.GenerateToken(user);
            string[] parts = token.Split('.');

            Assert.AreEqual(3, parts.Length);
        }

        // Test token contains email claim
        [TestMethod]
        public void JwtService_GenerateToken_ContainsEmailClaim()
        {
            IConfiguration config = BuildConfig();
            JwtService jwtService = new JwtService(config);
            UserEntity user       = BuildUser();

            string token = jwtService.GenerateToken(user);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken parsed         = handler.ReadJwtToken(token);
            string email                    = parsed.Payload[JwtRegisteredClaimNames.Email].ToString();

            Assert.AreEqual("test@example.com", email);
        }

        // Test token contains userId claim
        [TestMethod]
        public void JwtService_GenerateToken_ContainsUserIdClaim()
        {
            IConfiguration config = BuildConfig();
            JwtService jwtService = new JwtService(config);
            UserEntity user       = BuildUser();

            string token = jwtService.GenerateToken(user);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken parsed         = handler.ReadJwtToken(token);
            string userId                   = parsed.Payload["userId"].ToString();

            Assert.AreEqual("1", userId);
        }

        // Test token has correct issuer
        [TestMethod]
        public void JwtService_GenerateToken_HasCorrectIssuer()
        {
            IConfiguration config = BuildConfig();
            JwtService jwtService = new JwtService(config);
            UserEntity user       = BuildUser();

            string token = jwtService.GenerateToken(user);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken parsed         = handler.ReadJwtToken(token);

            Assert.AreEqual("QuantityMeasurementAPI", parsed.Issuer);
        }

        // Test token has correct audience
        [TestMethod]
        public void JwtService_GenerateToken_HasCorrectAudience()
        {
            IConfiguration config = BuildConfig();
            JwtService jwtService = new JwtService(config);
            UserEntity user       = BuildUser();

            string token = jwtService.GenerateToken(user);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken parsed         = handler.ReadJwtToken(token);
            List<string> audiences          = new List<string>(parsed.Audiences);

            Assert.AreEqual("QuantityMeasurementClient", audiences[0]);
        }

        // Test token contains Jti claim
        [TestMethod]
        public void JwtService_GenerateToken_ContainsJtiClaim()
        {
            IConfiguration config = BuildConfig();
            JwtService jwtService = new JwtService(config);
            UserEntity user       = BuildUser();

            string token = jwtService.GenerateToken(user);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken parsed         = handler.ReadJwtToken(token);
            string jti                      = parsed.Payload[JwtRegisteredClaimNames.Jti].ToString();

            Assert.IsNotNull(jti);
            Assert.AreNotEqual("", jti);
        }

        // Test each token has unique Jti
        [TestMethod]
        public void JwtService_GenerateToken_EachTokenHasUniqueJti()
        {
            IConfiguration config = BuildConfig();
            JwtService jwtService = new JwtService(config);
            UserEntity user       = BuildUser();

            string token1 = jwtService.GenerateToken(user);
            string token2 = jwtService.GenerateToken(user);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string jti1 = handler.ReadJwtToken(token1).Payload[JwtRegisteredClaimNames.Jti].ToString();
            string jti2 = handler.ReadJwtToken(token2).Payload[JwtRegisteredClaimNames.Jti].ToString();

            Assert.AreNotEqual(jti1, jti2);
        }

        // Test expiry seconds is correct
        [TestMethod]
        public void JwtService_GetExpirySeconds_ReturnsCorrectValue()
        {
            IConfiguration config = BuildConfig();
            JwtService jwtService = new JwtService(config);

            int seconds = jwtService.GetExpirySeconds();

            Assert.AreEqual(28800, seconds);
        }


        // Auth tests

        // Test register returns token
        [TestMethod]
        public void AuthService_Register_ReturnsToken()
        {
            IConfiguration config   = BuildConfig();
            JwtService jwtService   = new JwtService(config);
            IUserRepository repo    = new FakeUserRepository();
            AuthService authService = new AuthService(repo, jwtService);

            RegisterRequest request = new RegisterRequest
            {
                Name = "Yash Kumar",
                Email = "yash@example.com",
                Password = "password123"
            };

            AuthResponse response = authService.Register(request);

            Assert.IsNotNull(response.Token);
            Assert.AreNotEqual("", response.Token);
        }

        // Test register returns Bearer token type
        [TestMethod]
        public void AuthService_Register_ReturnsTokenTypeBearer()
        {
            IConfiguration config   = BuildConfig();
            JwtService jwtService   = new JwtService(config);
            IUserRepository repo    = new FakeUserRepository();
            AuthService authService = new AuthService(repo, jwtService);

            RegisterRequest request = new RegisterRequest
            {
                Name = "Yash Kumar",
                Email = "yash@example.com",
                Password = "password123"
            };

            AuthResponse response = authService.Register(request);

            Assert.AreEqual("Bearer", response.TokenType);
        }

        // Test register response has correct email
        [TestMethod]
        public void AuthService_Register_ResponseContainsEmail()
        {
            IConfiguration config   = BuildConfig();
            JwtService jwtService   = new JwtService(config);
            IUserRepository repo    = new FakeUserRepository();
            AuthService authService = new AuthService(repo, jwtService);

            RegisterRequest request = new RegisterRequest
            {
                Name = "Yash Kumar",
                Email = "yash@example.com",
                Password = "password123"
            };

            AuthResponse response = authService.Register(request);

            Assert.AreEqual("yash@example.com", response.Email);
        }

        // Test register with duplicate email
        [TestMethod]
        public void AuthService_Register_DuplicateEmail_Throws()
        {
            IConfiguration config   = BuildConfig();
            JwtService jwtService   = new JwtService(config);
            IUserRepository repo    = new FakeUserRepository();
            AuthService authService = new AuthService(repo, jwtService);

            RegisterRequest request = new RegisterRequest
            {
                Name = "Yash Kumar",
                Email = "duplicate@example.com",
                Password = "password123"
            };

            // first register succeeds
            authService.Register(request);

            // second register throws
            Assert.Throws<InvalidOperationException>(() => authService.Register(request));
        }

        // Test login with valid credentials
        [TestMethod]
        public void AuthService_Login_ValidCredentials_ReturnsToken()
        {
            IConfiguration config   = BuildConfig();
            JwtService jwtService   = new JwtService(config);
            IUserRepository repo    = new FakeUserRepository();
            AuthService authService = new AuthService(repo, jwtService);

            RegisterRequest registerRequest = new RegisterRequest
            {
                Name = "Yash Kumar",
                Email = "yash@example.com",
                Password = "password123"
            };
            authService.Register(registerRequest);

            LoginRequest loginRequest = new LoginRequest
            {
                Email = "yash@example.com",
                Password = "password123"
            };

            AuthResponse response = authService.Login(loginRequest);

            Assert.IsNotNull(response.Token);
            Assert.AreNotEqual("", response.Token);
        }

        // Test login with wrong password
        [TestMethod]
        public void AuthService_Login_WrongPassword_Throws()
        {
            IConfiguration config   = BuildConfig();
            JwtService jwtService   = new JwtService(config);
            IUserRepository repo    = new FakeUserRepository();
            AuthService authService = new AuthService(repo, jwtService);

            RegisterRequest registerRequest = new RegisterRequest
            {
                Name = "Yash Kumar",
                Email = "yash@example.com",
                Password = "password123"
            };
            authService.Register(registerRequest);

            LoginRequest loginRequest = new LoginRequest
            {
                Email = "yash@example.com",
                Password = "wrongpassword"
            };

            Assert.Throws<UnauthorizedAccessException>(() => authService.Login(loginRequest));
        }

        // Test login with email not registered
        [TestMethod]
        public void AuthService_Login_EmailNotFound_Throws()
        {
            IConfiguration config   = BuildConfig();
            JwtService jwtService   = new JwtService(config);
            IUserRepository repo    = new FakeUserRepository();
            AuthService authService = new AuthService(repo, jwtService);

            LoginRequest loginRequest = new LoginRequest
            {
                Email = "notregistered@example.com",
                Password = "password123"
            };

            Assert.Throws<UnauthorizedAccessException>(() => authService.Login(loginRequest));
        }

        // Test login response ExpiresIn matches config
        [TestMethod]
        public void AuthService_Login_ExpiresInMatchesConfig()
        {
            IConfiguration config   = BuildConfig();
            JwtService jwtService   = new JwtService(config);
            IUserRepository repo    = new FakeUserRepository();
            AuthService authService = new AuthService(repo, jwtService);

            RegisterRequest registerRequest = new RegisterRequest
            {
                Name = "Yash Kumar",
                Email = "yash@example.com",
                Password = "password123"
            };
            authService.Register(registerRequest);

            LoginRequest loginRequest = new LoginRequest
            {
                Email = "yash@example.com",
                Password = "password123"
            };

            AuthResponse response = authService.Login(loginRequest);

            Assert.AreEqual(28800, response.ExpiresIn);
        }


        // Encryption tests

        // Test encrypt returns non empty string
        [TestMethod]
        public void EncryptionService_Encrypt_ReturnsNonEmptyString()
        {
            IConfiguration config               = BuildConfig();
            EncryptionService encryptionService = new EncryptionService(config);

            string result = encryptionService.Encrypt("hello world");

            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
        }

        // Test encrypted output differs from input
        [TestMethod]
        public void EncryptionService_Encrypt_OutputDiffersFromInput()
        {
            IConfiguration config               = BuildConfig();
            EncryptionService encryptionService = new EncryptionService(config);

            string plainText = "hello world";
            string encrypted = encryptionService.Encrypt(plainText);

            Assert.AreNotEqual(plainText, encrypted);
        }

        // Test decrypt returns original text
        [TestMethod]
        public void EncryptionService_Decrypt_ReturnsOriginalText()
        {
            IConfiguration config               = BuildConfig();
            EncryptionService encryptionService = new EncryptionService(config);

            string plainText = "hello world";
            string encrypted = encryptionService.Encrypt(plainText);
            string decrypted = encryptionService.Decrypt(encrypted);

            Assert.AreEqual(plainText, decrypted);
        }

        // Test same input gives different output each time
        [TestMethod]
        public void EncryptionService_Encrypt_SameInputDifferentOutput()
        {
            IConfiguration config               = BuildConfig();
            EncryptionService encryptionService = new EncryptionService(config);

            string plainText  = "hello world";
            string encrypted1 = encryptionService.Encrypt(plainText);
            string encrypted2 = encryptionService.Encrypt(plainText);

            Assert.AreNotEqual(encrypted1, encrypted2);
        }

        // Test encrypted output is valid Base64
        [TestMethod]
        public void EncryptionService_Encrypt_OutputIsBase64()
        {
            IConfiguration config               = BuildConfig();
            EncryptionService encryptionService = new EncryptionService(config);

            string encrypted = encryptionService.Encrypt("test data");
            byte[] bytes     = Convert.FromBase64String(encrypted);

            Assert.IsTrue(bytes.Length > 0);
        }

        // Test empty string round trip
        [TestMethod]
        public void EncryptionService_Encrypt_EmptyString_RoundTrip()
        {
            IConfiguration config               = BuildConfig();
            EncryptionService encryptionService = new EncryptionService(config);

            string encrypted = encryptionService.Encrypt("");
            string decrypted = encryptionService.Decrypt(encrypted);

            Assert.AreEqual("", decrypted);
        }

        // Test long string round trip
        [TestMethod]
        public void EncryptionService_Encrypt_LongString_RoundTrip()
        {
            IConfiguration config               = BuildConfig();
            EncryptionService encryptionService = new EncryptionService(config);

            string longText  = "This is a longer string with special chars: @#$%^&*() 1234567890";
            string encrypted = encryptionService.Encrypt(longText);
            string decrypted = encryptionService.Decrypt(encrypted);

            Assert.AreEqual(longText, decrypted);
        }


        // Hashing tests

        // Test hash returns non empty string
        [TestMethod]
        public void HashingService_Hash_ReturnsNonEmptyString()
        {
            HashingService hashingService = new HashingService();

            string result = hashingService.Hash("mypassword");

            Assert.IsNotNull(result);
            Assert.AreNotEqual("", result);
        }

        // Test hash output differs from input
        [TestMethod]
        public void HashingService_Hash_OutputDiffersFromInput()
        {
            HashingService hashingService = new HashingService();

            string plainText = "mypassword";
            string hashed    = hashingService.Hash(plainText);

            Assert.AreNotEqual(plainText, hashed);
        }

        // Test same input gives different hash each time
        [TestMethod]
        public void HashingService_Hash_SameInputDifferentHash()
        {
            HashingService hashingService = new HashingService();

            string hash1 = hashingService.Hash("mypassword");
            string hash2 = hashingService.Hash("mypassword");

            Assert.AreNotEqual(hash1, hash2);
        }

        // Test verify with correct password
        [TestMethod]
        public void HashingService_Verify_CorrectPassword_ReturnsTrue()
        {
            HashingService hashingService = new HashingService();

            string plainText = "mypassword";
            string hashed    = hashingService.Hash(plainText);
            bool result      = hashingService.Verify(plainText, hashed);

            Assert.IsTrue(result);
        }

        // Test verify with wrong password
        [TestMethod]
        public void HashingService_Verify_WrongPassword_ReturnsFalse()
        {
            HashingService hashingService = new HashingService();

            string hashed = hashingService.Hash("mypassword");
            bool result   = hashingService.Verify("wrongpassword", hashed);

            Assert.IsFalse(result);
        }

        // Test hash starts with BCrypt prefix
        [TestMethod]
        public void HashingService_Hash_StartsWithBCryptPrefix()
        {
            HashingService hashingService = new HashingService();

            string hashed = hashingService.Hash("mypassword");

            Assert.IsTrue(hashed.StartsWith("$2a$") || hashed.StartsWith("$2b$"));
        }

    }


    // Fake repository

    // Simulates database in memory
    public class FakeUserRepository : IUserRepository
    {
        private List<UserEntity> store = new List<UserEntity>();
        private long nextId = 1;

        // Save user
        public UserEntity Save(UserEntity user)
        {
            user.Id = nextId;
            nextId  = nextId + 1;
            store.Add(user);
            return user;
        }

        // Update user
        public void Update(UserEntity user)
        {
            // not needed for these tests
        }

        // Find by email
        public UserEntity FindByEmail(string email)
        {
            foreach (UserEntity u in store)
            {
                if (u.Email == email)
                {
                    return u;
                }
            }
            return null;
        }

        // Find by id
        public UserEntity FindById(long id)
        {
            foreach (UserEntity u in store)
            {
                if (u.Id == id)
                {
                    return u;
                }
            }
            return null;
        }
    }
}