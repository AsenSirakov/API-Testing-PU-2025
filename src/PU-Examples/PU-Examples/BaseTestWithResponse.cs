namespace PU_Examples
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class BaseTestWithResponse
    {
        private HttpClient _httpClient;
        private const string BaseUrl = "https://gorest.co.in/public/v2/users";

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
        }

        [TearDown]
        public void Teardown()
        {
            _httpClient.Dispose();
        }

        [Test]
        public async Task GetUsers_ShouldReturnValidUserList()
        {
            // Act
            HttpResponseMessage response = await _httpClient.GetAsync(BaseUrl);

            // Assert
            Assert.That(response.IsSuccessStatusCode, Is.True, "Expected a successful response.");
            Assert.That((int)response.StatusCode, Is.EqualTo(200), "Expected HTTP status code 200 OK.");

            string responseData = await response.Content.ReadAsStringAsync();
            List<UserResponse> users = JsonConvert.DeserializeObject<List<UserResponse>>(responseData);

            Assert.That(users, Is.Not.Null.And.Not.Empty, "User list should not be null or empty.");
        }
    }

    public class UserResponse 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
    }
}

