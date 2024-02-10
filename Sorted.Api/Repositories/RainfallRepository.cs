using Microsoft.AspNetCore.Mvc;
using Sorted.Api.Interfaces;
using Sorted.Api.Models;
using System.Net;
using System.Text.Json;

namespace Sorted.Api.Repositories
{
    /// <summary>
    /// Implementation of Rainfall Repository
    /// </summary>
    public class RainfallRepository : IRainfallRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public RainfallRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        /// <summary>
        /// Request for rain fall reading on a specific station Id
        /// </summary>
        /// <param name="stationId">The station id to search for rainfall data</param>
        /// <param name="resultCountMax">The number of result to be return</param>
        /// <returns>A list of rainfall information based on station id</returns>
        public async Task<RainfallModelMessage> GetRainFallReadingByStationId(string stationId, int? resultCountMax = 0)
        {
            var client = _httpClientFactory.CreateClient();

            string apiUrl = $"https://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}/readings?_sorted&_limit={resultCountMax}";

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var readings = JsonSerializer.Deserialize<RainfallReadingResponse>(responseBody);

                if (readings?.Readings?.Count <= 0)
                {
                    return new RainfallModelMessage((int)HttpStatusCode.NotFound, null, new ErrorResponse
                    {
                        Message = "No readings found for the specified stationId."
                    });
                }

                return new RainfallModelMessage((int)HttpStatusCode.OK, readings, null);
            }
            else
            {
                ErrorResponse errorResponse;
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        errorResponse = new ErrorResponse
                        {
                            Message = "Bad request",
                            Detail = new List<ErrorDetail>() { new() { PropertyName = "stationId", Message = "Cannot find provided station id." } }
                        };
                        break;
                    case HttpStatusCode.NotFound:
                        errorResponse = new ErrorResponse
                        {
                            Message = "No readings found for the specified stationId",
                            Detail = new List<ErrorDetail>() { new() { PropertyName = "stationId", Message = "Cannot find provided station id." } }
                        };
                        break;
                    case HttpStatusCode.InternalServerError:
                        errorResponse = new ErrorResponse
                        {
                            Message = "Internal server error",
                            Detail = new List<ErrorDetail>()
                        };
                        break;
                    default:
                        errorResponse = new ErrorResponse
                        {
                            Message = "An unexpected error occurred",
                            Detail = new List<ErrorDetail>()
                        };
                        break;
                }
                return new RainfallModelMessage((int)response.StatusCode, null, errorResponse);
            }
        }
    }
}
