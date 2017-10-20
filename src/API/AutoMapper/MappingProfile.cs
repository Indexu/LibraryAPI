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
            CreateMap<UserDTO, UserDetailsDTO>();
            CreateMap<PatchUserViewModel, UserViewModel>();

            // Books
            CreateMap<BookViewModel, BookEntity>();
            CreateMap<BookEntity, BookDTO>();
            CreateMap<BookEntity, BookDetailsDTO>();
            CreateMap<BookDTO, BookDetailsDTO>();
            CreateMap<PatchBookViewModel, BookViewModel>();

            // Loans
            CreateMap<LoanViewModel, LoanEntity>();
            CreateMap<LoanEntity, LoanDTO>();
            CreateMap<LoanDTO, UserLoanDTO>();
            CreateMap<LoanEntity, UserLoanDTO>();
            CreateMap<LoanEntity, BookLoanDTO>();
            CreateMap<PatchLoanViewModel, LoanViewModel>();
            CreateMap<LoanViewModel, PatchLoanViewModel>();
        }
    }
}