using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Data;
using Classware.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.UnitTests
{
	[TestFixture]
	public class MessageServiceUnitTests
	{
		private IRepository repo;
		private ApplicationDbContext dbContext;
		private IMessageService messageService;

		[SetUp]
		public void SetUp()
		{
			var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("ClasswareDb")
				.Options;

			dbContext = new ApplicationDbContext(contextOptions);

			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
		}

		[Test]
		public async Task Test_MethodAddMessageAsyncShouldWorkProperly()
		{
			repo = new Repository(dbContext);

			messageService = new MessageService(repo);

			var message = new Message()
			{
				Id = 1,
				Email = "example@abv.bg",
				Description = "",
				Title = "",
				FullName = "Test",
				IsAnswered = false
			};

			await messageService.AddMessageAsync(message.FullName, message.Email, message.Title, message.Description);

			var expectedMessage = await repo.All<Message>().FirstOrDefaultAsync(m => m.Id == message.Id);

			Assert.That(message.Id, Is.EqualTo(expectedMessage?.Id));
		}

		[Test]
		public async Task Test_MethodSetMessageToAnsweredAsyncShouldWorkProperly()
		{
			repo = new Repository(dbContext);

			messageService = new MessageService(repo);

			var message = new Message()
			{
				Id = 1,
				Email = "example@abv.bg",
				Description = "",
				Title = "",
				FullName = "Test",
				IsAnswered = false
			};

			await messageService.AddMessageAsync(message.FullName,message.Email,message.Title, message.Description);

			var allMessages = await messageService.GetAllMessagesAsync();

			await messageService.SetMessageToAnsweredAsync(1);

			Assert.That(allMessages[0].IsAnswered, Is.True);

		}

		[Test]
		public async Task Test_MethodSetMessageToAnsweredAsyncShouldNotSetIsAnsweredToTrueIfMessageWithSuchIdDoesNotExist()
		{
			repo = new Repository(dbContext);

			messageService = new MessageService(repo);

			var message = new Message()
			{
				Id = 1,
				Email = "example@abv.bg",
				Description = "",
				Title = "",
				FullName = "Test",
				IsAnswered = false
			};

			await messageService.AddMessageAsync(message.FullName, message.Email, message.Title, message.Description);

			var allMessages = await messageService.GetAllMessagesAsync();

			await messageService.SetMessageToAnsweredAsync(5);

			Assert.That(allMessages[0].IsAnswered, Is.False);

		}

		[Test]
		public async Task Test_MethodGetAllMessagesAsyncShouldWorkProperly()
		{
			repo = new Repository(dbContext);

			messageService = new MessageService(repo);

			var firstMessage = new Message()
			{
				Id = 1,
				Email = "example1@abv.bg",
				Description = "1",
				Title = "1",
				FullName = "Test1",
				IsAnswered = false
			};

			var secondMessage = new Message()
			{
				Id = 2,
				Email = "example2@abv.bg",
				Description = "2",
				Title = "2",
				FullName = "Test2",
				IsAnswered = false
			};

			await messageService.AddMessageAsync(firstMessage.FullName,firstMessage.Email,firstMessage.Title, firstMessage.Description);

			await messageService.AddMessageAsync(secondMessage.FullName, secondMessage.Email, secondMessage.Title, secondMessage.Description);

			await messageService.SetMessageToAnsweredAsync(firstMessage.Id);

			var allMessages = await messageService.GetAllMessagesAsync();

			Assert.That(firstMessage.Id,Is.EqualTo(allMessages.Count));	
		}

		[TearDown]
		public void TearDown()
		{
			dbContext.Dispose();
		}
	}
}
