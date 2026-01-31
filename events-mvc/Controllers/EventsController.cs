using Microsoft.AspNetCore.Mvc;
using events_mvc.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace events_mvc.Controllers;

public class EventsController : Controller
{
    private readonly HttpClient _httpClient;
    private const string API_URL = "http://localhost:5117/api/events";

    public EventsController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private void AddAuthHeader()
    {
        if (Request.Cookies.TryGetValue("token", out var token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
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

            var events = await response.Content.ReadFromJsonAsync<List<EventViewModel>>(); 
            return View(events ?? new List<EventViewModel>());
        }
        catch
        {
            return RedirectToAction("Login", "Account");
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

            var ev = await response.Content.ReadFromJsonAsync<EventViewModel>();  
            return View(ev);
        }
        catch
        {
            return NotFound();
        }
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                AddAuthHeader();
                var response = await _httpClient.PostAsJsonAsync(API_URL, model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Događaj kreiran!";
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

            var ev = await response.Content.ReadFromJsonAsync<EventViewModel>();
            
            // Load EventTypes for dropdown
            var typesResponse = await _httpClient.GetAsync("http://localhost:5117/api/eventtypes");
            var types = await typesResponse.Content.ReadFromJsonAsync<List<EventTypeViewModel>>();
            ViewBag.EventTypes = types ?? new List<EventTypeViewModel>();

            return View(ev);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EventViewModel model)
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
                    TempData["Success"] = "Događaj ažuriran!";
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

            var ev = await response.Content.ReadFromJsonAsync<EventViewModel>();
            return View(ev);
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
                TempData["Success"] = "Događaj obrisan!";
                return RedirectToAction("Index");
            }
        }
        catch { }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpcomingEvents()
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"{API_URL}/upcoming");
            
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var upcoming = await response.Content.ReadFromJsonAsync<List<EventViewModel>>(); 
            return View(upcoming ?? new List<EventViewModel>());
        }
        catch
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> SearchByLocation(string location){
        if (string.IsNullOrWhiteSpace(location))
            return RedirectToAction("Index");

        try
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync(API_URL);
            var allEvents = await response.Content.ReadFromJsonAsync<List<EventViewModel>>();
            
            var results = allEvents?
                .Where(e => e.Lokacija.Contains(location, StringComparison.OrdinalIgnoreCase))
                .ToList() ?? new List<EventViewModel>();
            
            ViewBag.SearchTerm = location;
            return View(results);
        }
        catch
        {
            ViewBag.SearchTerm = location;
            return View(new List<EventViewModel>());
        }
    }


}
