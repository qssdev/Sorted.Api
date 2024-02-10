using System.Text.Json.Serialization;

namespace Sorted.Api.Models
{
    /// <summary>
    /// Gets or sets the list of rainfall readings.
    /// Rainfall reading response
    /// </summary>
    public class RainfallReadingResponse
    {
        /// <summary>
        /// Array of RainfallReading objects.
        /// </summary>
        [JsonPropertyName("items")]
        public List<RainfallReading>? Readings { get; set; }
    }
}
