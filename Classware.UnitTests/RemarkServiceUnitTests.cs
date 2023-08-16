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

            await remarkService.AddRemarkAsync("31a1f859-2295-4bb3-9bc2-874ee47e8ef7", "4b514cf9-5fe6-40c8-af4c-1ab9ad494dc1", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test", "");

            Assert.That( repo.All<Remark>().First()?.Title, Is.EqualTo("test"));
        }

        [Test]
        public async Task Test_MethodDeleteRemarkByIdAsyncShouldThrowExceptionWhenCannotFindRemark()
        {
            repo = new Repository(dbContext);
            remarkService = new RemarkService(repo);

            await remarkService.AddRemarkAsync("45b4c09f-d583-467c-8980-f9f25f680f93", "7108c9bc-3e1a-418e-967a-3a5d42b23fd3", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test", "");

            Assert.ThrowsAsync<NullReferenceException>(async () => await remarkService.DeleteRemarkByIdAsync("d6a30476-b960-4395-ab2f-46f021ebbcff"));
        }

        [Test]
        public async Task Test_MethodDeleteRemarkByIdAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            remarkService = new RemarkService(repo);

            await remarkService.AddRemarkAsync("eb4526d8-5814-4497-8479-98c71ee15fce", "065665f1-a3fc-43e8-b6c8-7aba8764bdae", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test", "");

            var remarkId = repo.All<Remark>().First().Id.ToString();

            await remarkService.DeleteRemarkByIdAsync(remarkId);

            Assert.That((await repo.All<Remark>().FirstOrDefaultAsync(r => r.Id == new Guid(remarkId)))?.IsActive, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_MethodEditRemarkByIdAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            remarkService = new RemarkService(repo);

            var student = new Student()
            {
                Id = new Guid("49f64c6e-b869-4393-a3cc-fff4f4e320f3"),
                User = new ApplicationUser()
                {
                    Id = "b8751ee64ccf4852aef8aa2f417bf58a"
                }
            };

            var teacher = new Teacher()
            {
                Id = new Guid("9e97798e-6557-4c1c-828b-babfeaed3ed7"),
                User = new ApplicationUser()
                {
                    Id = "59e4dc7b018c49639ba8de8f5f75d09d"
                }
            };

            await repo.AddAsync(student);

            await repo.AddAsync(teacher);

            await remarkService.AddRemarkAsync("49f64c6e-b869-4393-a3cc-fff4f4e320f3", "9e97798e-6557-4c1c-828b-babfeaed3ed7", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test title", "test description");

            var remarkId = repo.All<Remark>().First().Id.ToString();

			await remarkService.EditRemarkByIdAsync(remarkId, "edited title", "edited description");

            Assert.That(repo.All<Remark>().First()?.Title, Is.EqualTo("edited title"));

            Assert.That(repo.All<Remark>().First()?.Description, Is.EqualTo("edited description"));
        }

        [Test]
        public async Task Test_MethodGetRemarkByIdAsyncShouldThrowException()
        {
            repo = new Repository(dbContext);
            remarkService = new RemarkService(repo);

			var student = new Student()
			{
				Id = new Guid("49f64c6e-b869-4393-a3cc-fff4f4e320f3"),
				User = new ApplicationUser()
				{
					Id = "b8751ee64ccf4852aef8aa2f417bf58a"
				}
			};

			var teacher = new Teacher()
			{
				Id = new Guid("9e97798e-6557-4c1c-828b-babfeaed3ed7"),
				User = new ApplicationUser()
				{
					Id = "59e4dc7b018c49639ba8de8f5f75d09d"
				}
			};

			await repo.AddAsync(student);

            await repo.AddAsync(teacher);

            await remarkService.AddRemarkAsync("49f64c6e-b869-4393-a3cc-fff4f4e320f3", "9e97798e-6557-4c1c-828b-babfeaed3ed7", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test", "");

            Assert.ThrowsAsync<NullReferenceException>(async () => await remarkService.GetRemarkByIdAsync("2cfadbc9-f8f6-4980-94d0-7a06fbe324bc"));
        }

        [Test]
        public async Task Test_MethodGetRemarkByIdAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            remarkService = new RemarkService(repo);

			var student = new Student()
			{
				Id = new Guid("49f64c6e-b869-4393-a3cc-fff4f4e320f3"),
				User = new ApplicationUser()
				{
					Id = "b8751ee64ccf4852aef8aa2f417bf58a"
				}
			};

			var teacher = new Teacher()
			{
				Id = new Guid("9e97798e-6557-4c1c-828b-babfeaed3ed7"),
				User = new ApplicationUser()
				{
					Id = "59e4dc7b018c49639ba8de8f5f75d09d"
				}
			};

			await repo.AddAsync(student);

            await repo.AddAsync(teacher);

            await remarkService.AddRemarkAsync("49f64c6e-b869-4393-a3cc-fff4f4e320f3", "9e97798e-6557-4c1c-828b-babfeaed3ed7", "e60f5411-9d93-458c-82c6-1e45cc1888a6", "test", "");

            var remarkId = repo.All<Remark>().First().Id.ToString();

            Assert.That((await remarkService.GetRemarkByIdAsync(remarkId))?.Title,Is.EqualTo("test"));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
