namespace PU_Examples
{

    [TestFixture]
    public class BaseTest
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
        public async Task GetUsers_ShouldReturnSuccess()
        {
            // Act
            HttpResponseMessage response = await _httpClient.GetAsync(BaseUrl);

            // Assert
            Assert.That(response.IsSuccessStatusCode, Is.True, "Expected a successful response.");

            string responseData = await response.Content.ReadAsStringAsync();
            Assert.That(responseData, Is.Not.Empty, "Response should not be empty.");
        }
    }
}