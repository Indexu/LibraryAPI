using AutoMapper;

namespace LibraryAPI.Repositories.EntityFrameworkCore
{
    public abstract class AbstractRepository
    {
        protected readonly DatabaseContext db;
        protected readonly IMapper mapper;
        protected readonly int defaultPageSize;

        public AbstractRepository(DatabaseContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
            this.defaultPageSize = 50;
        }
    }
}