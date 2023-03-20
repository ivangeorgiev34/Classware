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
    public class SubjectServiceUnitTests
    {
        private IRepository repo;
        private ApplicationDbContext dbContext;
        private ISubjectService subjectService;

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
        public async Task Test_MethodAddSubjectAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);

            var subject = new Subject()
            {
                Id = 12,
                Name = "random"
            };

            await subjectService.AddSubjectAsync(subject);

            var result = (await subjectService.GetAllSubjectsAsync()).Count();

            var expectedSubjectCount = 12;

            Assert.That(result, Is.EqualTo(expectedSubjectCount));
        }

        [Test]
        public async Task Test_MethodGetAllSubjectsAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);

            var result = (await subjectService.GetAllSubjectsAsync()).Count();

            var expectedSubjectCount = 11;

            Assert.That(result, Is.EqualTo(expectedSubjectCount));
        }

        [Test]
        public async Task Test_MethodGetSubjectByNameAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);

            var subjectName = "English language";

            var result = (await subjectService.GetSubjectByNameAsync(subjectName)).Id;

            var expectedSubjectId = 1;

            Assert.That(result, Is.EqualTo(expectedSubjectId));
        }

        [Test]
        public async Task Test_MethodGetSubjectByNameAsyncShouldThrowExceptionWhenCannotFindSubject()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);

            Assert.ThrowsAsync<NullReferenceException>(async () => await subjectService.GetSubjectByNameAsync(string.Empty));
        }

        [Test]
        public async Task Test_MethodGetAllSubjectsByIdsAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);

            var subjectIds = new List<int>()
            {
                1,
                2
            };

            var result = (await subjectService.GetAllSubjectsByIdsAsync(subjectIds)).Count();

            var expectedSubjectCount = 2;

            Assert.That(result, Is.EqualTo(expectedSubjectCount));
        }

        [Test]
        public async Task Test_MethodSubjectExistsByNameAsyncShouldReturnTrueWhenSubjectExists()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);

            var subjectName = "English language";

            var result = await subjectService.SubjectExistsByNameAsync(subjectName);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task Test_MethodSubjectExistsByNameAsyncShouldReturnFalseWhenSubjectDoesntExists()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);

            var result = await subjectService.SubjectExistsByNameAsync(string.Empty);

            Assert.That(result, Is.False);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
