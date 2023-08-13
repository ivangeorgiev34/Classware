using Classware.Core.Contracts;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Core.Services
{
	public class MessageService : IMessageService
	{
		private readonly IRepository repo;
		public MessageService(IRepository _repo)
		{
			this.repo = _repo;
		}

		public async Task<string> AddMessageAsync(string fullName, string email, string title, string description)
		{
			var message = new Message()
			{
				//chekc if id should be included as guid explicitly
				Email = email,
				FullName = fullName,
				Description = description,
				Title = title,
				IsAnswered = false
			};

			await repo.AddAsync(message);

			await repo.SaveChangesAsync();

			return message.Id.ToString();
		}

		public async Task<List<Message>> GetAllMessagesAsync()
		{
			return await repo.All<Message>()
				.Where(m => m.IsAnswered == false)
				.ToListAsync();
		}

		public async Task SetMessageToAnsweredAsync(string messageId)
		{
			var message = await repo.All<Message>()
				.FirstOrDefaultAsync(m => m.Id == new Guid(messageId));

			if (message != null)
			{
				message.IsAnswered = true;

				await repo.SaveChangesAsync();
			}
		}
	}
}
