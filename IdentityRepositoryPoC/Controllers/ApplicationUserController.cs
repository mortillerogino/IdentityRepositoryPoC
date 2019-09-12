using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityRepositoryPoC.Data.Data;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<AppSettings> _appSettings;
        private const string _iSSUER = "THIS SITE";

        public ApplicationUserController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
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
                Email = dto.Email,
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, dto.Password);
                await _userManager.AddClaimAsync(applicationUser, new Claim("UserID", applicationUser.Id.ToString()));

                if (dto.IsAdmin)
                {
                    await _userManager.AddClaimAsync(applicationUser, new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, _iSSUER));
                }

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
                    var claims = await _userManager.GetClaimsAsync(user);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
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
                return BadRequest("Username does not exist.");
            }
        }

        
        [HttpGet]
        [Authorize]
        //Get : /api/ApplicationUser
        public async Task<IActionResult> GetCurrentUser()
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


        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route("All")]
        //Get : /api/ApplicationUser/All
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _unitOfWork.UserRepository.Get();

                if (users.Count == 0)
                {
                    return BadRequest();
                }

                var userDtos = new List<ApplicationUserDto>();
                foreach (ApplicationUser u in users)
                {
                    userDtos.Add(new ApplicationUserDto
                    {
                        UserName = u.UserName,
                        Email = u.Email,
                    });
                }

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            
        }


    }
}