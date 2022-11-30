﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbContext;

        public UserRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IEnumerable<User> AllUsers()

        {
            return _dbContext.Users;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _dbContext.Users.OrderBy(u => u.FirstName).ToListAsync();
        }


        public async Task<(IEnumerable<User>, PaginationMetadata)> GetUsersAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _dbContext.Users as IQueryable<User>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.FirstName == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.FirstName.Contains(searchQuery)
                                                   || (a.LastName.Contains(searchQuery) || a.Email.Contains(searchQuery)));
            }


            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(c => c.FirstName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }
        public async Task<User> GetUserAsync(int id)
        {
            return (await _dbContext.Users.FindAsync(id))!;
        }

        public async Task AddUser(User user)
        {
            _dbContext.Add(user);
           await SaveChangesAsync(); // dont forget this if you dont want to lose another day
           
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }

        

    }
}
