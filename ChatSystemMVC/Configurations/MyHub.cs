using ChatSystemMVC.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace ChatSystemMVC.Configurations
{
    public class MyHub : Hub
    {
        private readonly IChatServices _chatServices;
        private readonly ILogger<MyHub> _logger;
        private static Dictionary<string, List<string>> _userGroups = new Dictionary<string, List<string>>();
        private static Dictionary<string, string> _userGroupMap = new Dictionary<string, string>();

        public MyHub(IChatServices chatServices, ILogger<MyHub> logger)
        {
            _logger = logger;
            _chatServices = chatServices;
        }

        public async Task ConnectToChat(List<string> userIds, string groupName)
        {
            // Add the user to the group
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            foreach (var userId in userIds)
            {
                // Update the user-group mapping
                if (_userGroups.ContainsKey(groupName))
                {
                    _userGroups[groupName].Add(userId);
                }
                else
                {
                    _userGroups[groupName] = new List<string> { userId };
                }
            }
            await Clients.Caller.SendAsync("ConnectToChat", false);
        }


        public async Task CheckUserInGroup(string userId, string groupName)
        {
            if (_userGroups.ContainsKey(groupName) && _userGroups[groupName].Contains(userId))
            {
                // User is in the group
                await Clients.Caller.SendAsync("UserInGroupStatus", true);
            }
            else
            {
                // User is not in the group
                await Clients.Caller.SendAsync("UserInGroupStatus", false);
            }
        }

        public async Task GetMessages(List<string> userIds)
        {
            try
            {
                var response = await _chatServices.GetLatestChatMessage(userIds).ConfigureAwait(false);

                // Send the messages to the calling client
                await Clients.Caller.SendAsync("ReceiveMessages", response);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveError", ex.Message);
                _logger.LogWarning(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        //public async Task GetNewMessage(MessageDto messageDto)
        //{
        //    try
        //    {
        //        var response = await _chatServices.GetMessage(messageDto).ConfigureAwait(false);

        //        // Send the messages to the calling client
        //        await Clients.Caller.SendAsync("Messages", response);
        //    }
        //    catch (Exception ex)
        //    {
        //        await Clients.Caller.SendAsync("ReceiveError", ex.Message);
        //        _logger.LogWarning(ex.Message);
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
    public class EventSourceMiddleware
    {
        private readonly RequestDelegate _next;

        public EventSourceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Accept", out var acceptHeader)
                && acceptHeader.Contains("text/event-stream"))
            {
                context.Response.Headers.Add("Content-Type", "text/event-stream");
            }
            var connectionID = context.Connection.Id;
            await _next(context);
        }
    }
}
