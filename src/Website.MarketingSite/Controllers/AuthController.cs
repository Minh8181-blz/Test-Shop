using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Website.MarketingSite.Models.Dtos;
using Website.MarketingSite.Models.ViewModels;
using Website.MarketingSite.Services;

namespace Website.MarketingSite.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("/sign-up")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost("/sign-up")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var result = await _authService.SignUp(model);
                var result = new SignUpResultDto
                {
                    Succeeded = true,
                    User = new UserDto
                    {
                        Id = 123,
                        Email = "abc@gmail.comm"
                    },
                    Data = "test_jwt"
                };

                if (result.Succeeded)
                {
                    var token = result.Data as string;

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, result.User.Id.ToString()),
                        new Claim(ClaimTypes.Name, result.User.Email),
                        new Claim("Token", result.Data as string)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                    await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

                    return Ok(User?.Claims);
                }
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            // todo

            return BadRequest(allErrors);
        }

        [HttpGet]
        [Authorize]
        public IActionResult TestCookie()
        {
            var user = User;
            return Ok(user.Claims.Select(x => x.Value));
        }
    }
}
