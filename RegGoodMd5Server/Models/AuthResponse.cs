namespace RegGoodMd5.Server.Models
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }

    }
}
