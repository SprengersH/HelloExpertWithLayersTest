using Application.Services;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<(IEnumerable<User>, PaginationMetadata)> GetUsersAsync(
        string? name, string? searchQuery, int pageNumber, int pageSize);
    Task<User> GetUserAsync(int id);
    Task AddUser(User user);
}