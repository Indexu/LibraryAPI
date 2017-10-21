using System;
using System.Collections.Generic;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Interfaces.Services;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using AutoMapper;

namespace LibraryAPI.Services
{
    public class ReportingService : AbstractService, IReportingService
    {
        private readonly ILoanRepository loanRepository;

        public ReportingService(ILoanRepository loanRepository, IMapper mapper)
            : base(mapper)
        {
            this.loanRepository = loanRepository;
        }

        public Envelope<UserReportDTO> GetUsersReport(int pageNumber, int? pageMaxSize, DateTime? loanDate, int? duration)
        {
            try
            {
                return loanRepository.GetUsersReport(pageNumber, pageMaxSize, loanDate, duration);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Envelope<BookReportDTO> GetBooksReport(int pageNumber, int? pageMaxSize, DateTime? loanDate, int? duration)
        {
            try
            {
                return loanRepository.GetBooksReport(pageNumber, pageMaxSize, loanDate, duration);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
