namespace Notes_API_SERVICE
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public int ExpirationMinutes { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
