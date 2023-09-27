namespace ChatSystemMVC.IServices
{
    public interface IChatServices
    {
        Task SendMessage(MessageDto messageDto);
        Task<IEnumerable<MessageDto>> GetChatMessage(List<string> userIds);

        Task<IEnumerable<MessageDto>> GetLatestChatMessage(List<string> userIds);

        public string SecretGroupName(string user1, string user2);
    }
}
