using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Infrastructure.Data;
using AutoMapper;
using Infrastructure.Repositories;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public TagController(UserDbContext context, IMapper mapper, IUserRepository userRepository)
        {
            _context = context;
            _mapper = mapper ??
                      throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ??
                              throw new ArgumentNullException(nameof(userRepository));
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTag()
        {
            return await _context.Tags.ToListAsync();
        }

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }

        // PUT: api/Tags/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }

            _context.Entry(tag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tags
        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTag", new { id = tag.Id }, tag);
        }



        [HttpPost("AddUserTag", Name = "AddUserTag")]
        public async Task<ActionResult<TagDto>> AddTagToUser(int userId, TagForCreationDto tag)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
            {
                return NotFound();
            }

            var finalTag = _mapper.Map<Tag>(tag);

            var user = await _userRepository.GetUserAsync(userId, false);
            if (user == null)
            {
                return NotFound();
            }
            user.UserTags.Add(finalTag);

            await _context.SaveChangesAsync();

            var createdTagToReturn =
                _mapper.Map<TagDto>(finalTag);

            return CreatedAtRoute("AddUserTag",
                new
                {
                    userId = userId,
                    tagId = createdTagToReturn.Id
                },
                createdTagToReturn);
        }




        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }
    }
}
