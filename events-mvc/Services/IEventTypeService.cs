using events_mvc.Models;

namespace events_mvc.Services{
    public interface IEventTypeService
    {
        Task<List<EventTypeViewModel>> GetAllAsync();
        Task<EventTypeViewModel?> GetByIdAsync(int id);
        Task<int> CreateAsync(EventTypeViewModel model);
        Task<bool> UpdateAsync(int id, EventTypeViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}