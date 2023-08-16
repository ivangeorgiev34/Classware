using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Data;
using Classware.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.UnitTests
{
    [TestFixture]
    public class ClassServiceUnitTests
    {
        private IRepository repo;
        private ApplicationDbContext dbContext;
        private IClassService classService;

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
        public async Task Test_MethodAddClassShouldWorkProperly()
        {
            var repo = new Repository(dbContext);

            classService = new ClassService(repo);

            var _class = new Class()
            {
                Id = new Guid("cb9f8e37-0aa0-4578-96ee-e3ad6027df77"),
                Name = "Test"
            };

            await classService.AddClassAsync(_class);

            Assert.That(_class, Is.SameAs(await repo.All<Class>().Where(c => c.Id == _class.Id).FirstOrDefaultAsync()));
        }

        [Test]
        public async Task Test_MethodClassExistsByNameAsyncShouldReturnTrue()
        {
            var repo = new Repository(dbContext);

            classService = new ClassService(repo);

            var _class = new Class()
            {
                Id = new Guid("a91ea3eb-5e58-442e-bee6-2bf9afa7e36d"),
                Name = "Test"
            };

            await classService.AddClassAsync(_class);

            var result = await classService.ClassExistsByNameAsync(_class.Name);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task Test_MethodClassExistsByNameAsyncShouldReturnFalse()
        {
            var repo = new Repository(dbContext);

            classService = new ClassService(repo);

            var _class = new Class()
            {
                Id = new Guid("30d37823-9ae8-4276-ae91-5930a6ca908b"),
                Name = "Test"
            };

            await classService.AddClassAsync(_class);

            var result = await classService.ClassExistsByNameAsync("");

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task Test_MethodGetAllClassesAsyncShouldReturnOnlyClassesWhichAreActive()
        {
            var repo = new Repository(dbContext);

            classService = new ClassService(repo);

            var _class = new Class()
            {
                Id = new Guid("f691832a-855b-485c-a9ad-7a250c83a613"),
                Name = "Test"
            };

            var _class2 = new Class()
            {
                Id = new Guid("75957fef-59c5-4829-a12f-2025ec956a0e"),
                Name = "Test"
            };

            await classService.AddClassAsync(_class);

            await classService.AddClassAsync(_class2);

            await classService.DeleteClassByIdAsync(_class.Id.ToString());

            var result = await classService.GetAllClassesAsync();

            Assert.That(result.Count(), Is.EqualTo(1));

        }

        [Test]
        public async Task Test_MethodGetAllClassNamesAsyncShouldReturnOnlyNamesOfActiveClasses()
        {
            var repo = new Repository(dbContext);

            classService = new ClassService(repo);

            var _class = new Class()
            {
                Id = new Guid("7e8c7114-1173-4e72-847c-23fb425ce26f"),
                Name = "Test"
            };

            var _class2 = new Class()
            {
                Id = new Guid("7990cb0d-fd2a-4627-ac35-8136f1385ae4"),
                Name = "Test2"
            };

            await classService.AddClassAsync(_class);

            await classService.AddClassAsync(_class2);

            await classService.DeleteClassByIdAsync(_class.Id.ToString());

            var result = await classService.GetAllClassNamesAsync();

            Assert.That(result.First(), Is.EqualTo("Test2"));
        }

        [Test]
        public async Task Test_MethodGetClassByIdAsyncShouldReturnNullWhenClassCannotBeFound()
        {
            var repo = new Repository(dbContext);

            classService = new ClassService(repo);

            var _class = new Class()
            {
                Id = new Guid("fea5848e-7b72-44d9-92b6-516ca0cb83c3"),
                Name = "Test"
            };

            var _class2 = new Class()
            {
                Id = new Guid("6c630d1a-7a0f-431e-81a7-4a7c4e71f9eb"),
                Name = "Test2"
            };

            await classService.AddClassAsync(_class);

            await classService.AddClassAsync(_class2);

            await classService.DeleteClassByIdAsync(_class.Id.ToString());

            var result = await classService.GetClassByIdAsync(_class.Id.ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task Test_MethodGetClassByIdAsyncShouldReturnClassWhenCanFindClassWithSuchId()
        {
            var repo = new Repository(dbContext);

            classService = new ClassService(repo);

            var _class = new Class()
            {
                Id = new Guid("f4050641-d83b-42d8-b442-92508b2a256c"),
                Name = "Test"
            };

            var _class2 = new Class()
            {
                Id = new Guid("fb9bbd9e-41d1-4e71-b3c5-f59ba5cf4b04"),
                Name = "Test2"
            };

            await classService.AddClassAsync(_class);

            await classService.AddClassAsync(_class2);

            var result = await classService.GetClassByIdAsync(_class.Id.ToString());

            Assert.That(_class, Is.SameAs(result));
        }

        [Test]
        public async Task Test_MethodClassExistsByIdAsyncShouldReturnTrueIfClassExists()
        {
			var repo = new Repository(dbContext);

			classService = new ClassService(repo);

			var _class = new Class()
			{
				Id = new Guid("f4050641-d83b-42d8-b442-92508b2a256c"),
				Name = "Test"
			};

            await classService.AddClassAsync(_class);

            var actualResult = await classService.ClassExistsByIdAsync(_class.Id.ToString());

            Assert.That(actualResult, Is.True);
		}

		[Test]
		public async Task Test_MethodClassExistsByIdAsyncShouldReturnFalseIfClassDoesNotExists()
		{
			var repo = new Repository(dbContext);

			classService = new ClassService(repo);

			var _class = new Class()
			{
				Id = new Guid("f4050641-d83b-42d8-b442-92508b2a256c"),
				Name = "Test"
			};

			await classService.AddClassAsync(_class);

            await classService.DeleteClassByIdAsync(_class.Id.ToString());

            var actualResult = await classService.ClassExistsByIdAsync(_class.Id.ToString());

			Assert.That(actualResult, Is.False);
		}

		[TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
