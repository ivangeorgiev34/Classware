using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Data;
using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.UnitTests
{
    [TestFixture]
    public class RemarkServiceUnitTests
    {
        private IRepository repo;
        private ApplicationDbContext dbContext;
        private IRemarkService remarkService;

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
        public async Task Test_MethodAddRemarkAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            remarkService = new RemarkService(repo);

            await remarkService.AddRemarkAsync(1, 1, 1, "test", "");

            Assert.That((await repo.All<Remark>().FirstOrDefaultAsync(r => r.Id == 1))?.Title, Is.EqualTo("test"));
        }

        [Test]
        public async Task Test_MethodDeleteRemarkByIdAsyncShouldThrowExceptionWhenCannotFindRemark()
        {
            repo = new Repository(dbContext);
            remarkService = new RemarkService(repo);

            await remarkService.AddRemarkAsync(1, 1, 1, "test", "");

            Assert.ThrowsAsync<NullReferenceException>(async () => await remarkService.DeleteRemarkByIdAsync(0));
        }

        [Test]
        public async Task Test_MethodDeleteRemarkByIdAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            remarkService = new RemarkService(repo);

            await remarkService.AddRemarkAsync(1, 1, 1, "test", "");

            await remarkService.DeleteRemarkByIdAsync(1);

            Assert.That((await repo.All<Remark>().FirstOrDefaultAsync(r => r.Id == 1))?.IsActive, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_MethodEditRemarkByIdAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            remarkService = new RemarkService(repo);

            var student = new Student()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "b8751ee64ccf4852aef8aa2f417bf58a"
                }
            };

            var teacher = new Teacher()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "59e4dc7b018c49639ba8de8f5f75d09d"
                }
            };

            await repo.AddAsync(student);

            await repo.AddAsync(teacher);

            await remarkService.AddRemarkAsync(1, 1, 1, "test title", "test description");

            await remarkService.EditRemarkByIdAsync(1, "edited title", "edited description");

            Assert.That((await repo.All<Remark>().FirstOrDefaultAsync(r => r.Id == 1))?.Title, Is.EqualTo("edited title"));

            Assert.That((await repo.All<Remark>().FirstOrDefaultAsync(r => r.Id == 1))?.Description, Is.EqualTo("edited description"));
        }

        [Test]
        public async Task Test_MethodGetRemarkByIdAsyncShouldThrowException()
        {
            repo = new Repository(dbContext);
            remarkService = new RemarkService(repo);

            var student = new Student()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "b8751ee64ccf4852aef8aa2f417bf58a"
                }
            };

            var teacher = new Teacher()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "59e4dc7b018c49639ba8de8f5f75d09d"
                }
            };

            await repo.AddAsync(student);

            await repo.AddAsync(teacher);

            await remarkService.AddRemarkAsync(1, 1, 1, "test", "");

            Assert.ThrowsAsync<NullReferenceException>(async () => await remarkService.GetRemarkByIdAsync(0));
        }

        [Test]
        public async Task Test_MethodGetRemarkByIdAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            remarkService = new RemarkService(repo);

            var student = new Student()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "b8751ee64ccf4852aef8aa2f417bf58a"
                }
            };

            var teacher = new Teacher()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "59e4dc7b018c49639ba8de8f5f75d09d"
                }
            };

            await repo.AddAsync(student);

            await repo.AddAsync(teacher);

            await remarkService.AddRemarkAsync(1, 1, 1, "test", "");

            Assert.That((await remarkService.GetRemarkByIdAsync(1))?.Title,Is.EqualTo("test"));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
