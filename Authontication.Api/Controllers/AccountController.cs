using ApiRover.Errors;
using Authontication.Api.Dtos;
using Authontication.Core.Entities.Identity;

using Authontication.Core.intrtfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Authontication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthServices _authServices;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthServices authServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authServices = authServices;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));
            }

            var refreshToken = await _authServices.GenerateRefreshTokenAsync();
            TokenStore.AddToken(refreshToken, user.Id, TimeSpan.FromSeconds(30)); 
            var jwtToken = await _authServices.CreateTokenAsync(user);


            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = loginDto.Email,
                Token = await _authServices.CreateTokenAsync(user),
                RefreshToken = refreshToken
            });
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                PhoneNumber = registerDto.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded is false)
            {

                return BadRequest(new ApiResponse(400));
            }
           
            return Ok(new UserDto
            {

                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user),
               

            });
    }



        

    }
}