using System.Text.Json.Serialization;

namespace PowerPlant.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public required string Username { get; set; }

        [JsonIgnore]
        public required string PasswordHash { get; set; }

        [JsonIgnore]
        public required string Salt { get; set; }
    }
}
