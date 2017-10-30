using System;
using System.Runtime.Serialization;

namespace LibraryAPI.Exceptions
{
    /// <summary>
    /// An exception indicating an entity already exists
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    [Serializable]
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() { }
        public AlreadyExistsException(string message) : base(message) { }
        public AlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected AlreadyExistsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
