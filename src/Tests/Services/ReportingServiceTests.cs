using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LibraryAPI.Services;
using LibraryAPI.Interfaces.Services;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Tests.MockUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace LibraryAPI.Tests.Services
{
    /// <summary>
    /// The unit tests for the ReportingService
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    [TestClass]
    public class ReportingServiceTests
    {
        /* 
         * ===================
         *   GetUsersReport
         * ===================
         */

        [TestMethod]
        public void GetUsersReportBasic()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            DateTime? loanDate = new DateTime(2000, 1, 1);
            int? duration = 30;
            var numberOfReports = 10;
            var reports = MockDataGenerator.CreateUserReports(numberOfReports);
            var envelope = MockDataGenerator.CreateUserReportsEnvelope(reports, pageNumber, pageSize);

            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.GetUsersReport(pageNumber, pageSize, loanDate, duration)).Returns(envelope);

            var service = new ReportingService(mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedReports = service.GetUsersReport(pageNumber, pageSize, loanDate, duration);

            // Assert
            mockLoanRepo.Verify(f => f.GetUsersReport(pageNumber, pageSize, loanDate, duration), Times.Once());
            Assert.IsNotNull(returnedReports);
            Assert.AreEqual(returnedReports.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedReports.Paging.PageMaxSize, 50);
            Assert.AreEqual(returnedReports.Paging.PageCount, 1);
            Assert.AreEqual(returnedReports.Paging.TotalNumberOfItems, numberOfReports);
            Assert.AreEqual(returnedReports.Items.First().User.ID, reports.First().User.ID);
            Assert.AreEqual(returnedReports.Items.Last().User.ID, reports.Last().User.ID);
        }

        [TestMethod]
        public void GetUsersReport2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;
            DateTime? loanDate = new DateTime(2000, 1, 1);
            int? duration = 30;
            var numberOfReports = 10;
            var reports = MockDataGenerator.CreateUserReports(numberOfReports);
            var envelope = MockDataGenerator.CreateUserReportsEnvelope(reports, pageNumber, pageSize);

            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.GetUsersReport(pageNumber, pageSize, loanDate, duration)).Returns(envelope);

            var service = new ReportingService(mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedReports = service.GetUsersReport(pageNumber, pageSize, loanDate, duration);

            // Assert
            mockLoanRepo.Verify(f => f.GetUsersReport(pageNumber, pageSize, loanDate, duration), Times.Once());
            Assert.IsNotNull(returnedReports);
            Assert.AreEqual(returnedReports.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedReports.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(returnedReports.Paging.PageCount, 2);
            Assert.AreEqual(returnedReports.Paging.TotalNumberOfItems, numberOfReports);
            Assert.AreEqual(returnedReports.Items.First().User.ID, reports.First().User.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetUsersReportException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            DateTime? loanDate = new DateTime(2000, 1, 1);
            int? duration = 30;

            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.GetUsersReport(pageNumber, pageSize, loanDate, duration)).Throws(new Exception());

            var service = new ReportingService(mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedReports = service.GetUsersReport(pageNumber, pageSize, loanDate, duration);
        }

        /* 
         * ===================
         *   GetBooksReport
         * ===================
         */

        [TestMethod]
        public void GetBooksReportBasic()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            DateTime? loanDate = new DateTime(2000, 1, 1);
            int? duration = 30;
            var numberOfReports = 10;
            var reports = MockDataGenerator.CreateBookReports(numberOfReports);
            var envelope = MockDataGenerator.CreateBookReportsEnvelope(reports, pageNumber, pageSize);

            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.GetBooksReport(pageNumber, pageSize, loanDate, duration)).Returns(envelope);

            var service = new ReportingService(mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedReports = service.GetBooksReport(pageNumber, pageSize, loanDate, duration);

            // Assert
            mockLoanRepo.Verify(f => f.GetBooksReport(pageNumber, pageSize, loanDate, duration), Times.Once());
            Assert.IsNotNull(returnedReports);
            Assert.AreEqual(returnedReports.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedReports.Paging.PageMaxSize, 50);
            Assert.AreEqual(returnedReports.Paging.PageCount, 1);
            Assert.AreEqual(returnedReports.Paging.TotalNumberOfItems, numberOfReports);
            Assert.AreEqual(returnedReports.Items.First().Book.ID, reports.First().Book.ID);
            Assert.AreEqual(returnedReports.Items.Last().Book.ID, reports.Last().Book.ID);
        }

        [TestMethod]
        public void GetBooksReport2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;
            DateTime? loanDate = new DateTime(2000, 1, 1);
            int? duration = 30;
            var numberOfReports = 10;
            var reports = MockDataGenerator.CreateBookReports(numberOfReports);
            var envelope = MockDataGenerator.CreateBookReportsEnvelope(reports, pageNumber, pageSize);

            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.GetBooksReport(pageNumber, pageSize, loanDate, duration)).Returns(envelope);

            var service = new ReportingService(mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedReports = service.GetBooksReport(pageNumber, pageSize, loanDate, duration);

            // Assert
            mockLoanRepo.Verify(f => f.GetBooksReport(pageNumber, pageSize, loanDate, duration), Times.Once());
            Assert.IsNotNull(returnedReports);
            Assert.AreEqual(returnedReports.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedReports.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(returnedReports.Paging.PageCount, 2);
            Assert.AreEqual(returnedReports.Paging.TotalNumberOfItems, numberOfReports);
            Assert.AreEqual(returnedReports.Items.First().Book.ID, reports.First().Book.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetBooksReportException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            DateTime? loanDate = new DateTime(2000, 1, 1);
            int? duration = 30;

            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.GetBooksReport(pageNumber, pageSize, loanDate, duration)).Throws(new Exception());

            var service = new ReportingService(mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.GetBooksReport(pageNumber, pageSize, loanDate, duration);
        }
    }
}
