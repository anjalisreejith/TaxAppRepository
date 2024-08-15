namespace TaxApp.Application
{
    /// <summary>
    /// Model to return error response
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Gets or sets Status Code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets Detailed
        /// </summary>
        public string Detailed { get; set; }
    }
}
