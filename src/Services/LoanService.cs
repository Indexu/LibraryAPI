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
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository loanRepository;
        private readonly IMapper mapper;

        public LoanService(ILoanRepository loanRepository, IMapper mapper)
        {
            this.loanRepository = loanRepository;
            this.mapper = mapper;
        }

        public Envelope<LoanDTO> GetLoans(int pageNumber, int? pageMaxSize, int? userID, int? bookID, DateTime? date, bool? monthSpan)
        {
            try
            {
                return loanRepository.GetLoans(pageNumber, pageMaxSize, userID, bookID, date, monthSpan);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LoanDTO GetLoanByID(int loanID)
        {
            try
            {
                return loanRepository.GetLoanByID(loanID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddLoan(LoanViewModel loan)
        {
            try
            {
                return loanRepository.AddLoan(loan);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReplaceLoan(int loanID, LoanViewModel loan)
        {
            try
            {
                loanRepository.UpdateLoan(loanID, loan);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLoan(int loanID, PatchLoanViewModel loan)
        {
            try
            {
                var oldLoan = loanRepository.GetLoanByID(loanID);

                MergeLoansForPatch(loan, oldLoan);

                var newLoan = mapper.Map<PatchLoanViewModel, LoanViewModel>(loan);

                loanRepository.UpdateLoan(loanID, newLoan);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLoanByID(int loanID)
        {
            try
            {
                loanRepository.DeleteLoanByID(loanID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MergeLoansForPatch(PatchLoanViewModel loan, LoanDTO oldLoan)
        {
            // if (loan.BookID.HasValue)
            // {
            //     loan.BookID = oldLoan.Book.ID;
            // }

            // if (loan.UserID.HasValue)
            // {
            //     loan.UserID = oldLoan.User.ID;
            // }

            // if (loan.LoanDate.HasValue)
            // {
            //     loan.LoanDate = oldLoan.LoanDate;
            // }

            // if (loan.ReturnDate.HasValue)
            // {
            //     loan.ReturnDate = oldLoan.ReturnDate;
            // }
        }
    }
}
