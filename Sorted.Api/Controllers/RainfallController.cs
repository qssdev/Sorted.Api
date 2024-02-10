using Microsoft.AspNetCore.Mvc;
using Sorted.Api.Interfaces;
using Sorted.Api.Models;
using System.Net.Http;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace Sorted.Api.Controllers
{
    /// <summary>
    /// Operations relating to rainfall
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    public class RainfallController : ControllerBase
    {
        private readonly IRainfallRepository repository;
        private readonly ILogger<RainfallController> _logger;

        /// <summary>
        /// Constructor for rain fall repository
        /// </summary>
        /// <param name="rainfallRepo">Rainfall repository to use </param>
        /// <param name="logger">logger interface to create logging</param>
        public RainfallController(IRainfallRepository rainfallRepo, ILogger<RainfallController> logger)
        {
            repository = rainfallRepo;
            _logger = logger;
        }

        /// <summary>
        /// Get rainfall readings by station Id 
        /// </summary>
        /// <param name="stationId" required>The id of the reading station</param>
        /// <param name="count">The number of readings to return</param>
        /// <response code="200">A list of rainfall readings successfully retrieved</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">No readings found for the specified stationId</response>
        /// <response code="500">Internal server error</response>
        /// <returns>Retrieve the latest readings for the specified stationId</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RainfallReadingResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RainfallReadingResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [HttpGet("rainfall/id/{stationId}/readings")]
        public async Task<ActionResult<RainfallReadingResponse>> GetRainfallReadings(string stationId, int? count = 10)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = new ErrorResponse
                    {
                        Message = "Invalid request",
                        Detail = new List<ErrorDetail>()
                    };
                    return BadRequest(errorResponse);
                }

                if (!Regex.IsMatch(stationId, @"^[a-zA-Z0-9]*$"))
                {
                    return BadRequest("StationId must consist of alphanumeric characters only");
                }

                _logger.LogInformation($"request rainfall for stationid: {stationId}", stationId, count);

                var rainFallData = await repository.GetRainFallReadingByStationId(stationId, count);

                if (rainFallData.StatusCode != (int)StatusCodes.Status200OK)
                {
                    return StatusCode(rainFallData.StatusCode, rainFallData.Error);
                }

                return StatusCode(rainFallData.StatusCode, rainFallData.Data);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while processing the request");

                return StatusCode(500, new ErrorResponse
                {
                    Message = "Internal server error"
                });
            }
        }

    }
}
