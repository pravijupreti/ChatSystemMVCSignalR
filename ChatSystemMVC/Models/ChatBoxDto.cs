using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChatSystemMVC.Models
{
    public class ChatBoxDto
    {

        public List<string> FriendsNameList { get; set; }
        public string CurrentUserName { get; set; }
        public ChatBoxDto()
        {
            FriendsNameList = new List<string>();
        }
    }
}
