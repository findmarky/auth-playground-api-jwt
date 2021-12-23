namespace Auth.Playground.API
{
    public class JwtTokenSettings
    {
        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpiryInMinutes { get; set; }
    }
}
