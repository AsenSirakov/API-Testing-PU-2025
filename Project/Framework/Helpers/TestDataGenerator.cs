using FinalProject.Framework.Models;

namespace FinalProject.Framework.Helpers
{
    public static class TestDataGenerator
    {
        private static readonly Random Random = new();
        private static readonly List<string> FirstNames = new()
        {
            "John", "Jane", "Michael", "Sarah", "David", "Emily", "Robert", "Jessica",
            "William", "Ashley", "James", "Amanda", "Charles", "Stephanie", "Thomas", "Melissa"
        };

        private static readonly List<string> LastNames = new()
        {
            "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis",
            "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas"
        };

        private static readonly List<string> Genders = new() { "male", "female" };
        private static readonly List<string> Statuses = new() { "active", "inactive" };

        public static CreateUserRequest GenerateUser()
        {
            var firstName = FirstNames[Random.Next(FirstNames.Count)];
            var lastName = LastNames[Random.Next(LastNames.Count)];
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return new CreateUserRequest
            {
                Name = $"{firstName} {lastName}",
                Email = $"{firstName.ToLower()}.{lastName.ToLower()}.{timestamp}@example.com",
                Gender = Genders[Random.Next(Genders.Count)],
                Status = Statuses[Random.Next(Statuses.Count)]
            };
        }

        public static UpdateUserRequest GenerateUpdateUser(string[]? fieldsToUpdate = null)
        {
            fieldsToUpdate ??= new[] { "name", "email", "gender", "status" };
            var updateUser = new UpdateUserRequest();

            foreach (var field in fieldsToUpdate)
            {
                switch (field.ToLower())
                {
                    case "name":
                        var firstName = FirstNames[Random.Next(FirstNames.Count)];
                        var lastName = LastNames[Random.Next(LastNames.Count)];
                        updateUser.Name = $"{firstName} {lastName}";
                        break;
                    case "email":
                        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        updateUser.Email = $"updated{timestamp}@example.com";
                        break;
                    case "gender":
                        updateUser.Gender = Genders[Random.Next(Genders.Count)];
                        break;
                    case "status":
                        updateUser.Status = Statuses[Random.Next(Statuses.Count)];
                        break;
                }
            }

            return updateUser;
        }
    }
}