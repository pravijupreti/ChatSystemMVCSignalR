﻿using ChatSystemMVC.Models;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace ChatSystemMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChatServices _chatServices;
        private readonly IHubContext<MyHub> _hubContext;
        private static Dictionary<string, List<string>> _userGroups = new Dictionary<string, List<string>>();
        public HomeController(ILogger<HomeController> logger,IChatServices chatServices,IHubContext<MyHub> hubContext)
        {
            _hubContext = hubContext;
            _chatServices = chatServices;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ChatBox()
        {
            return View();
        }

        [Route("/chat")]
        [HttpGet]
        public async Task<IActionResult> GetMessage(string name)
        {
            try
            {
                var string2 = "string";
                var stringGroup = string.Empty;
                if (name.Equals("Ram"))
                {
                    stringGroup = "stringGroup";
                }
                if(name.Equals("Hari"))
                {
                    stringGroup = "stringGroup1";
                }
                // Connect users to the chat
                //await _hubContext.Clients.All.SendAsync("ConnectToChat", string1);
                //await _hubContext.Clients.All.SendAsync("ConnectToChat", string2);
                //var groupName = _chatServices.SecretGroupName(name,string2);
                // Check if users are in the specified group
                //await _hubContext.Clients.All.SendAsync("CheckUserInGroup", string1, stringGroup);
                //await _hubContext.Clients.All.SendAsync("CheckUserInGroup", string2, stringGroup);
                //await _hubContext.Clients.All.SendAsync("ConnectToChat", new List<string> { name, string2 }, stringGroup);

                // Get chat messages and latest chat message
                var response = await _chatServices.GetChatMessage(new List<string> { name, string2 }).ConfigureAwait(false);
                var abc = await _chatServices.GetLatestChatMessage(new List<string> { name, string2 }).ConfigureAwait(false);

                // Send the latest chat message to the specified group
                await _hubContext.Clients.Group(stringGroup).SendAsync("ReceiveMessages", abc);
                if (response.Count() == default)
                {
                    response = response.Concat(new[] { new MessageDto
                    {
                          SenderId = string2,
                         To = name,
                    }});
                }
                var messageResponse = new MessageViewModel();
                messageResponse.MessageDto.AddRange(response.ToList());
                messageResponse.CurrentUserId = string2;
                messageResponse.SecondUserID = name;
                messageResponse.ConnectionRoom = stringGroup;
                return View(messageResponse);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage( MessageDto messageDto)
        {
            messageDto.Id = "assd";
            await _chatServices.SendMessage(messageDto).ConfigureAwait(false);
            return RedirectToAction("GetMessage", new { name = messageDto.To});
            //return RedirectToAction("GetMessage", "Home", new { name = "yourNameValue" });

        }
    }
}