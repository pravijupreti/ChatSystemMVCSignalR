namespace ChatSystemMVC.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ChatRoom { get; set; }
        public IList<Friends> Friends { get; set; }
    }
}