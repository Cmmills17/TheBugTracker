namespace TheBugTracker.Client.Models
{
    // Add properties to this class and update the server and client AuthenticationStateProviders
    // to expose more information about the authenticated user to the client.
    public class UserInfo
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public string FullName => $"{Firstname} {Lastname}";
        public required string ProfilePictureUrl { get; set; }
    }
}
