namespace ChatSystemMVC.Models
{
    public class MessageDto
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public string To { get; set; }
        public string DateTime { get; set; }
        public string Reply { get; set; }
    }
}