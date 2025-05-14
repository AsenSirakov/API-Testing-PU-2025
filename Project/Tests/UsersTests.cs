using FinalProject.Framework.Models;
using FinalProject.Framework.Helpers;
using System.Net;
using System.Text.Json;

namespace FinalProject.Tests
{
    [TestFixture]
    public class UsersTests : TestBase
    {
        private int _createdUserId;

        [Test, Order(1)]
        public async Task GetAllUsers_ShouldReturnOkStatusAndUserList()
        {
            // Act
            var response = await UserService.GetAllUsersAsync();
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().NotBeNullOrEmpty();

            var users = JsonSerializer.Deserialize<List<UserResponse>>(responseContent);
            users.Should().NotBeNull();
            users.Should().BeOfType<List<UserResponse>>();
        }

        [Test, Order(2)]
        public async Task CreateUser_ShouldReturnCreatedStatusAndUser()
        {
            // Arrange
            var newUser = TestDataGenerator.GenerateUser();

            // Act
            var response = await UserService.CreateUserAsync(newUser);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseContent.Should().NotBeNullOrEmpty();

            var createdUser = JsonSerializer.Deserialize<UserResponse>(responseContent);
            createdUser.Should().NotBeNull();
            createdUser!.Id.Should().BeGreaterThan(0);
            createdUser.Name.Should().Be(newUser.Name);
            createdUser.Email.Should().Be(newUser.Email);
            createdUser.Gender.Should().Be(newUser.Gender);
            createdUser.Status.Should().Be(newUser.Status);

            // Store for later tests
            _createdUserId = createdUser.Id;
        }

        [Test, Order(3)]
        public async Task GetUserById_ExistingUser_ShouldReturnOkStatusAndUser()
        {
            // Arrange
            _createdUserId.Should().BeGreaterThan(0, "User must be created first");

            // Act
            var response = await UserService.GetUserByIdAsync(_createdUserId);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().NotBeNullOrEmpty();

            var user = JsonSerializer.Deserialize<UserResponse>(responseContent);
            user.Should().NotBeNull();
            user!.Id.Should().Be(_createdUserId);
            user.Name.Should().NotBeNullOrEmpty();
            user.Email.Should().NotBeNullOrEmpty();
            user.Gender.Should().BeOneOf("male", "female");
            user.Status.Should().BeOneOf("active", "inactive");
        }

        [Test, Order(4)]
        public async Task GetUserById_NonExistingUser_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistingUserId = 999999999;

            // Act
            var response = await UserService.GetUserByIdAsync(nonExistingUserId);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseContent.Should().NotBeNullOrEmpty();

            // GoRest API returns different error formats for different scenarios
            // For 404, it might return a different format than for 422
            // Let's check if it contains the expected message string
            bool containsErrorMessage = responseContent.ToLower().Contains("resource not found") ||
                                       responseContent.ToLower().Contains("not found");
            containsErrorMessage.Should().BeTrue("Response should contain 'not found' or 'resource not found'");
        }

        [Test, Order(5)]
        public async Task UpdateUserPut_ExistingUser_ShouldReturnOkStatusAndUpdatedUser()
        {
            // Arrange
            _createdUserId.Should().BeGreaterThan(0, "User must be created first");
            var updateUser = TestDataGenerator.GenerateUser();

            // Act
            var response = await UserService.UpdateUserPutAsync(_createdUserId, updateUser);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().NotBeNullOrEmpty();

            var updatedUser = JsonSerializer.Deserialize<UserResponse>(responseContent);
            updatedUser.Should().NotBeNull();
            updatedUser!.Id.Should().Be(_createdUserId);
            updatedUser.Name.Should().Be(updateUser.Name);
            updatedUser.Email.Should().Be(updateUser.Email);
            updatedUser.Gender.Should().Be(updateUser.Gender);
            updatedUser.Status.Should().Be(updateUser.Status);
        }

