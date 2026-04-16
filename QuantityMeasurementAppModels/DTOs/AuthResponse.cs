namespace QuantityMeasurementAppModels.DTOs
{
    public class AuthResponse
    {
        public string Token     { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public int    ExpiresIn { get; set; }
        public long   UserId    { get; set; }
        public string Email     { get; set; } = string.Empty;
        public string Name      { get; set; } = string.Empty;
        public string IssuedAt  { get; set; } = string.Empty;
        public string Role      { get; set; } = "User";    // NEW
    }
}
