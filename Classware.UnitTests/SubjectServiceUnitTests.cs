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
                Id = new Guid("69887c5d-bf81-4b47-923d-e7f19ca0343a"),
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

            var expectedSubjectId = "e60f5411-9d93-458c-82c6-1e45cc1888a6";

            Assert.That(result.ToString(), Is.EqualTo(expectedSubjectId));
        }

        [Test]
        public void Test_MethodGetSubjectByNameAsyncShouldThrowExceptionWhenCannotFindSubject()
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

            var subjectIds = new List<string>()
            {
				"e60f5411-9d93-458c-82c6-1e45cc1888a6",
				"18c2ee89-e19a-4c59-ab53-9a572e1840e2"
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
