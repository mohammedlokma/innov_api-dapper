using innov_web.Models.DTO;

namespace innov_web.Services.Interfaces
{
    public interface IGroupService
    {
        Task<T> CreateAsync<T>(GroupDto dto);
        Task<T> DeleteGroupAsync<T>(int groupId);
        Task<T> UnDeleteGroupAsync<T>(int groupId);
        Task<T> GetAllAsync<T>();
        Task<T> GetGroupAsync<T>(int id);
        Task<T> GetGroupVerbsAsync<T>(int id);
        Task<T> UpdateAsync<T>(GroupDto dto);
    }
}
