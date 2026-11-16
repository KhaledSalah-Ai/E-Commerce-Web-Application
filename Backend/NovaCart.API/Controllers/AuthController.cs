using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NovaCart.Application.DTOs;
using NovaCart.Application.Interfaces;
using NovaCart.Domain.Entities;

namespace NovaCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        // 1. تسجيل حساب جديد (POST: api/auth/register)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // التحقق مما إذا كان البريد الإلكتروني مستخدماً مسبقاً
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email is already in use." });
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // توليد الـ Token للمستخدم الجديد مباشرة بعد التسجيل
            var token = _tokenService.CreateToken(user.Id, user.Email, user.FirstName, user.LastName);

            var response = new AuthResponseDto
            {
                Message = "User registered successfully",
                IsAuthenticated = true,
                Token = token,
                Email = user.Email,
                ExpiresOn = DateTime.UtcNow.AddDays(7)
            };

            return Ok(response);
        }

        // 2. تسجيل الدخول (POST: api/auth/login)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid Email or Password." });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid Email or Password." });
            }

            // توليد الـ Token عند نجاح تسجيل الدخول
            var token = _tokenService.CreateToken(user.Id, user.Email!, user.FirstName, user.LastName);

            var response = new AuthResponseDto
            {
                Message = "Logged in successfully",
                IsAuthenticated = true,
                Token = token,
                Email = user.Email!,
                ExpiresOn = DateTime.UtcNow.AddDays(7)
            };

            return Ok(response);
        }
    }
}