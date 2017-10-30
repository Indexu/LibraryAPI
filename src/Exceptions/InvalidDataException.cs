using System;
using System.Runtime.Serialization;

namespace LibraryAPI.Exceptions
{
    /// <summary>
    /// An exception indicating the data is invalid
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    [Serializable]
    public class InvalidDataException : Exception
    {
        public InvalidDataException() { }
        public InvalidDataException(string message) : base(message) { }
        public InvalidDataException(string message, Exception inner) : base(message, inner) { }
        protected InvalidDataException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
