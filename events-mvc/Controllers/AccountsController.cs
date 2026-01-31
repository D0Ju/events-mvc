using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace events_mvc.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    private const string API_URL = "http://localhost:5117/api";

    public AccountController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            ViewBag.Error = "Lozinke se ne poklapaju!";
            return View();
        }

        var registerModel = new { email, password };
        var response = await _httpClient.PostAsJsonAsync($"{API_URL}/account/register", registerModel);

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Registracija uspješna! Prijavite se.";
            return RedirectToAction("Login");
        }

        ViewBag.Error = "Greška pri registraciji!";
        return View();
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var loginModel = new { email, password };
        var response = await _httpClient.PostAsJsonAsync($"{API_URL}/account/login", loginModel);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();  // ✅ CHANGE
            var token = result?.token;
            
            if (token != null)
            {
                Response.Cookies.Append("token", token, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,  // ✅ Set to false for localhost
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
                });

                TempData["Success"] = "Uspješno ste se prijavili!";
                return RedirectToAction("Index", "Home");
            }
        }

        ViewBag.Error = "Pogrešan email ili lozinka!";
        return View();
    }

    public IActionResult Logout()
    {
        Response.Cookies.Delete("token");
        TempData["Success"] = "Odjavljeni ste!";
        return RedirectToAction("Index", "Home");
    }
}

public class LoginResponse
{
    public string token { get; set; } = string.Empty;
}
