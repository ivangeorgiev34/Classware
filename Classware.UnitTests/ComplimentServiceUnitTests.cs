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

            await complimentService.AddComplimentAsync("da1fd710-0374-4180-82d0-ef94021be249", "ad5e1c2d-ec25-420b-8d8b-b25edc4e5f70", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test", "");

            Assert.That((await repo.All<Compliment>().Where(c => c.Title == "test").FirstOrDefaultAsync())?.Title, Is.EqualTo("test"));
        }

        [Test]
        public async Task Test_MethodDeleteComplimentByIdAsyncShouldThrowExceptionWhenCannotFindCompliment()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            await complimentService.AddComplimentAsync("dc888754-7f2a-4b97-bd44-88af02b14e2e", "c4ea2f98-2636-4117-bbbf-9797bfecfbea", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test", "");


            Assert.ThrowsAsync<NullReferenceException>(async () => await complimentService.DeleteComplimentByIdAsync("dc888754-7f2a-4b97-bd44-88af02b14e2e"));
        }

        [Test]
        public async Task Test_MethodDeleteComplimentByIdAsyncShouldDeleteProperCompliment()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            await complimentService.AddComplimentAsync("200ee2eb-03b7-4e11-ad53-cd84baadf4b5", "70479b76-401e-4cd0-9013-628ccd3d2417", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test", "");

            await complimentService.DeleteComplimentByIdAsync(repo.All<Compliment>().First().Id.ToString());

            Assert.That(repo.All<Compliment>().First()?.IsActive, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_MethodEditComplimentByIdAsyncShouldThrowExceptionWhenCannotFindCompliment()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            await complimentService.AddComplimentAsync("d8f87545-eac7-42e1-bbc9-82caf3d4104e", "7fd79a64-9e2d-45b8-90d6-647f0b848084", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test", "");

            Assert.ThrowsAsync<NullReferenceException>(async () => await complimentService.EditComplimentByIdAsync("e6d828cc-09ac-484f-8502-a7d0bdd333e6", "", ""));
        }

        [Test]
        public async Task Test_MethodEditComplimentByIdAsyncShouldEditProperly()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            await complimentService.AddComplimentAsync("1e0c261f-6fba-419e-931e-d162b811c777", "fc0b617f-ebd9-4377-a134-0cbaf5c032dc", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test", "");

            var complimentId =  repo.All<Compliment>().First().Id.ToString();

            await complimentService.EditComplimentByIdAsync(complimentId, "edited title", "edited description");

            Assert.That((await repo.All<Compliment>().Where(c => c.Id == new Guid(complimentId)).FirstOrDefaultAsync())?.Title, Is.EqualTo("edited title"));

            Assert.That((await repo.All<Compliment>().Where(c => c.Id == new Guid(complimentId)).FirstOrDefaultAsync())?.Description, Is.EqualTo("edited description"));
        }

        [Test]
        public async Task Test_MethodGetComplimentByIdAsyncShouldThrowExceptionWhenCannotFindCompliment()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            await complimentService.AddComplimentAsync("73c3a142-d2b3-48d8-af6f-64e74f93f761", "8f79d5f8-4cc1-49bb-9b05-29ba59a469c0", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test", "");

            Assert.ThrowsAsync<NullReferenceException>(async () => await complimentService.GetComplimentByIdAsync("609bf89b-a1ac-4a86-b1b9-7810bd1bc884"));
        }

        [Test]
        public async Task Test_MethodGetComplimentByIdAsync()
        {
            repo = new Repository(dbContext);
            complimentService = new ComplimentService(repo);

            var student = new Student()
            {
                Id = new Guid("7943a892-769d-4670-bf21-256e2e32c731"),
                User = new ApplicationUser()
                {
                    Id = "f2eb23b1-adad-4225-b50f-e5c4cc309146"
                }
            };

            var teacher = new Teacher()
            {
                Id = new Guid("d2951c03-4a5a-49ad-8641-a37791cca18e"),
                User = new ApplicationUser()
                {
                    Id = "fa57882a-1a8d-495c-9b48-07943b983dac"
                }
            };
            await repo.AddAsync(student);

            await repo.AddAsync(teacher);

            await complimentService.AddComplimentAsync("7943a892-769d-4670-bf21-256e2e32c731", "d2951c03-4a5a-49ad-8641-a37791cca18e", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test", "");

            var complimentId = repo.All<Compliment>().First().Id.ToString();

            Assert.That((await complimentService.GetComplimentByIdAsync(complimentId)).Title, Is.EqualTo
                ("test"));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
