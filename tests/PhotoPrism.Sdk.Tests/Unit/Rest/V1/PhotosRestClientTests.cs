using System.Net;
using System.Text;

using Moq;
using Serilog;

using PhotoPrism.Sdk.Rest.V1;
using PhotoPrismOrganizer.Common.Apis.Exceptions;

namespace PhotoPrism.Sdk.Tests.Unit.Rest.V1;

public class PhotosRestClientTests
{
    private static (PhotosRestClient client, Mock<IHttpClientFactory> httpFactory, Mock<ILogger> logger) CreateClient(HttpResponseMessage response, Action<HttpRequestMessage>? capture = null)
    {
        var handler = new StubHttpMessageHandler(response, capture);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var httpFactory = new Mock<IHttpClientFactory>();
        httpFactory.Setup(f => f.CreateClient("PhotoPrism")).Returns(httpClient);

        var logger = new Mock<ILogger>();

        var client = new PhotosRestClient(httpFactory.Object, logger.Object);
        return (client, httpFactory, logger);
    }

    [Fact]
    public async Task SearchPhotosAsync_MalformedJson_Throws_ApiException_With_JsonParse_Code()
    {
        var badJson = "{ not-valid-json ]";
        var response = JsonResponse(badJson);
        var (client, _, _) = CreateClient(response);

        var ex = await Assert.ThrowsAsync<ApiException>(() => client.SearchPhotosAsync(1));
        Assert.Equal("JSON_PARSE_ERROR", ex.Code);
        Assert.Contains("photo search", ex.Message);
    }

    [Fact]
    public async Task SearchPhotosAsync_Object_Instead_Of_Array_Throws_ApiException_JsonParse()
    {
        var wrongShape = "{ \"UID\": \"only-object\" }"; // Expecting array, got object
        var response = JsonResponse(wrongShape);
        var (client, _, _) = CreateClient(response);

        var ex = await Assert.ThrowsAsync<ApiException>(() => client.SearchPhotosAsync(1));
        Assert.Equal("JSON_PARSE_ERROR", ex.Code);
    }

    [Fact]
    public async Task SearchPhotosAsync_Null_Body_Throws_InvalidOperationException()
    {
        var nullBody = "null";
        var response = JsonResponse(nullBody);
        var (client, _, _) = CreateClient(response);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => client.SearchPhotosAsync(1));
        Assert.Contains("Failed to deserialize response", ex.Message);
    }

    private static HttpResponseMessage JsonResponse(string json, HttpStatusCode status = HttpStatusCode.OK) =>
        new(status)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

    [Fact]
    public async Task SearchPhotosAsync_Returns_Items()
    {
        var json = "[ { \"UID\": \"u1\", \"Title\": \"Photo 1\" }, { \"UID\": \"u2\", \"Title\": \"Photo 2\" } ]";
        var response = JsonResponse(json);
        var (client, _, _) = CreateClient(response);

        var results = await client.SearchPhotosAsync(count: 2, cancellationToken: default);

        Assert.NotNull(results);
        Assert.Equal(2, results.Count);
        Assert.Equal("u1", results[0].UID);
        Assert.Equal("Photo 1", results[0].Title);
    }

    [Fact]
    public async Task SearchPhotosAsync_EmptyArray_Returns_EmptyList()
    {
        var json = "[]";
        var response = JsonResponse(json);
        var (client, _, _) = CreateClient(response);

        var results = await client.SearchPhotosAsync(count: 0);

        Assert.NotNull(results);
        Assert.Empty(results);
    }

    [Fact]
    public async Task SearchPhotosAsync_Maps_400_To_BadRequest()
    {
        var response = JsonResponse("{ \"error\": \"bad params\" }", HttpStatusCode.BadRequest);
        var (client, _, _) = CreateClient(response);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => client.SearchPhotosAsync(1));
        Assert.Contains("photo search", ex.Message);
    }

    [Fact]
    public async Task SearchPhotosAsync_Maps_401_To_Unauthenticated()
    {
        var response = JsonResponse("{}", HttpStatusCode.Unauthorized);
        var (client, _, _) = CreateClient(response);

        await Assert.ThrowsAsync<UnauthenticatedException>(() => client.SearchPhotosAsync(1));
    }

    [Fact]
    public async Task SearchPhotosAsync_Maps_403_To_Unauthorized()
    {
        var response = JsonResponse("{}", HttpStatusCode.Forbidden);
        var (client, _, _) = CreateClient(response);

        await Assert.ThrowsAsync<UnauthorizedException>(() => client.SearchPhotosAsync(1));
    }

    [Fact]
    public async Task SearchPhotosAsync_Maps_404_To_NotFound()
    {
        var response = JsonResponse("{}", HttpStatusCode.NotFound);
        var (client, _, _) = CreateClient(response);

        await Assert.ThrowsAsync<NotFoundException>(() => client.SearchPhotosAsync(1));
    }

    [Fact]
    public async Task SearchPhotosAsync_Maps_429_To_RateLimit()
    {
        var response = JsonResponse("{}", HttpStatusCode.TooManyRequests);
        var (client, _, _) = CreateClient(response);

        await Assert.ThrowsAsync<RateLimitException>(() => client.SearchPhotosAsync(1));
    }

    [Fact]
    public async Task SearchPhotosAsync_Maps_5xx_To_ServerException()
    {
        var response = JsonResponse("{}", HttpStatusCode.InternalServerError);
        var (client, _, _) = CreateClient(response);

        await Assert.ThrowsAsync<ServerException>(() => client.SearchPhotosAsync(1));
    }

    [Fact]
    public async Task SearchPhotosAsync_Builds_Query_Params_Correctly()
    {
        HttpRequestMessage? captured = null;
        var json = "[]";
        var response = JsonResponse(json);
        var (client, _, _) = CreateClient(response, req => captured = req);

        await client.SearchPhotosAsync(
            count: 25,
            offset: 5,
            order: "newest",
            merged: true,
            @public: false,
            quality: 3,
            query: "sunset",
            albumUid: "A1",
            path: "/images",
            video: true
        );

        Assert.NotNull(captured);
        var uri = captured!.RequestUri!;
        Assert.Equal("/api/v1/photos", uri.AbsolutePath);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        Assert.Equal("25", query["count"]);
        Assert.Equal("5", query["offset"]);
        Assert.Equal("newest", query["order"]);
        Assert.Equal("True", query["merged"]);
        Assert.Equal("False", query["public"]);
        Assert.Equal("3", query["quality"]);
        Assert.Equal("sunset", query["q"]);
        Assert.Equal("A1", query["s"]);
        Assert.Equal("/images", query["path"]);
        Assert.Equal("True", query["video"]);
    }

    private sealed class StubHttpMessageHandler(HttpResponseMessage response, Action<HttpRequestMessage>? capture)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            capture?.Invoke(request);
            return Task.FromResult(response);
        }
    }
}
