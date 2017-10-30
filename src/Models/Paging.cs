namespace LibraryAPI.Models
{
    /// <summary>
    /// A POCO class representing paging information
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public class Paging
    {
        /// <summary>
        /// The number of pages in total corresponding with the page size
        /// </summary>
        /// <value>
        /// The PageCount property is the integer value of the number of pages
        /// <para />
        /// Example: 15
        /// </value>
        public int PageCount { get; set; }

        /// <summary>
        /// The number of items on the current page
        /// </summary>
        /// <value>
        /// The PageSize property is the integer value of the number of items on the current page
        /// <para />
        /// Example: 10
        /// </value>
        public int PageSize { get; set; }

        /// <summary>
        /// The maximum number of items that can be on a single page
        /// </summary>
        /// <value>
        /// The PageMaxSize property is the integer value of the maximum number of items that can be on a single page
        /// <para />
        /// Example: 10
        /// </value>
        public int PageMaxSize { get; set; }

        /// <summary>
        /// The current page number
        /// </summary>
        /// <value>
        /// The PageNumber property is the integer value of the current page number
        /// <para />
        /// Example: 3
        /// </value>
        public int PageNumber { get; set; }

        /// <summary>
        /// The total number of items in all pages
        /// </summary>
        /// <value>
        /// The TotalNumberOfItems property is the integer value of the total number of items on all pages
        /// <para />
        /// Example: 150
        /// </value>
        public int TotalNumberOfItems { get; set; }
    }
}