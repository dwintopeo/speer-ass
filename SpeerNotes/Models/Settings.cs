namespace SpeerNotes.Models
{
    public class JwtSetting
    {
        public required string Key { get; set; }
        public required string Issuer { get; set; }
        public int ExpiryMinutes { get; set; }
    }


    public class RateLimitSetting
    {
        public int PermitLimit { get; set; }
        public int QueueLimit { get; set; }
        public int Window { get; set; }
    }

}
