using Classware.Core.Contracts;
using Classware.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Administrator.Controllers
{
	public class ChatController : BaseController
	{
		private readonly IMessageService messageService;
        public ChatController(IMessageService _messageService)
        {
				this.messageService = _messageService;
        }

        [HttpGet]
		public async Task<IActionResult> AllMessages()
		{
			var messages = await messageService.GetAllMessagesAsync();

			var model = new List<ChatViewModel>();

			foreach (var message in messages)
			{
				model.Add(new ChatViewModel()
				{
					Id = message.Id,
					Email = message.Email,
					Description = message.Description,
					FullName = message.FullName,
					Title = message.Title
				});
			}

			return View(model);

		}
	}
}
