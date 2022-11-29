using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserAsync(int id);
    Task AddUser(User user);
}