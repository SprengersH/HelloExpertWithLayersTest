using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task AddUser(User user)
        {
            _dbContext.Add(user);
           await _dbContext.SaveChangesAsync(); // dont forget this if you dont want to lose another day
           
        }
        
        public async Task<User> GetUserAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

    }
}
