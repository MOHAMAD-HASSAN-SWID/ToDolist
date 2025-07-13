using Microsoft.AspNetCore.Mvc;
using to_do_list.Domain.Entities;
using to_do_list.Application.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace to_do_list.WebAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        [Flags]
        public enum enum_permission
        {
            all_permission = -1,
            shoow_all_dolist = 1,
            show_dolist_bycategory = 2,
            show_dolist_priority = 4,
            show_dolist_list_id = 8,
            change_state_dolist = 16,
            add_users = 32,
            add_dolist = 64,
            update_dolist = 128,
            remove_dolist = 256
        }

        public UsersController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("SignIn", Name = "SignIn")]

        public async Task<IActionResult> SignIn(int id, string userName)
        {
            var user = await _userService.GetUserAsync(id, userName);

            if (user == null)
                return NotFound("User not found");

            var token = GenerateJwtToken(user); 

            return Ok(new
            {
                message = "User signed in successfully",
                user = user,
                token = token
            });
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> Add(string UserName, int Permission, DateTime DateRegistered)
        {
            enum_permission eper = (enum_permission)GetPermissionFromToken();
            if (!eper.HasFlag(enum_permission.add_users) && !eper.HasFlag(enum_permission.all_permission))
                return BadRequest("you don't have permission");

            var id = await _userService.AddUserAsync(UserName, Permission, DateRegistered);
            if (id == -1)
                return BadRequest("Something went wrong while adding the user");

            return Ok($"User added with ID: {id}");
        }


        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, (user.Permission & 32) == 32 ? "Owner" : "Guest"),
                new Claim("permission", user.Permission.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private int GetPermissionFromToken()
        {
            var permissionClaim = User.FindFirst("permission");
            var permission = permissionClaim != null ? int.Parse(permissionClaim.Value) : 0;
            return permission;
        }
    }
}
