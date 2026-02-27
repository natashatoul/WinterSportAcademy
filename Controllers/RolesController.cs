using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterSportAcademy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace WinterSportAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]// only the Admin should have access to line 13 endpoint
    //The [Authorize] attribute is temporarily commented out for the system bootstrapping phase. Since we are working with a fresh database, no 'Admin' role or administrative user exists yet. Disabling this restriction allows me to initialize the core roles and assign the first administrator. Once the initial RBAC (Role-Based Access Control) structure is in place, I will re-enable the attribute to secure the endpoints.
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RolesController> _logger;

        public RolesController(
            RoleManager<IdentityRole> roleManager, 
            UserManager<IdentityUser> userManager,
            ILogger<RolesController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: api/Roles
        [HttpGet]
        public IActionResult GetRoles()
        {
            _logger.LogInformation("Request the list of all roles.");
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        // GET: api/Roles/{roleId}
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound("Role not found.");
            return Ok(role);
        }

        // GET: api/Roles/user-roles/{userId}
        [HttpGet("user-roles/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
            {
                _logger.LogWarning("User with ID {UserId} not found.", userId);
                return NotFound("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest("Role name cannot be empty.");

            if (await _roleManager.RoleExistsAsync(roleName))
                return BadRequest("This role already exists.");

            _logger.LogInformation("Creating a new role: {RoleName}", roleName);
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            
            if (result.Succeeded)
                return Ok($"Role '{roleName}' created successfully.");

            return BadRequest(result.Errors);
        }

        // PUT: api/Roles
        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null) return NotFound("Role not found.");

            if (role.Name == UserRoles.Admin)
                return BadRequest("The 'Admin' role name is fixed and cannot be changed.");

            _logger.LogInformation("Updating role ID {Id}. New name: {NewName}", model.RoleId, model.NewRoleName);
            role.Name = model.NewRoleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded) return Ok("Role updated successfully.");
            return BadRequest(result.Errors);
        }

        // DELETE: api/Roles/{roleId}
        [HttpDelete("{roleId}")]
        [Authorize(Roles = UserRoles.Admin)] 
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound("Role not found.");

            if (role.Name == UserRoles.Admin)
            {
                _logger.LogCritical("Attempt to delete the Admin role was blocked!");
                return BadRequest("Deleting the 'Admin' role is prohibited.");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                _logger.LogInformation("Role {RoleName} deleted successfully.", role.Name);
                return Ok("Role deleted successfully.");
            }
            return BadRequest(result.Errors);
        }

        // POST: api/Roles/assign-role-to-user
        [HttpPost("assign-role-to-user")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound("User not found.");

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists) return NotFound($"Role '{model.RoleName}' does not exist.");

            _logger.LogInformation("Assigning role {Role} to user {UserEmail}", model.RoleName, user.Email);
            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (result.Succeeded) return Ok("Role assigned to user successfully.");
            return BadRequest(result.Errors);
        }
    }
}