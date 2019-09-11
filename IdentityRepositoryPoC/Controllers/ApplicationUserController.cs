using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityRepositoryPoC.Data.Models;
using IdentityRepositoryPoC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityRepositoryPoC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<AppSettings> _appSettings;
        private const string _iSSUER = "THIS SITE";

        public ApplicationUserController(UserManager<ApplicationUser> userManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings;
        }

        [HttpPost]
        [Route("[action]")]
        //POST : /api/ApplicationUser/Register
        public async Task<IActionResult> Register(ApplicationUserDto dto)
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

        [HttpPost]
        [Route("[action]")]
        //POST : /api/ApplicationUser/Login
        public async Task<IActionResult> Login(ApplicationUserDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, dto.Password))
                {
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim("UserID", user.Id.ToString()),
                            //new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, ISSUER)
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(30),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Value.Secret)), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return Ok( new { token });
                }
                else
                {
                    return BadRequest("Password is incorrect.");
                }
            }
            else
            {
                return BadRequest("Username does not exit.");
            }
        }

        
        [HttpGet]
        [Authorize]
        //Get : /api/ApplicationUser
        public async Task<IActionResult> GetUser()
        {
            var claims = User.Claims;
            var currentUserId = claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(currentUserId);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email
            });
        }


    }
}