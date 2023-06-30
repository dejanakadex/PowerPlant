namespace PowerPlant.Infrastructure
{
    public static class PasswordHashingHelper
    {
        public static (string passwordHash, string salt) HashPassword(this string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return new(passwordHash, salt);
        }

        public static bool ArePasswordsEqual(string enteredPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }
    }
}
