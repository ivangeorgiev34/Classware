using Classware.Areas.Administrator.Models.Chat;
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
		public async Task<IActionResult> AllMessages(int page = 1)
		{
			var messages = await messageService.GetAllMessagesAsync();

			var model = new AllMessagesViewModel()
			{
				Page = page,
				TotalMessages = messages.Count()
			};

			var paginatedMessages = messages
				.Skip((page - 1) * 4)
				.Take(4)
				.ToList();

			foreach (var message in paginatedMessages)
			{
				model.Messages.Add(new ChatViewModel()
				{
					Id = message.Id.ToString(),
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
