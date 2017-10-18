using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models.EntityModels;
using AutoMapper;

namespace LibraryAPI.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Users
            CreateMap<UserViewModel, UserEntity>();
            CreateMap<UserEntity, UserDTO>();
            CreateMap<UserEntity, UserDetailsDTO>();
            CreateMap<PatchUserViewModel, UserViewModel>();

            // Books
            CreateMap<BookViewModel, BookEntity>();
            CreateMap<BookEntity, BookDTO>();
            CreateMap<PatchBookViewModel, BookViewModel>();

            // Loans
            CreateMap<LoanViewModel, LoanEntity>();
            CreateMap<LoanEntity, LoanDTO>();
            CreateMap<LoanDTO, UserLoanDTO>();
            CreateMap<LoanEntity, UserLoanDTO>();
            CreateMap<PatchLoanViewModel, LoanViewModel>();
            CreateMap<LoanViewModel, PatchLoanViewModel>();
        }
    }
}