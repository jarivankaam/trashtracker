namespace trashtracker1.Controllers;
using Microsoft.AspNetCore.Mvc;

[Route("auth")]
public class AuthController : Controller
{
    [HttpGet("set-cookie")]
    public IActionResult SetCookie(string token)
    {
        Response.Cookies.Append("authToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(1)
        });

        return Redirect("/Home");
    }
}
