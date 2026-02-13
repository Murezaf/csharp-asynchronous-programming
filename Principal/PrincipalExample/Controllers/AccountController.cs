namespace PrincipalExample.Controllers;

public class AccountController : Controller
{
	public async Task<IActionResult> Login()
	{
		var user = HttpContext.User;

		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, "testuser"),
			new Claim(ClaimTypes.Role, "Admin")
		};
		var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
		var principal = new ClaimsPrincipal(identity);

		await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).ConfigureAwait(ConfigureAwaitOptions.ForceYielding | ConfigureAwaitOptions.None);

		return RedirectToAction("Index", "Home");
	}

	public async Task<IActionResult> Logout()
	{
		var user = User;

		await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(ConfigureAwaitOptions.ForceYielding | ConfigureAwaitOptions.None);
		return RedirectToAction("Index", "Home");
	}
}