using Moq;
using Serilog;

using PhotoPrism.Sdk.Rest.V1;

namespace PhotoPrism.Sdk.Tests.Integration.Rest.V1;

public class PhotosRestClientTests
{

    private static PhotosRestClient CreateClient(ILogger? logger = null)
    {
        var baseUrl = Environment.GetEnvironmentVariable("PHOTOPRISM_SITE_URL") ?? "http://host.docker.internal:2342";
        var httpFactory = new Mock<IHttpClientFactory>();
        httpFactory.Setup(f => f.CreateClient("PhotoPrism")).Returns(new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        });

        logger ??= new Mock<ILogger>().Object;

        return new PhotosRestClient(httpFactory.Object, logger);
    }

    [Fact]
    public async Task SearchPhotosAsync_FindsDemoPhotos_ReturnsExpectedFivePhotos()
    {
        // Arrange
        var client = CreateClient();
        var expectedPhotoNames = new[] { "1", "2", "3", "4", "5" };

        // Act
        var searchResults = await client.SearchPhotosAsync(
            count: 100,
            order: "name"
        );

        // Assert
        Assert.NotNull(searchResults);
        Assert.True(searchResults.Count >= 5, $"Expected at least 5 photos, but found {searchResults.Count}");

        // Find the demo photos by name
        var demoPhotos = searchResults
            .Where(photo => expectedPhotoNames.Contains(photo.Name))
            .ToList();

        Assert.Equal(5, demoPhotos.Count);

        // Verify each expected photo is present
        foreach (var expectedName in expectedPhotoNames)
        {
            var photo = demoPhotos.FirstOrDefault(p => p.Name == expectedName);
            Assert.NotNull(photo);
            Assert.NotNull(photo.UID);
            Assert.NotNull(photo.Name);
            Assert.Equal(expectedName, photo.Name);
        }

        // Verify that photos are ordered by name
        var orderedNames = searchResults.Select(p => p.Name).OrderBy(n => n).ToList();
        Assert.Equal(orderedNames, [.. searchResults.Select(p => p.Name)]);
    }

    [Fact]
    public async Task SearchPhotosAsync_WithDifferentCountParameters_RespectsLimits()
    {
        // Arrange
        var client = CreateClient();

        // Act - Test with a small count limit
        var limitedResults = await client.SearchPhotosAsync(count: 2);
        var unlimitedResults = await client.SearchPhotosAsync(count: 100);

        // Assert
        Assert.NotNull(limitedResults);
        Assert.NotNull(unlimitedResults);

        if (unlimitedResults.Count >= 2)
        {
            Assert.True(limitedResults.Count <= 2, $"Expected at most 2 results, got {limitedResults.Count}");
            Assert.True(unlimitedResults.Count >= limitedResults.Count,
                "Unlimited search should return at least as many results as limited search");
        }
    }

}
