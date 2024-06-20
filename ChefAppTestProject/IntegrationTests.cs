using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SmileChef;
using SmileChef.Repository;
using SmileChef.ViewModels;
using System.Text.Json;

namespace ChefAppTestProject
{
    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly IChefRepository _chefRepo;

        public IntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            // Create a scope to resolve the service provider
            var scope = factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            _chefRepo = scopedServices.GetRequiredService<IChefRepository>();
        }

        [Fact]
        public async Task Get_Chefs_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = "/tests"; // Adjust this based on your actual API endpoint

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact()]
        public async Task Get_ChefsInfo_ReturnsExpectedContentType()
        {
            // Arrange
            var request = "/tests/info"; // Adjust this based on your actual API endpoint

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
            var request = "/tests/infov2"; // Adjust this based on your actual API endpoint

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json", response.Content.Headers.ContentType!.ToString());
        }

        [Fact]
        public async Task Get_ChefViewModel_ReturnsExpectedContentTypeAndData()
        {
            // Arrange
            var request = "/?json=true"; // Adjust this based on your actual API endpoint
            int currentUserId = 1; // This should match the logic used in your controller

            // Expected data from the repository
            var expected = _chefRepo.GetChefsWithDetails().FirstOrDefault(c => c.User.UserId == currentUserId);

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType!.ToString());

            var responseContent = await response.Content.ReadAsStringAsync();

            // Here we assume your view renders a JSON representation of ChefViewModel for the test purposes
            var actual = JsonSerializer.Deserialize<ChefViewModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Compare the expected and actual ChefViewModel
            Assert.NotNull(actual);
            Assert.Equal(expected.ChefName, actual.ChefName);
            Assert.Equal(expected.User.Email, actual.User.Email);

            // Compare the list of recipes
            Assert.Equal(expected.Recipes.Count, actual.Recipes.Count);
            for (int i = 0; i < expected.Recipes.Count; i++)
            {
                Assert.Equal(expected.Recipes[i].Name, actual.Recipes[i].Name);
                Assert.Equal(expected.Recipes[i].RecipeId, actual.Recipes[i].RecipeId);
                // Add more detailed assertions as needed to compare other properties
            }
        }
    }
}