using NUnit.Framework;
using WebDriverAPI.Core;
using WebDriverAPI.Business.Models;
using WebDriverAPI.Core.Logging;
using RestSharp;
using System.Net;

namespace WebDriverAPI.Tests
{
    [TestFixture]
    [Category("API")]
    [Parallelizable(ParallelScope.All)]
    public class UserApiTests : BaseApiClient
    {
        [Test]
        public async Task GetUsersListTest()
        {
            var request = CreateRequest("/users", Method.Get);
            var response = await Client.ExecuteAsync<List<UserModel>>(request);

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            LoggerManager.LogInfo("Users list received successfully");
        }

        [Test]
        public async Task ValidateResponseHeadersTest()
        {
            var request = CreateRequest("/users", Method.Get);
            var response = await Client.ExecuteAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            LoggerManager.LogInfo("Status code is 200 OK");

            Assert.That(response.ContentType, Is.Not.Null);
            Assert.That(response.ContentType, Contains.Substring("application/json"));
            LoggerManager.LogInfo($"Content-Type header validated: {response.ContentType}");

            Assert.That(response.ErrorException, Is.Null, "Response contains error");
            LoggerManager.LogInfo("Response headers validated successfully");
        }

        [Test]
        public async Task ValidateResponseForUsersList()
        {
            var request = new RestRequest("/users", Method.Get);
            var response = await Client.ExecuteAsync<List<UserModel>>(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Expected status code 200 OK.");
            Assert.That(response.Data, Is.Not.Null, "Response body is null.");
            Assert.That(response.Data.Count, Is.EqualTo(10), "Expected 10 users in the response.");

            var userIds = response.Data.Select(user => user.Id).ToList();
            Assert.That(userIds.Distinct().Count(), Is.EqualTo(userIds.Count), "User IDs are not unique.");

            foreach (var user in response.Data)
            {
                Assert.That(string.IsNullOrWhiteSpace(user.Name), Is.False, $"User with ID {user.Id} has an empty or null Name.");
                Assert.That(string.IsNullOrWhiteSpace(user.Username), Is.False, $"User with ID {user.Id} has an empty or null Username.");

                Assert.That(user.Company, Is.Not.Null, $"User with ID {user.Id} has no Company.");
                Assert.That(string.IsNullOrWhiteSpace(user.Company.Name), Is.False, $"User with ID {user.Id} has an empty or null Company Name.");
                LoggerManager.LogInfo("All users have been successfully validated: Name, Username, and Company are not empty.");
            }
        }

        [Test]
        public async Task ValidateUserCreation()
        {
            var newUser = new UserModel
            {
                Name = "John Doe",
                Username = "johndoe"
            };

            var request = new RestRequest("/users", Method.Post);
            request.AddJsonBody(newUser);
            var response = await Client.ExecuteAsync<UserModel>(request);

            Assert.That(response.Data, Is.Not.Null, "Response body is empty.");
            Assert.That(response.Data.Id, Is.GreaterThan(0), "User ID should be greater than 0.");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Expected status code 201 Created.");
            Assert.That(response.ErrorException, Is.Null, "Response contains an error.");
            LoggerManager.LogInfo($"User with Name '{newUser.Name}' and Username '{newUser.Username}' created successfully with ID {response.Data.Id}.");
        }
        [Test]
        public async Task ValidateResourceNotFound()
        {
            var request = new RestRequest("/invalidendpoint", Method.Get);
            var response = await Client.ExecuteAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Expected status code 404 Not Found.");
            LoggerManager.LogInfo("Successfully received 404 Not Found status for a non-existent endpoint.");
        }
    }
}