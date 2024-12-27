namespace UserAPI.Models
{
    public class RequestUpdateUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public bool Active { get; set; } = true;

    }
}
