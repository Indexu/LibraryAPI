using System.Collections.Generic;
using System.Linq;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models;
using LibraryAPI.Exceptions;
using AutoMapper;

using Newtonsoft.Json;
using System;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace LibraryAPI.Repositories.EntityFrameworkCore
{
    public class UserRepository : AbstractRepository, IUserRepository
    {
        private readonly ILoanRepository loanRepository;

        private const string notFoundMessage = "User not found";
        private const string alreadyExistsMessage = "User with that email already exists";

        public UserRepository(DatabaseContext db, IMapper mapper, ILoanRepository loanRepository)
            : base(db, mapper)
        {
            this.loanRepository = loanRepository;
        }

        public Envelope<UserDTO> GetUsers(int pageNumber, int? pageMaxSize)
        {
            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);
            var totalNumberOfItems = db.Users.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var userEntites = db.Users
                                .OrderBy(u => u.ID)
                                .Skip((pageNumber - 1) * maxSize)
                                .Take(maxSize)
                                .ToList();

            var userDTOs = Mapper.Map<IList<UserEntity>, IList<UserDTO>>(userEntites);

            return new Envelope<UserDTO>
            {
                Items = userDTOs,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = userDTOs.Count,
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public UserDetailsDTO GetUserByID(int userID, bool loanHistory, int pageNumber, int? pageMaxSize)
        {
            var userEntity = db.Users.Where(u => u.ID == userID).SingleOrDefault();

            if (userEntity == null)
            {
                throw new NotFoundException(notFoundMessage);
            }

            var userDetailsDTO = mapper.Map<UserEntity, UserDetailsDTO>(userEntity);

            if (loanHistory)
            {
                var history = loanRepository.GetLoansByUserID(userID, false, pageNumber, pageMaxSize);

                userDetailsDTO.LoanHistory = history;
            }

            return userDetailsDTO;
        }

        public int AddUser(UserViewModel user)
        {
            // Check if exists by email
            if (db.Users.Where(u => u.Email == user.Email).Any())
            {
                throw new AlreadyExistsException(alreadyExistsMessage);
            }

            // Add user
            var userEntity = mapper.Map<UserViewModel, UserEntity>(user);

            db.Users.Add(userEntity);
            db.SaveChanges();

            return userEntity.ID;
        }

        public void UpdateUser(int userID, UserViewModel user)
        {
            // Get entity from DB
            var userEntity = db.Users.Where(u => u.ID == userID).SingleOrDefault();

            if (userEntity == null)
            {
                throw new NotFoundException(notFoundMessage);
            }

            // Check if email change and make sure email is still unique
            if (userEntity.Email != user.Email && db.Users.Where(u => u.Email == user.Email).Any())
            {
                throw new AlreadyExistsException(alreadyExistsMessage);
            }

            userEntity.Name = user.Name;
            userEntity.Address = user.Address;
            userEntity.Email = user.Email;

            db.Users.Update(userEntity);
            db.SaveChanges();
        }

        public void DeleteUserByID(int userID)
        {
            var userEntity = db.Users.Where(u => u.ID == userID).SingleOrDefault();

            if (userEntity == null)
            {
                throw new NotFoundException(notFoundMessage);
            }

            db.Users.Remove(userEntity);
            db.SaveChanges();
        }

        /// <summary>
        /// Reseed the database.
        /// <para />
        /// Place this method call at the start of any method in this repository
        /// and call that function using the API, then remove it after it has run.
        /// </summary>
        private void Reseed()
        {
            // Warning. Ugly code ahead.
            // I was going to not include this code with the hand-in, but I want to show you
            // how I got the data into the database.

            db.Database.ExecuteSqlCommand("DELETE FROM Loans;DELETE FROM sqlite_sequence WHERE name='Loans';");
            db.Database.ExecuteSqlCommand("DELETE FROM Reviews;DELETE FROM sqlite_sequence WHERE name='Reviews';");
            db.Database.ExecuteSqlCommand("DELETE FROM Books;DELETE FROM sqlite_sequence WHERE name='Books';");
            db.Database.ExecuteSqlCommand("DELETE FROM Users;DELETE FROM sqlite_sequence WHERE name='Users';");

            var books_inorder = true;
            var users_inorder = true;

            // Books
            using (StreamReader r = new StreamReader("../../SC-T-302-HONN 2017- Bækur.json"))
            {
                var json = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);

                var counter = 1;

                foreach (var item in array)
                {
                    var date = (string)item.utgafudagur;
                    var id = (int)item.bok_id;

                    if (books_inorder && counter == id)
                    {
                        counter++;
                    }
                    else
                    {
                        books_inorder = false;
                    }

                    var book = new BookEntity
                    {
                        Title = item.bok_titill,
                        Author = item.fornafn_hofundar + " " + item.eftirnafn_hofundar,
                        PublishDate = DateTime.Parse(date),
                        ISBN = item.ISBN
                    };

                    db.Books.Add(book);
                }

                db.SaveChanges();
            }

            // Users and loans
            using (StreamReader r = new StreamReader("../../SC-T-HONN-302 2017- Vinir.json"))
            {
                var json = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);

                var counter = 1;

                foreach (var item in array)
                {
                    var id = (int)item.vinur_id;

                    if (users_inorder && counter == id)
                    {
                        counter++;
                    }
                    else
                    {
                        users_inorder = false;
                    }

                    string address = null;

                    try
                    {
                        address = item.heimilisfang;
                    }
                    catch (NullReferenceException)
                    {
                        // User has no loaned books
                    }

                    var user = new UserEntity
                    {
                        Name = item.fornafn + " " + item.eftirnafn,
                        Email = item.netfang,
                        Address = address
                    };

                    db.Users.Add(user);

                    db.SaveChanges();

                    try
                    {
                        // Loans
                        foreach (var loan in item.lanasafn)
                        {
                            var date = (string)loan.bok_lanadagsetning;

                            var loanEntity = new LoanEntity
                            {
                                UserID = id,
                                BookID = (int)loan.bok_id,
                                LoanDate = DateTime.Parse(date)
                            };

                            db.Loans.Add(loanEntity);
                        }
                    }
                    catch (NullReferenceException)
                    {
                        // User has no loaned books
                    }

                    db.SaveChanges();
                }
            }

            throw new System.NotImplementedException("SEEDED | Books in order: " + books_inorder + " | Users in order: " + users_inorder);
        }
    }
}