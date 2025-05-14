using FinalProject.Framework.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FinalProject.Framework.Service
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetAllUsersAsync()
        {
            return await _httpClient.GetAsync("users");
        }

        public async Task<HttpResponseMessage> GetUserByIdAsync(int id)
        {
            return await _httpClient.GetAsync($"users/{id}");
        }

        public async Task<HttpResponseMessage> CreateUserAsync(CreateUserRequest user)
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("users", content);
        }

        public async Task<HttpResponseMessage> UpdateUserPutAsync(int id, CreateUserRequest user)
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync($"users/{id}", content);
        }

        public async Task<HttpResponseMessage> UpdateUserPatchAsync(int id, UpdateUserRequest user)
        {
            // For PATCH, we need to serialize only non-null properties
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var json = JsonSerializer.Serialize(user, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PatchAsync($"users/{id}", content);
        }

        public async Task<HttpResponseMessage> DeleteUserAsync(int id)
        {
            return await _httpClient.DeleteAsync($"users/{id}");
        }
    }
}