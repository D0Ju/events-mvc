using events_mvc.Models;
using System.Text.Json;
using System.Net.Http.Headers;

namespace events_mvc.Services;

public class EventTypeService : IEventTypeService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EventTypeService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EventTypeService(HttpClient httpClient, ILogger<EventTypeService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _httpClient.BaseAddress = new Uri("http://localhost:5117"); // YOUR API PORT!
    }
    private void AddAuthHeader()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context?.Request.Cookies.TryGetValue("token", out var token) == true)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<EventTypeViewModel>> GetAllAsync()
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync("api/eventtypes");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<EventTypeViewModel>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<EventTypeViewModel>();
    }

    public async Task<EventTypeViewModel?> GetByIdAsync(int id)
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync($"api/eventtypes/{id}");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EventTypeViewModel>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<int> CreateAsync(EventTypeViewModel model)
    {
        AddAuthHeader();
        var response = await _httpClient.PostAsJsonAsync("api/eventtypes", model);
        response.EnsureSuccessStatusCode();
        var created = await GetByIdAsync(model.Id);
        return created?.Id ?? 0;
    }

    public async Task<bool> UpdateAsync(int id, EventTypeViewModel model)
    {
        AddAuthHeader();
        var response = await _httpClient.PutAsJsonAsync($"api/eventtypes/{id}", model);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        AddAuthHeader();
        var response = await _httpClient.DeleteAsync($"api/eventtypes/{id}");
        return response.IsSuccessStatusCode;
    }


}
