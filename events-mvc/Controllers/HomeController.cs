using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using events_mvc.Models;
using events_mvc.Services;

namespace events_mvc.Controllers;

public class HomeController : Controller
{
    private readonly IEventService _eventService;

    public HomeController(IEventService eventService)
    {
        _eventService = eventService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var upcoming = await _eventService.UpcomingEventsAsync();
            ViewBag.TotalEvents = (await _eventService.GetAllAsync()).Count;
            return View(upcoming);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("401"))
        {
            // User not logged in - show empty list
            return View(new List<EventViewModel>());
        }
        catch
        {
            // Any other error - show empty list
            return View(new List<EventViewModel>());
        }
    }
}
