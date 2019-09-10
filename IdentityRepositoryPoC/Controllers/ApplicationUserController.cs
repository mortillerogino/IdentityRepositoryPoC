using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityRepositoryPoC.Data.Models;
using IdentityRepositoryPoC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityRepositoryPoC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        //POST : /api/ApplicationUser/Register
        public async Task<IActionResult> PostApplicationUser(ApplicationUserDto dto)
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, dto.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
                
            }
        }
    }
}