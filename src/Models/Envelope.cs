using System.Collections.Generic;

namespace LibraryAPI.Models
{
    /// <summary>
    /// A POCO class for storing data and paging information
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public class Envelope<T>
    {
        /// <summary>
        /// The payload of the envelope
        /// </summary>
        /// <value>
        /// The Items property is the IEnumerable value of the payload for the envelope
        /// <para />
        /// Example: An IEnumerable of UserDTOs
        /// </value>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// The paging information of the envelope
        /// </summary>
        /// <value>
        /// The Paging property is the Paging value of for paging information
        /// </value>
        public Paging Paging { get; set; }
    }
}