namespace Sorted.Api.Models
{
    /// <summary>
    /// Details of invalid request property
    /// </summary>
    public class ErrorDetail
    {
        /// <summary>
        /// name of the property
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;
        /// <summary>
        /// error message
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
