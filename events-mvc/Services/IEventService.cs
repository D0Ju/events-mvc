using events_mvc.Models;

namespace events_mvc.Services{
    public interface IEventService
    {
        Task<List<EventViewModel>> GetAllAsync();
        Task<EventViewModel?> GetByIdAsync(int id);
        Task<int> CreateAsync(EventViewModel model);
        Task<bool> UpdateAsync(int id, EventViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<List<EventViewModel>> SearchByLocationAsync(string? lokacija);
        Task<List<EventViewModel>> UpcomingEventsAsync();
    }
}