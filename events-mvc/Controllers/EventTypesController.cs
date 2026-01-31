using Microsoft.AspNetCore.Mvc;
using events_mvc.Models;
using events_mvc.Services;

namespace events_mvc.Controllers;

public class EventTypesController : Controller
{
    private readonly IEventTypeService _eventTypeService;

    public EventTypesController(IEventTypeService eventTypeService)
    {
        _eventTypeService = eventTypeService;
    }

    // GET: /EventTypes
    public async Task<IActionResult> Index()
    {
        var types = await _eventTypeService.GetAllAsync();
        return View(types);
    }

    // GET: /EventTypes/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var type = await _eventTypeService.GetByIdAsync(id);
        if (type == null) return NotFound();
        return View(type);
    }

    // GET: /EventTypes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /EventTypes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventTypeViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _eventTypeService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // GET: /EventTypes/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var type = await _eventTypeService.GetByIdAsync(id);
        if (type == null) return NotFound();
        return View(type);
    }

    // POST: /EventTypes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EventTypeViewModel model)
    {
        if (id != model.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _eventTypeService.UpdateAsync(id, model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // GET: /EventTypes/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var type = await _eventTypeService.GetByIdAsync(id);
        if (type == null) return NotFound();
        return View(type);
    }

    // POST: /EventTypes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _eventTypeService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
