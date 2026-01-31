using Microsoft.AspNetCore.Mvc;
using events_mvc.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace events_mvc.Controllers;

public class EventTypesController : Controller
{
    private readonly HttpClient _httpClient;
    private const string API_URL = "http://localhost:5117/api/eventtypes";

    public EventTypesController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private void AddAuthHeader()
    {
        if (Request.Cookies.TryGetValue("token", out var token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync(API_URL);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Account");
            var types = await response.Content.ReadFromJsonAsync<List<EventTypeViewModel>>(); 
            return View(types ?? new List<EventTypeViewModel>());
        }
        catch
        {
            return View(new List<EventTypeViewModel>());
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"{API_URL}/{id}");
            
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var type = await response.Content.ReadFromJsonAsync<EventTypeViewModel>();
            return View(type);
        }
        catch
        {
            return NotFound();
        }
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventTypeViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                AddAuthHeader();
                var response = await _httpClient.PostAsJsonAsync(API_URL, model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Vrsta kreirana!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Greška: {ex.Message}");
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"{API_URL}/{id}");
            
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var type = await response.Content.ReadFromJsonAsync<EventTypeViewModel>();  // ✅ CHANGE
            return View(type);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EventTypeViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                AddAuthHeader();
                var response = await _httpClient.PutAsJsonAsync($"{API_URL}/{id}", model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Vrsta ažurirana!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Greška: {ex.Message}");
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"{API_URL}/{id}");
            
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var type = await response.Content.ReadFromJsonAsync<EventTypeViewModel>();  // ✅ CHANGE
            return View(type);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.DeleteAsync($"{API_URL}/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Vrsta obrisana!";
                return RedirectToAction("Index");
            }
        }
        catch { }

        return RedirectToAction("Index");
    }
}
