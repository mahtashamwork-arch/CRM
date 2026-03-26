using CRM.API.Data;
using CRM.API.DTO.User;
using CRM.API.Entities;
using CRM.API.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly AppDbContext _context;
        public UserController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        [HttpGet]
       public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsers()
        {
            var users = await _context.Users
                .AsNoTracking()
                .Select(c => new UserReadDto
                {
                    id=c.id,
                    FirstName=c.FirstName,
                    LastName=c.LastName,
                    Email=c.Email,
                    Phone=c.Phone,
                    Address=c.Address,
                    CreateAt=c.CreateAt,
                    UpdateAt=c.UpdateAt
                }).ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserReadDto>> GetById(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(c => c.id == id)
                .Select(c => new UserReadDto
                {
                    id = c.id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.Phone,
                    Address = c.Address,
                    CreateAt = c.CreateAt,
                    UpdateAt = c.UpdateAt
                }).FirstOrDefaultAsync();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserReadDto>> CreateUser(UserCreateDto userCreateDto)
        {
            var userEmail = await _context.Users.AnyAsync(c => c.Email == userCreateDto.Email);
            if (userEmail)
                return Conflict(new { message = $"User with this email {userCreateDto.Email} already exists" });
            var user = new User
            {
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                Email = userCreateDto.Email,
                Phone = userCreateDto.Phone,
                Address = userCreateDto.Address
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = new UserReadDto
            {
                id = user.id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                CreateAt = user.CreateAt,
                UpdateAt = user.UpdateAt
            };


            return CreatedAtAction(nameof(GetById), new {id=user.id},result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUserByid(int id,UserUpdateDto userUpdateDto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is null)
                return NotFound(new { message = $"user with id {id} not found" });
            user.FirstName = userUpdateDto.FirstName;
            user.LastName = userUpdateDto.LastName;
            user.Email = userUpdateDto.Email;
            user.Phone = userUpdateDto.Phone;
            user.Address = userUpdateDto.Address;
            user.UpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is null)
                return NotFound(new { message = $"user with id {id} not found" });
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
