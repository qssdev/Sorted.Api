namespace Sorted.Api.Models
{
    /// <summary>
    /// Error response
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Details of a rainfall reading
        /// </summary>
        public string Message { get; set; } = string.Empty;
        /// <summary>
        /// Details of invalid request property
        /// </summary>
        public List<ErrorDetail> Detail { get; set; } = new List<ErrorDetail>();
    }
}
