using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models.EntityModels;

namespace LibraryAPI.Repositories.EntityFrameworkCore
{
    /// <summary>
    /// A DbContext for the SQL database
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }

        /// <summary>
        /// The users
        /// </summary>
        /// <returns>A UserEntity</returns>
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// The books
        /// </summary>
        /// <returns>A BookEntity</returns>
        public DbSet<BookEntity> Books { get; set; }

        /// <summary>
        /// The book loans
        /// </summary>
        /// <returns>A LoanEntity</returns>
        public DbSet<LoanEntity> Loans { get; set; }

        /// <summary>
        /// The book ratings
        /// </summary>
        /// <returns>A RatingEntity</returns>
        public DbSet<RatingEntity> Ratings { get; set; }
    }
}
