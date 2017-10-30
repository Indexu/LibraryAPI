using System;
using System.Runtime.Serialization;

namespace LibraryAPI.Exceptions
{
    /// <summary>
    /// An exception indicating an entity was not found
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception inner) : base(message, inner) { }
        protected NotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
