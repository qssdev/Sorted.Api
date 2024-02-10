using System.Net;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Sorted.Api.Models;
using Sorted.Api.Repositories;

namespace Storted.Api.Test
{
    public class RainfallRepositoryTests
    {
        [Fact]
        public async Task GetRainFallReadingByStationId_Success()
        {
            // Arrange
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHandler = new Mock<HttpMessageHandler>();

            var stationId = "123";
            var resultCountMax = 10;

            var responseBody = "{\"Readings\": []}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseBody)
            };

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:5283")
            };

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                       .Returns(httpClient);

            var repository = new RainfallRepository(mockFactory.Object);

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
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHandler = new Mock<HttpMessageHandler>();

            var stationId = "123";
            var resultCountMax = 10;

            var response = new HttpResponseMessage(HttpStatusCode.NotFound);

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:5283")
            };

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                       .Returns(httpClient);

            var repository = new RainfallRepository(mockFactory.Object);

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
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHandler = new Mock<HttpMessageHandler>();

            var stationId = "123";
            var resultCountMax = 10;

            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:5283")
            };

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                       .Returns(httpClient);

            var repository = new RainfallRepository(mockFactory.Object);

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
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHandler = new Mock<HttpMessageHandler>();

            var stationId = "123";
            var resultCountMax = 10;

            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:5283")
            };

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                       .Returns(httpClient);

            var repository = new RainfallRepository(mockFactory.Object);

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