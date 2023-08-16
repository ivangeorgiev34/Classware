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
				Id= new Guid("46a48bdb-8c97-400a-9479-00dbe54472f5"),
				Email = "example@abv.bg",
				Description = "",
				Title = "",
				FullName = "Test",
				IsAnswered = false
			};

			await messageService.AddMessageAsync(message.FullName, message.Email, message.Title, message.Description);

			var expectedMessage = await repo.All<Message>().FirstOrDefaultAsync();

			Assert.That(message.Email, Is.EqualTo(expectedMessage?.Email));
		}

		[Test]
		public async Task Test_MethodSetMessageToAnsweredAsyncShouldWorkProperly()
		{
			repo = new Repository(dbContext);

			messageService = new MessageService(repo);

			var message = new Message()
			{
				Id = new Guid("c3b94935-1cbc-4d56-9e03-540fafc3ddf6"),
				Email = "example@abv.bg",
				Description = "",
				Title = "",
				FullName = "Test",
				IsAnswered = false
			};

			await messageService.AddMessageAsync(message.FullName,message.Email,message.Title, message.Description);

			var allMessages = await messageService.GetAllMessagesAsync();

			await messageService.SetMessageToAnsweredAsync(allMessages.First().Id.ToString());

			Assert.That(allMessages[0].IsAnswered, Is.True);

		}

		[Test]
		public async Task Test_MethodSetMessageToAnsweredAsyncShouldNotSetIsAnsweredToTrueIfMessageWithSuchIdDoesNotExist()
		{
			repo = new Repository(dbContext);

			messageService = new MessageService(repo);

			var message = new Message()
			{
				Id = new Guid("5a88df67-33e3-42ae-84df-06e94acd7701"),
				Email = "example@abv.bg",
				Description = "",
				Title = "",
				FullName = "Test",
				IsAnswered = false
			};

			await messageService.AddMessageAsync(message.FullName, message.Email, message.Title, message.Description);

			var allMessages = await messageService.GetAllMessagesAsync();

			await messageService.SetMessageToAnsweredAsync("5a88df67-33e3-42ae-84df-06e94acd7701");

			Assert.That(allMessages[0].IsAnswered, Is.False);

		}

		[Test]
		public async Task Test_MethodGetAllMessagesAsyncShouldWorkProperly()
		{
			repo = new Repository(dbContext);

			messageService = new MessageService(repo);

			var firstMessage = new Message()
			{
				Id = new Guid("51aadf89-de5b-4f08-a03f-b15521e4f3d2"),
				Email = "example1@abv.bg",
				Description = "1",
				Title = "1",
				FullName = "Test1",
				IsAnswered = false
			};

			var secondMessage = new Message()
			{
				Id = new Guid("a07ea973-9eac-48be-b5eb-5bcf2d3787ff"),
				Email = "example2@abv.bg",
				Description = "2",
				Title = "2",
				FullName = "Test2",
				IsAnswered = false
			};

			await messageService.AddMessageAsync(firstMessage.FullName,firstMessage.Email,firstMessage.Title, firstMessage.Description);

			await messageService.AddMessageAsync(secondMessage.FullName, secondMessage.Email, secondMessage.Title, secondMessage.Description);

			await messageService.SetMessageToAnsweredAsync(repo
				.AllReadonly<Message>()
				.First().Id.ToString());

			var allMessages = await messageService.GetAllMessagesAsync();

			var expectedCount = 1;

			Assert.That(expectedCount, Is.EqualTo(allMessages.Count));	
		}

		[TearDown]
		public void TearDown()
		{
			dbContext.Dispose();
		}
	}
}
