using WinterSportAcademy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WinterSportAcademy.Services;
namespace WinterSportAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ITraineeService _traineeService;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            EmailService emailService,
            IConfiguration configuration,
            ITraineeService traineeService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
            _traineeService = traineeService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(TraineeRegisterModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, UserRoles.Trainee);

            var trainee = await _traineeService.CreateAsync(new TraineeDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                SkillLevel = model.SkillLevel
            });

            await _userManager.AddClaimAsync(user, new Claim("TraineeId", trainee.TraineeId.ToString()));

            return Ok("User registered successfully.");
        }
        // Add an action to handle email verification
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email verification successful.");
            }
            return BadRequest("Email verification failed.");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Unauthorized("Invalid login attempt.");
                }

                var roles = await _userManager.GetRolesAsync(user);
                var token = await GenerateJwtToken(user, roles);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid login attempt.");
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();//then click thay on the server side it would remove tooken
            return Ok("Logged out");
        }
        private async Task<string> GenerateJwtToken(IdentityUser user, IList<string> roles)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
            var subject = user.Email ?? user.UserName ?? user.Id;

            var claims = new List<Claim>
{
new Claim(JwtRegisteredClaimNames.Sub, subject),
new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
};
            // Add roles as claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"]));
            var token = new JwtSecurityToken(
            jwtIssuer,
            jwtIssuer,
            claims,
            expires: expires,
            signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
