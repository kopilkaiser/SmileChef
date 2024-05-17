using Microsoft.AspNetCore.Mvc.Testing;
using SmileChef;

namespace ChefAppTestProject
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Chefs_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = "/chefs/"; // Adjust this based on your actual API endpoint

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact()]
        public async Task Get_ChefsInfo_ReturnsExpectedContentType()
        {
            // Arrange
            var request = "/chefs/info"; // Adjust this based on your actual API endpoint

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json", response.Content.Headers.ContentType!.ToString());
        }

        [Fact()]
        public async Task Get_ChefsInfoV2_ReturnsExpectedContentType()
        {
            // Arrange
            var request = "/chefs/infov2"; // Adjust this based on your actual API endpoint

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json", response.Content.Headers.ContentType!.ToString());
        }
    }
}