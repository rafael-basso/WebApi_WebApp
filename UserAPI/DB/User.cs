namespace UserAPI.DB
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public bool Active { get; set; } = true;
        public DateTime? UpdatedAt { get; set; }
    }
}
