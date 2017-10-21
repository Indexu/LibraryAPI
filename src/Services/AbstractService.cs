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
    public abstract class AbstractService
    {
        protected readonly IMapper mapper;

        public AbstractService(IMapper mapper)
        {
            this.mapper = mapper;
        }
    }
}
