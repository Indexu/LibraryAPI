using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.ViewModels
{
    /// <summary>
    /// A POCO class for recieving user input for a review PATCH
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public class PatchReviewViewModel
    {
        /// <summary>
        /// The rating of the book
        /// </summary>
        /// <value>
        /// The Rating property is the integer value of the book's rating between 0 and 5, inclusive
        /// <para />
        /// Example: 3
        /// </value>
        [Range(0, 5)]
        public int? Rating { get; set; }
    }
}
