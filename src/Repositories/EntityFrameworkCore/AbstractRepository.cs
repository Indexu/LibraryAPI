using AutoMapper;

namespace LibraryAPI.Repositories.EntityFrameworkCore
{
    /// <summary>
    /// An abstract class for repositories
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public abstract class AbstractRepository
    {
        protected readonly DatabaseContext db;
        protected readonly IMapper mapper;
        protected const int defaultPageSize = 50;
        protected const string bookNotFoundMessage = "Book not found";
        protected const string bookAlreadyExistsMessage = "Book with that ISBN already exists";
        protected const string reviewNotFoundMessage = "Review not found";
        protected const string reviewAlreadyExistsMessage = "Review already exists";
        protected const string loanNotFoundMessage = "User does not have the book loaned";
        protected const string loanAlreadyExistsMessage = "User already has book loaned";
        protected const string dateNonsenseMessage = "Loan date must be before the return date";
        protected const string userNotFoundMessage = "User not found";
        protected const string userAlreadyExistsMessage = "User with that email already exists";

        public AbstractRepository(DatabaseContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
    }
}