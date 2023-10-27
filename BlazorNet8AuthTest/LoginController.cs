using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorNet8AuthTest
{
    public class LoginController : Controller
    {
        [IgnoreAntiforgeryToken]
        [HttpPost]
        [Route("/user-login")]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync("USER");

            var claims = new List<Claim> { new("Name", "Kalai") };
            var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "login"));

            await HttpContext.SignInAsync("USER", claimPrincipal);

            if (Request.Query.ContainsKey("returnurl") &&
                !string.IsNullOrWhiteSpace(Request.Query["returnurl"]))
            {
                return Redirect(Request.Query["returnurl"]);
            }

            return Redirect("/");
        }

        [HttpGet]
        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("USER");
            return Redirect("/login");
        }
    }
}