using Microsoft.AspNetCore.Mvc;
using Sorted.Api.Models;
using System.Text.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Net;
using Sorted.Api.Interfaces;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rainfallRepo"></param>
        public RainfallController(IRainfallRepository rainfallRepo)
        {
            repository = rainfallRepo;
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
                Console.WriteLine($"An error occurred: {ex.Message}");

                return StatusCode(500, new ErrorResponse
                {
                    Message = "Internal server error"
                });
            }
        }

    }
}
