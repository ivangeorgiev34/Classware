using Microsoft.AspNetCore.SignalR;

namespace Classware.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToAdmins(string fullName, string email,string title,string description)
        {
            await this.Clients.Group("Admin").SendAsync("RecieveMessage",fullName,email,title,description);
        }

        public async Task AddAdmin()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
        }
    }
}