        [Test, Order(6)]
        public async Task UpdateUserPatch_ExistingUser_ShouldReturnOkStatusAndPartiallyUpdatedUser()
        {
            // Arrange
            _createdUserId.Should().BeGreaterThan(0, "User must be created first");
            var updateUser = TestDataGenerator.GenerateUpdateUser(new[] { "name", "status" });

            // Act
            var response = await UserService.UpdateUserPatchAsync(_createdUserId, updateUser);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Debug output if the test fails
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"PATCH failed with status: {response.StatusCode}");
                Console.WriteLine($"Response: {responseContent}");
            }

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().NotBeNullOrEmpty();

            var updatedUser = JsonSerializer.Deserialize<UserResponse>(responseContent);
            updatedUser.Should().NotBeNull();
            updatedUser!.Id.Should().Be(_createdUserId);
            updatedUser.Name.Should().Be(updateUser.Name);
            updatedUser.Status.Should().Be(updateUser.Status);
            // Email and Gender should remain unchanged
            updatedUser.Email.Should().NotBeNullOrEmpty();
            updatedUser.Gender.Should().NotBeNullOrEmpty();
        }

        [Test, Order(7)]
        public async Task UpdateUserPut_NonExistingUser_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistingUserId = 999999999;
            var updateUser = TestDataGenerator.GenerateUser();

            // Act
            var response = await UserService.UpdateUserPutAsync(nonExistingUserId, updateUser);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseContent.Should().NotBeNullOrEmpty();

            // Check if it contains the expected message string
            bool containsErrorMessage = responseContent.ToLower().Contains("resource not found") ||
                                       responseContent.ToLower().Contains("not found");
            containsErrorMessage.Should().BeTrue("Response should contain 'not found' or 'resource not found'");
        }

        [Test, Order(8)]
        public async Task CreateUser_InvalidData_ShouldReturnUnprocessableEntity()
        {
            // Arrange
            var invalidUser = new CreateUserRequest
            {
                Name = "",
                Email = "invalid-email",
                Gender = "invalid",
                Status = "invalid"
            };

            // Act
            var response = await UserService.CreateUserAsync(invalidUser);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            responseContent.Should().NotBeNullOrEmpty();

            var errors = JsonSerializer.Deserialize<List<ErrorResponse>>(responseContent);
            errors.Should().NotBeNull();
            errors.Should().NotBeEmpty();
        }

        [Test, Order(9)]
        public async Task DeleteUser_ExistingUser_ShouldReturnNoContent()
        {
            // Arrange
            _createdUserId.Should().BeGreaterThan(0, "User must be created first");

            // Act
            var response = await UserService.DeleteUserAsync(_createdUserId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Verify user is deleted
            var getResponse = await UserService.GetUserByIdAsync(_createdUserId);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test, Order(10)]
        public async Task DeleteUser_NonExistingUser_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistingUserId = 999999999;

            // Act
            var response = await UserService.DeleteUserAsync(nonExistingUserId);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseContent.Should().NotBeNullOrEmpty();

            // Check if it contains the expected message string
            bool containsErrorMessage = responseContent.ToLower().Contains("resource not found") ||
                                       responseContent.ToLower().Contains("not found");
            containsErrorMessage.Should().BeTrue("Response should contain 'not found' or 'resource not found'");
        }

        [Test]
        public async Task CreateUserWithExistingEmail_ShouldReturnUnprocessableEntity()
        {
            // Arrange - Create a user first
            var user1 = TestDataGenerator.GenerateUser();
            var createResponse1 = await UserService.CreateUserAsync(user1);
            createResponse1.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdUser1 = JsonSerializer.Deserialize<UserResponse>(
                await createResponse1.Content.ReadAsStringAsync());

            // Create another user with the same email
            var user2 = TestDataGenerator.GenerateUser();
            user2.Email = user1.Email; // Same email

            // Act
            var response = await UserService.CreateUserAsync(user2);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            responseContent.Should().NotBeNullOrEmpty();

            var errors = JsonSerializer.Deserialize<List<ErrorResponse>>(responseContent);
            errors.Should().NotBeNull();
            errors.Should().NotBeEmpty();
            errors.Should().Contain(e => e.field == "email" && e.message.Contains("has already been taken"));

            // Cleanup
            await UserService.DeleteUserAsync(createdUser1!.Id);
        }
    }
}