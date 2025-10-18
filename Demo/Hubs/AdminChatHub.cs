using Demo.Infrastructure;
using Demo.Models;
using Demo.Models.Entities;
using Demo.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Demo.Hubs
{
    [Authorize(Roles = "Admin")]
    public class AdminChatHub : Hub
    {
       
        private IMessageQueue _queue;
        private static readonly ConcurrentBag<string> OnlineAdmins = new ConcurrentBag<string>();
        public AdminChatHub(IMessageQueue queue) {
            _queue = queue;
        }

        public override async Task OnConnectedAsync()
        {
            var user = Context?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(user))
                return;

            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");

            var userAlreadyOnline = OnlineAdmins.Contains(user);

            if(!userAlreadyOnline)
            {
                OnlineAdmins.Add(user);
                await Clients.All.SendAsync("UpdateOnlineList", OnlineAdmins.ToList());
            }

            //passing the Online List to the client so on refresh it will have the online admins list ready to display
            await Clients.Caller.SendAsync("ReceiveOnlineAdminsList", OnlineAdmins.ToList());

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = Context?.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(user))
            {
                // Remove user from online admins
                var updatedList = OnlineAdmins.Where(u => u != user).ToList();
                OnlineAdmins.Clear();
                foreach (var admin in updatedList)
                {
                    OnlineAdmins.Add(admin);
                }

                // Notify all clients of the updated list
                await Clients.All.SendAsync("UpdateOnlineList", OnlineAdmins.ToList());
                
                // Remove from group
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            var user = Context?.User?.Identity?.Name;
            var userId = Context.UserIdentifier?.ToString();
            var currentTime = DateTime.UtcNow;

            //currently send to everyone, later make the admin group
            await Clients.All.SendAsync("ReceiveMessage",message, user, currentTime);

            var messageModel = new Message
            {
                MessageContent = message,
                UserId = userId,
                UserName = user,
                SentAt = currentTime,
            };

            //add message to Messages queue for the background services
            _queue.Enqueue(messageModel);
        }

        public async Task SendTyper(bool isTyping)
        {
            var user = Context?.User?.Identity?.Name;
            await Clients.Others.SendAsync("Typing", isTyping, user);
        }
    }
}
