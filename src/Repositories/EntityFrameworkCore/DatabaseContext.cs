using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models.EntityModels;

namespace LibraryAPI.Repositories.EntityFrameworkCore
{
    /// <summary>
    /// A DbContext for the SQL database using Entity Framework Core
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }

        /// <summary>
        /// The users
        /// </summary>
        /// <returns>A DbSet of UserEntity</returns>
        public virtual DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// The books
        /// </summary>
        /// <returns>A DbSet of BookEntity</returns>
        public virtual DbSet<BookEntity> Books { get; set; }

        /// <summary>
        /// The book loans
        /// </summary>
        /// <returns>A DbSet of LoanEntity</returns>
        public virtual DbSet<LoanEntity> Loans { get; set; }

        /// <summary>
        /// The book reviews
        /// </summary>
        /// <returns>A DbSet of ReviewEntity</returns>
        public virtual DbSet<ReviewEntity> Reviews { get; set; }
    }
}
