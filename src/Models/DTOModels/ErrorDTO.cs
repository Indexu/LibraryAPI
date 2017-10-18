namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for a user
    /// </summary>
    public class ErrorDTO
    {
        /// <summary>
        /// The error code, an HTTP status code
        /// </summary>
        /// <value>
        /// The Code property is the integer value of the error code for the error
        /// <para />
        /// Example: 404
        /// </value>
        public int Code { get; set; }

        /// <summary>
        /// The error message
        /// </summary>
        /// <value>
        /// The Message property is the string value of the error message for the error
        /// <para />
        /// Example: User not found
        /// </value>
        public string Message { get; set; }
    }
}
