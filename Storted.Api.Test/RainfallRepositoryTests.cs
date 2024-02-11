using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using Sorted.Api.Models;
using Sorted.Api.Repositories;
using Sorted.Api.Extensions;
using System.Net;

namespace Storted.Api.Test
{
    public class RainfallRepositoryTests
    {
        private void SetupMockObjects(HttpResponseMessage response, out Mock<IHttpClientFactory> mockFactory)
        {
            mockFactory = new Mock<IHttpClientFactory>();
            var mockHandler = new Mock<HttpMessageHandler>();

            var responseBody = "{\"Readings\": []}";

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:5283")
            };

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                       .Returns(httpClient);
        }


        [Fact]
        public async Task GetRainFallReadingByStationId_Success()
        {
            // Arrange
            var stationId = "123";
            var resultCountMax = 10;

            var responseBody = "{\"Readings\": []}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseBody)
            };

            SetupMockObjects(response, out Mock<IHttpClientFactory> mockFactory);

            var mockCache = new Mock<IMemoryCache>();
            var cachedResult = new RainfallModelMessage(200, new RainfallReadingResponse(), null);
            mockCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                     .Returns(true)
                     .Callback((object key, out object value) =>
                     {
                         value = cachedResult;
                         TimeSpan.FromMinutes(5);
                     });

            var repository = new RainfallRepository(mockFactory.Object, mockCache.Object);

            // Act
            var result = await repository.GetRainFallReadingByStationId(stationId, resultCountMax);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Null(result.Error);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetRainFallReadingByStationId_NotFound()
        {
            // Arrange
            var stationId = "-1";
            var resultCountMax = 10;

            var response = new HttpResponseMessage(HttpStatusCode.NotFound);

            SetupMockObjects(response, out Mock<IHttpClientFactory> mockFactory);

            var mockCache = new Mock<IMemoryCache>();
            var cachedResult = new RainfallModelMessage(404, new RainfallReadingResponse(), null);

            mockCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            mockCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                     .Returns(false);

            mockCache.Object.SetCache(stationId, cachedResult, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5) });

            var repository = new RainfallRepository(mockFactory.Object, mockCache.Object);

            // Act
            var result = await repository.GetRainFallReadingByStationId(stationId, resultCountMax);


            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetRainFallReadingByStationId_BadRequest()
        {
            // Arrange
            var stationId = "123";
            var resultCountMax = 10;

            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            SetupMockObjects(response, out Mock<IHttpClientFactory> mockFactory);

            var mockCache = new Mock<IMemoryCache>();

            var cachedResult = new RainfallModelMessage(400, new RainfallReadingResponse(), null);

            mockCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            mockCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                     .Returns(false);

            mockCache.Object.SetCache(stationId, cachedResult, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5) });

            var repository = new RainfallRepository(mockFactory.Object, mockCache.Object);

            // Act
            var result = await repository.GetRainFallReadingByStationId(stationId, resultCountMax);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetRainFallReadingByStationId_InternalServerError()
        {
            // Arrange
            var stationId = "123";
            var resultCountMax = 10;

            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            SetupMockObjects(response, out Mock<IHttpClientFactory> mockFactory);

            var mockCache = new Mock<IMemoryCache>();

            var cachedResult = new RainfallModelMessage(500, new RainfallReadingResponse(), null);

            mockCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            mockCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                     .Returns(false);

            mockCache.Object.SetCache(stationId, cachedResult, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5) });

            var repository = new RainfallRepository(mockFactory.Object, mockCache.Object);

            // Act
            var result = await repository.GetRainFallReadingByStationId(stationId, resultCountMax);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.Null(result.Data);
        }
    }
}