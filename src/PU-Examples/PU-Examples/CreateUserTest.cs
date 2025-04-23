using System.Net.Http.Headers;
using System.Text;

namespace PU_Examples
{

 [TestFixture]
 public class CreateUserTest
    {
        private readonly string _accessToken = "bbb78baeaa8eb6766958e7aed8d8870c7a4cb34e086d706c010de5dfcdf4b6d5"; // смени с реален token
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient();
            _client.BaseAddress = new System.Uri("https://gorest.co.in/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        [Test]
        public async Task CreateUser_ShouldReturn201Created()
        {
            var requestBody = new
            {
                name = "Tenali Ramakrishna",
                gender = "male",
                email = $"tenali.ramakrishna.{System.Guid.NewGuid()}@example.com", // уникален email
                status = "active"
            };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("public/v2/users", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created), $"Unexpected response: {responseBody}");
        }
    }
}
