namespace QuantityMeasurementAppServices.Services
{
    // Class that Handles BCrypt and SHA-256 hashing
    public class HashingService
    {
        // Method to Hash plain text using BCrypt
        public string Hash(string plainText)
        {
            // Work factor 11 = 2048 rounds, good balance of speed and security
            return BCrypt.Net.BCrypt.HashPassword(plainText, workFactor: 11);
        }

        // Method to Verify plain text against a stored BCrypt hash
        public bool Verify(string plainText, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(plainText, storedHash);
        }

        // Method to Hash input using SHA-256 and return lowercase hex string
        public string HashSha256(string input)
        {
            using (System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashBytes  = sha.ComputeHash(inputBytes);
                return System.Convert.ToHexString(hashBytes).ToLower();
            }
        }
    }
}