using ChatSystemMVC.ApplicationDBcontext;
using ChatSystemMVC.Models;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;

namespace ChatSystemMVC.Services
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext Context) : base(Context)
        {
            _context = Context;
        }

        public async Task<User> GetUserAsync(string id)
        {
            var user = await _context.Users
                 .Include(u => u.Friends) // Include the Friends navigation property
                   .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<List<string>> GetUserIds(List<string> UserNames)
        {
            var user = await _context.Users.Where(x => UserNames.Contains(x.Name)).ToListAsync();
            return user.Select(x => x.Id).ToList();
        }

        public async Task<string> GetUsersChatGroup(List<string> userIds)
        {
            var users = await _context.Users.Where(x => userIds.Contains(x.Id)).ToListAsync();
            var groupName = users.FirstOrDefault().ChatRoom;
            return groupName;
        }

        public async Task<List<User>> GetUsersFriends(List<string> userId)
        {
            var usersFriends = await _context.Users.Where(x => userId.Contains(x.Id)).ToListAsync();
            return usersFriends;
        }
    }
}
