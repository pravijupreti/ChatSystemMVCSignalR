using ChatSystemMVC.Models;

namespace ChatSystemMVC.Services
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string id);

        Task<List<User>> GetUsersFriends(List<string> userId);

        Task<string> GetUsersChatGroup(List<string> userIds);

        Task<List<string>> GetUserIds(List<string> UserNames);
    }
}
