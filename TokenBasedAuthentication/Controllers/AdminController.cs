using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBasedAuthentication.Authentication;
using TokenBasedAuthentication.Helpers;
using TokenBasedAuthentication.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TokenBasedAuthentication.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        // GET: api/<AdminController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AdminController>/users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var admins = await userManager.GetUsersInRoleAsync(UserRoles.Admin);
            var users = await userManager.GetUsersInRoleAsync(UserRoles.User);

            return Ok(new Dictionary<string, dynamic>()
            {
                {"Admins", admins },
                {"Users", users },
            });
        }

        // POST api/<AdminController>/<id>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/<AdminController>/5
        [HttpPut("users/{id}")]
        public async Task<IActionResult> EditUser(string id, [FromBody] UserModel model)
        {
            var user = await userManager.FindByIdAsync(id);

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(500, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            var updatedUser = await userManager.FindByIdAsync(JwtHelper.GetUserIdByToken(id));

            return Ok(updatedUser.ToJson());
        }

        // DELETE api/<AdminController>/5
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if(user == null)
            {
                return NotFound();
            }

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(500, new Response { Status = "Error", Message = "User deletion failed! Please check user details and try again." });
            }

            return Ok();
        }
    }
}
