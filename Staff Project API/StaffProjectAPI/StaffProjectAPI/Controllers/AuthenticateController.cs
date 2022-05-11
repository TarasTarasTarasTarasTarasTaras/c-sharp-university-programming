using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StaffProjectAPI.Data;
using StaffProjectAPI.Models;
using StaffProjectAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StaffProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticateService _authenticateService;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<User> userManager, IAuthenticateService service, IConfiguration configuration)
        {
            _userManager = userManager;
            _authenticateService = service;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if(user is null)
                return Unauthorized("The user with such username is not registered");

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = _authenticateService.GetAuthorizationToken(model.Username, _configuration);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            else
                return Unauthorized("The password is incorrect");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return BadRequest("User with such username already exists!");

            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Payments = new List<Payment>()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest("User creation failed! Please check user details and try again.");

            return Ok("User created successfully!");
        }
    }
}
