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
                Id = 1,
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
                Id = 1,
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
                Id = 1,
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
                Id = 1,
                Name = "Test"
            };

            var _class2 = new Class()
            {
                Id = 2,
                Name = "Test"
            };

            await classService.AddClassAsync(_class);

            await classService.AddClassAsync(_class2);

            await classService.DeleteClassByIdAsync(_class.Id);

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
                Id = 1,
                Name = "Test"
            };

            var _class2 = new Class()
            {
                Id = 2,
                Name = "Test2"
            };

            await classService.AddClassAsync(_class);

            await classService.AddClassAsync(_class2);

            await classService.DeleteClassByIdAsync(_class.Id);

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
                Id = 1,
                Name = "Test"
            };

            var _class2 = new Class()
            {
                Id = 2,
                Name = "Test2"
            };

            await classService.AddClassAsync(_class);

            await classService.AddClassAsync(_class2);

            await classService.DeleteClassByIdAsync(_class.Id);

            var result = await classService.GetClassByIdAsync(_class.Id);

            Assert.IsNull(result);
        }

        [Test]
        public async Task Test_MethodGetClassByIdAsyncShouldReturnClassWhenCanFindClassWithSuchId()
        {
            var repo = new Repository(dbContext);

            classService = new ClassService(repo);

            var _class = new Class()
            {
                Id = 1,
                Name = "Test"
            };

            var _class2 = new Class()
            {
                Id = 2,
                Name = "Test2"
            };

            await classService.AddClassAsync(_class);

            await classService.AddClassAsync(_class2);

            var result = await classService.GetClassByIdAsync(_class.Id);

            Assert.That(_class,Is.SameAs(result));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
