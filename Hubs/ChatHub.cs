using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace DenimERP.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMESSAGE _message;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _accessor;

        public ChatHub(IMESSAGE message,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor accessor)
        {
            _message = message;
            _userManager = userManager;
            _accessor = accessor;
        }

        public async Task SendMessage(string userId, string userName, string message)
        {
            var currentUser = await _userManager.GetUserAsync(_accessor?.HttpContext?.User as ClaimsPrincipal);

            var insertedObjByAsync = await _message.GetInsertedObjByAsync(new MESSAGE
            {
                ReceiverName = string.IsNullOrEmpty(userId) ? currentUser.UserName : userName,
                ReceiverId = string.IsNullOrEmpty(userId) ? currentUser.Id : userId,
                Text = message,
                SendAt = DateTime.Now,
                SenderId = currentUser.Id
            });

            var isCurrentUser = insertedObjByAsync.ReceiverId == currentUser.Id;
            var receiver = await _userManager.FindByIdAsync(string.IsNullOrEmpty(userId) ? currentUser.Id : userId);

            await Clients.All.SendAsync("ReceiveMessage", string.IsNullOrEmpty(userId) ? currentUser.Id : userId, string.IsNullOrEmpty(userId) ? currentUser.UserName : userName, message, $"{insertedObjByAsync.SendAt:M/d/yyyy hh:mm:ss tt}", isCurrentUser, currentUser.PhotoPath, receiver.PhotoPath);
        }
    }
}
