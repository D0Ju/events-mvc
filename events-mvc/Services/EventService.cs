using events_mvc.Models;
using System.Text.Json;

namespace events_mvc.Services;

public class EventService : IEventService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EventService> _logger;

    public EventService(HttpClient httpClient, ILogger<EventService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = new Uri("http://localhost:5117"); // YOUR API PORT!
    }

    public async Task<List<EventViewModel>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync("api/events");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<EventViewModel>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<EventViewModel>();
    }

    public async Task<EventViewModel?> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/events/{id}");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventViewModel>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<int> CreateAsync(EventViewModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/events", model);
        response.EnsureSuccessStatusCode();
        var created = await GetByIdAsync(model.Id);
        return created?.Id ?? 0;
    }

    public async Task<bool> UpdateAsync(int id, EventViewModel model)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/events/{id}", model);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/events/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<EventViewModel>> SearchByLocationAsync(string? location)
    {
        var url = $"api/events/filter?lokacija={Uri.EscapeDataString(location ?? "")}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<EventViewModel>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<EventViewModel>();
    }

    public async Task<List<EventViewModel>> UpcomingEventsAsync()
    {
        var all = await GetAllAsync();
        return all.Where(e => e.DatumPocetka > DateTime.Now.Date && e.Aktivan)
                  .OrderBy(e => e.DatumPocetka).Take(5).ToList();
    }
}
