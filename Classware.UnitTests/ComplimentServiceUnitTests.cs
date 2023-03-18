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
    public class ComplimentServiceUnitTests
    {
        private IRepository repo;
        private ApplicationDbContext dbContext;
        private IComplimentService complimentService;


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
        public async Task Test_MethodAddComplimentAsyncShouldAddComplimentPropely()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            await complimentService.AddComplimentAsync(1, 2, 3, "test", "");

            Assert.That((await repo.All<Compliment>().Where(c => c.Title == "test").FirstOrDefaultAsync())?.Title, Is.EqualTo("test"));
        }

        [Test]
        public async Task Test_MethodDeleteComplimentByIdAsyncShouldThrowExceptionWhenCannotFindCompliment()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            await complimentService.AddComplimentAsync(1, 2, 3, "test", "");


            Assert.ThrowsAsync<NullReferenceException>(async () => await complimentService.DeleteComplimentByIdAsync(0));
        }

        [Test]
        public async Task Test_MethodDeleteComplimentByIdAsyncShouldDeleteProperCompliment()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            await complimentService.AddComplimentAsync(1, 2, 3, "test", "");

            await complimentService.DeleteComplimentByIdAsync(1);

            Assert.That(repo.All<Compliment>().Where(c => c.Id == 1).FirstOrDefault()?.IsActive, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_MethodEditComplimentByIdAsyncShouldThrowExceptionWhenCannotFindCompliment()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            await complimentService.AddComplimentAsync(1, 2, 3, "test", "");

            Assert.ThrowsAsync<NullReferenceException>(async () => await complimentService.EditComplimentByIdAsync(0, "", ""));
        }

        [Test]
        public async Task Test_MethodEditComplimentByIdAsyncShouldEditProperly()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            await complimentService.AddComplimentAsync(1, 2, 3, "test", "");

            await complimentService.EditComplimentByIdAsync(1, "edited title", "edited description");

            Assert.That((await repo.All<Compliment>().Where(c => c.Id == 1).FirstOrDefaultAsync())?.Title, Is.EqualTo("edited title"));

            Assert.That((await repo.All<Compliment>().Where(c => c.Id == 1).FirstOrDefaultAsync())?.Description, Is.EqualTo("edited description"));

            //check if the compliment is active
        }

        [Test]
        public async Task Test_MethodGetComplimentByIdAsyncShouldThrowExceptionWhenCannotFindCompliment()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            await complimentService.AddComplimentAsync(1, 2, 3, "test", "");

            Assert.ThrowsAsync<NullReferenceException>(async () => await complimentService.GetComplimentByIdAsync(1));
        }

        [Test]
        public async Task Test_MethodGetComplimentByIdAsync()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            var student = new Student()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "f2eb23b1-adad-4225-b50f-e5c4cc309146"
                }
            };

            var teacher = new Teacher()
            {
                Id = 2,
                User = new ApplicationUser()
                {
                    Id = "fa57882a-1a8d-495c-9b48-07943b983dac"
                }
            };
            await repo.AddAsync(student);

            await repo.AddAsync(teacher);

            await complimentService.AddComplimentAsync(1, 2, 3, "test", "");

            Assert.That((await complimentService.GetComplimentByIdAsync(1)).Title, Is.EqualTo
                ("test"));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
