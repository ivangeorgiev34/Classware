using Classware.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Core.Contracts
{
	public interface IMessageService
	{
		Task<string> AddMessageAsync(string fullName, string email, string title, string description);

		Task<List<Message>> GetAllMessagesAsync();

		Task SetMessageToAnsweredAsync(string messageId);
	}
}
