using Application.Models;
using AutoMapper;
using Core.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private const int maxUserPageSize = 20;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ??
                              throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithoutTagsDto>>> GetUsers(string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxUserPageSize)
            {
                pageSize = maxUserPageSize;
            }
            //var users = await _userRepository.GetUsersAsync();

            //return Ok((users));

            var (userEntities, paginationMetadata) = await _userRepository
                .GetUsersAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<UserWithoutTagsDto>>(userEntities));
        }

        //[HttpPost]
        //public async Task AddUser(User user)
        //{
        //    await _userRepository.AddUser(user);
        //}

        [HttpPost]
        public async Task<ActionResult<UserForCreationDto>> AddUserWithDto(UserForCreationDto user)
        {
            var finalUser = _mapper.Map<User>(user);
            await _userRepository.AddUser(finalUser);
            var createdUserToReturn = _mapper.Map<UserDto>(finalUser);
            return CreatedAtRoute("GetUser",
                new
                {
                    createdUserToReturn.Id
                }, createdUserToReturn);
        }
        

        [HttpGet("{id}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
