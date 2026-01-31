using Microsoft.AspNetCore.Mvc;
using events_mvc.Models;
using events_mvc.Services;

namespace events_mvc.Controllers;

public class EventsController : Controller
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    // GET: /Events
    public async Task<IActionResult> Index()
    {
        var events = await _eventService.GetAllAsync();
        return View(events);
    }

    // GET: /Events/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var ev = await _eventService.GetByIdAsync(id);
        if (ev == null) return NotFound();
        return View(ev);
    }

    // GET: /Events/UpcomingEvents
    public async Task<IActionResult> UpcomingEvents()
    {
        var upcoming = await _eventService.UpcomingEventsAsync();
        return View(upcoming);
    }

    // GET: /Events/SearchByLocation?location=zagreb
    public async Task<IActionResult> SearchByLocation(string? location)
    {
        ViewBag.SearchTerm = location;
        var events = await _eventService.SearchByLocationAsync(location);
        return View("Index", events);
    }

    // GET: /Events/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Events/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _eventService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // GET: /Events/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var ev = await _eventService.GetByIdAsync(id);
        if (ev == null) return NotFound();
        return View(ev);
    }

    // POST: /Events/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EventViewModel model)
    {
        if (id != model.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _eventService.UpdateAsync(id, model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // GET: /Events/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var ev = await _eventService.GetByIdAsync(id);
        if (ev == null) return NotFound();
        return View(ev);
    }

    // POST: /Events/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _eventService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
