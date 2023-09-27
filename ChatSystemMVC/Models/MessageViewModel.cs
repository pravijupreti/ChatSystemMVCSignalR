namespace ChatSystemMVC.Models
{
    public class MessageViewModel
    {
        public List<MessageDto> MessageDto { get; set; }
        public string CurrentUserId { get; set; }

        public string SecondUserID { get; set; }

        public string ConnectionRoom { get; set; }

        public MessageViewModel()
        {
            MessageDto = new List<MessageDto>();
        }
    }
}
