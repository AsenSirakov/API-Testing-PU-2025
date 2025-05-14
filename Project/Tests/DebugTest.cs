using FinalProject.Framework.Models;
using FinalProject.Framework.Helpers;
using System.Net;
using System.Text.Json;

namespace FinalProject.Tests
{
    [TestFixture]
    public class DebugTests : TestBase
    {
        [Test]
        public async Task Debug_CheckAPIToken()
        {
            var response = await UserService.GetAllUsersAsync();
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET Users Status: {response.StatusCode}");
            Console.WriteLine($"Response Length: {responseContent.Length}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("✅ Basic connectivity works");
            }
            else
            {
                Console.WriteLine($"❌ Basic connectivity failed: {responseContent}");
            }
        }

        [Test]
        public async Task Debug_CreateUserWithDetails()
        {
            var newUser = TestDataGenerator.GenerateUser();

            Console.WriteLine($"Creating user:");
            Console.WriteLine($"Name: {newUser.Name}");
            Console.WriteLine($"Email: {newUser.Email}");
            Console.WriteLine($"Gender: {newUser.Gender}");
            Console.WriteLine($"Status: {newUser.Status}");

            // Log the JSON being sent
            var json = System.Text.Json.JsonSerializer.Serialize(newUser);
            Console.WriteLine($"JSON being sent: {json}");

            var response = await UserService.CreateUserAsync(newUser);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"\nResponse Status: {response.StatusCode}");
            Console.WriteLine($"Response Content: {responseContent}");

            if (response.StatusCode == HttpStatusCode.Created)
            {
                Console.WriteLine("✅ SUCCESS: User created successfully!");
                var createdUser = JsonSerializer.Deserialize<UserResponse>(responseContent);
                Console.WriteLine($"Created user ID: {createdUser?.Id}");
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("❌ ERROR: Unauthorized - Check your API token!");
            }
            else if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                Console.WriteLine("❌ ERROR: Unprocessable Entity - Check request format or token permissions");

                try
                {
                    var errors = JsonSerializer.Deserialize<List<ErrorResponse>>(responseContent);
                    Console.WriteLine("Error details:");
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"  {error.field}: {error.message}");
                    }
                }
                catch
                {
                    Console.WriteLine("Could not parse error response");
                }
            }
        }
    }
}