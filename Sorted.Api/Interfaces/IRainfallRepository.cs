using Microsoft.AspNetCore.Mvc;
using Sorted.Api.Models;

namespace Sorted.Api.Interfaces
{
    /// <summary>
    /// Provide rain fall repository definition
    /// </summary>
    public interface IRainfallRepository
    {
        /// <summary>
        /// Request for rain fall reading on a specific station Id
        /// </summary>
        /// <param name="stationId">The station id to search for rainfall data</param>
        /// <param name="resultCountMax">The number of result to be return</param>
        /// <returns>A list of rainfall information based on station id</returns>
        public Task<RainfallModelMessage> GetRainFallReadingByStationId(string stationId, int? resultCountMax = 0);
    }
}
