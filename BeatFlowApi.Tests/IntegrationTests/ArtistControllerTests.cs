using System.Net;
using System.Net.Http.Json;
using BeatFlowApi.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BeatFlowApi.Tests.IntegrationTests;

public class ArtistControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ArtistControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetArtists_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/artists");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }

    [Fact]
    public async Task CreateArtist_ReturnsCreatedArtist()
    {
        // Arrange
        var newArtist = new ArtistDto
        {
            Name = "Integration Test Artist",
            Genre = "Rock",
            Bio = "Testing..."
        };

        // Act
        var response = await _client.PostAsJsonAsync("/artists", newArtist);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdArtist = await response.Content.ReadFromJsonAsync<ArtistDto>();
        Assert.NotNull(createdArtist);
        Assert.Equal(newArtist.Name, createdArtist.Name);
        Assert.True(createdArtist.Id > 0);
    }
}
