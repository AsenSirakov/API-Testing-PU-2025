using FinalProject.Framework.Models;
using Newtonsoft.Json;

namespace FinalProject.Tests
{
    [TestFixture]
    public class UsersTests : TestBase
    {
        [Test]
        public async Task GetUsers()
        {
            var response = await Client.GetAsync("users");
            string responseData = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<UserResponse>>(responseData);
        }
    }
}
