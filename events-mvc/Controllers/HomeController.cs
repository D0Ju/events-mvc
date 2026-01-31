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
        var upcoming = await _eventService.UpcomingEventsAsync();
        ViewBag.TotalEvents = (await _eventService.GetAllAsync()).Count;
        return View(upcoming);
    }
}

