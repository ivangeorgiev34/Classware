using Classware.Core.Contracts;
using Classware.Core.Services;
using Microsoft.AspNetCore.SignalR;

namespace Classware.Hubs
{
	public class ChatHub : Hub
	{
		private readonly IMessageService messageService;
		public ChatHub(IMessageService _messageService)
		{
			messageService = _messageService;
		}

		public async Task SendMessageToAdmins(string fullName, string email, string title, string description)
		{
			var messageId = await messageService.AddMessageAsync(fullName, email, title, description);

			await this.Clients.Group("Admin").SendAsync("RecieveMessage", fullName, email, title, description, messageId);
		}

		public async Task AddAdmin()
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
		}

		public async Task SetMessageToAnswered(string messageId)
		{
			await messageService.SetMessageToAnsweredAsync(messageId);	
		}
	}
}
