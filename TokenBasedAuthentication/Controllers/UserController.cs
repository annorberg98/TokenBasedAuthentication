using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        // GET api/<UserController>
        [HttpGet]
        public async Task<IActionResult> Get([FromHeader] string authorization)
        {
            var user = await userManager.FindByIdAsync(JwtHelper.GetUserIdByToken(authorization));
            if (user != null)
            {
                return Ok(user.ToJson());
            }
            return NotFound();
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>
        [HttpPut]
        public async Task<IActionResult> Put([FromHeader] string authorization, [FromBody] UserModel model)
        {
            var user = await userManager.FindByIdAsync(JwtHelper.GetUserIdByToken(authorization));

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(500, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            var updatedUser = await userManager.FindByIdAsync(JwtHelper.GetUserIdByToken(authorization));

            return Ok(updatedUser.ToJson());
        }
    }
}
