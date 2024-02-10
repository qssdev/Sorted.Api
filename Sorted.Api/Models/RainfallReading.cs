using System.Text.Json.Serialization;

namespace Sorted.Api.Models
{
    /// <summary>
    /// Rainfall reading
    /// </summary>
    public class RainfallReading
    {
        [JsonIgnore]
        public string Id { get; set; }
        [JsonIgnore]
        public string Measure { get; set; }
        
        /// <summary>
        /// Gets or sets the date and time when the rainfall was measured.
        /// </summary>
        /// <remarks>Format: YYYY-MM-DDTHH:MM:SSZ</remarks>
        [JsonPropertyName("dateTime")]
        public DateTime DateMeasured { get; set; }

        /// <summary>
        /// Gets or sets the amount of rainfall measured.
        /// </summary>
        /// <remarks>Measured in millimeters.</remarks>
        [JsonPropertyName("value")]
        public decimal AmountMeasured { get; set; }

       
        ///// <summary>
        ///// Gets or sets the error response, if any.
        ///// </summary>
        //public ErrorResponse Error { get; set; }
    }
}
