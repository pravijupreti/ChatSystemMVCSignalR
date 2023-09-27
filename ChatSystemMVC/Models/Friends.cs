using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Principal;

namespace ChatSystemMVC.Models
{
    public class Friends
    {
        public string Id { get; set; }
        public string FriendId { get; set; }

        public string UserId { get; set; }
        public User Users { get; set; }
    }
}
