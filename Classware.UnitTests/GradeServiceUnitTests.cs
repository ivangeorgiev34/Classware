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
    public class GradeServiceUnitTests
    {
        private IRepository repo;
        private ApplicationDbContext dbContext;
        private IGradeService gradeService;

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
        public async Task Test_MethodAddGradeAsync()
        {
            repo = new Repository(dbContext);
            gradeService = new GradeService(repo);

            await gradeService.AddGradeAsync(1, 1, 1, 6);

            Assert.That((await repo.All<Grade>().FirstOrDefaultAsync(g => g.Id == 1))?.Type, Is.EqualTo(6));
        }

        [Test]
        public async Task Test_MethodDeleteGradeByIdAsyncThrowsExceptionWhenGradeCannotBeFound()
        {
            repo = new Repository(dbContext);
            gradeService = new GradeService(repo);

            await gradeService.AddGradeAsync(1, 1, 1, 6);

            Assert.ThrowsAsync<NullReferenceException>(async () => await gradeService.DeleteGradeByIdAsync(0));
        }

        [Test]
        public async Task Test_MethodDeleteGradeByIdAsyncShouldDeleteGradeProperly()
        {
            repo = new Repository(dbContext);
            gradeService = new GradeService(repo);

            await gradeService.AddGradeAsync(1, 1, 1, 6);

            await gradeService.DeleteGradeByIdAsync(1);

            Assert.That((await repo.All<Grade>().FirstOrDefaultAsync(g=>g.Id == 1))?.IsActive,Is.EqualTo(false));
        }

        [Test]
        public async Task Test_MethodEditGradeByIdAsyncShouldEditGradeProperly()
        {
            repo = new Repository(dbContext);
            gradeService = new GradeService(repo);

            var student = new Student()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "f2eb23b1-adad-4225-b50f-e5c4cc309146"
                },
                Class = new Class()
                {
                    Id = 1,
                    Name=""
                }
            };

            var teacher = new Teacher()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "fa57882a-1a8d-495c-9b48-07943b983dac"
                }
            };
            await repo.AddAsync(student);
            await repo.AddAsync(teacher);

            await gradeService.AddGradeAsync(1, 1, 1, 6);

            await gradeService.EditGradeByIdAsync(1,5);

            Assert.That((await repo.All<Grade>().FirstOrDefaultAsync(g => g.Id == 1))?.Type, Is.EqualTo(5));
        }

        [Test]
        public async Task Test_MethodGetGradeByIdAsyncShouldТhrowExceptionWhenCannotFindGrade()
        {
            repo = new Repository(dbContext);
            gradeService = new GradeService(repo);

            var student = new Student()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "f2eb23b1-adad-4225-b50f-e5c4cc309146"
                },
                Class = new Class()
                {
                    Id = 1,
                    Name = ""
                }
            };

            var teacher = new Teacher()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "fa57882a-1a8d-495c-9b48-07943b983dac"
                }
            };
            await repo.AddAsync(student);
            await repo.AddAsync(teacher);

            await gradeService.AddGradeAsync(1, 1, 1, 6);

            Assert.ThrowsAsync<NullReferenceException>(async () => await gradeService.GetGradeByIdAsync(0));
        }

        [Test]
        public async Task Test_MethodGetGradeByIdAsyncShouldFindGradeProperly()
        {
            repo = new Repository(dbContext);
            gradeService = new GradeService(repo);

            var student = new Student()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "f2eb23b1-adad-4225-b50f-e5c4cc309146"
                },
                Class = new Class()
                {
                    Id = 1,
                    Name = ""
                }
            };

            var teacher = new Teacher()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "fa57882a-1a8d-495c-9b48-07943b983dac"
                }
            };
            await repo.AddAsync(student);
            await repo.AddAsync(teacher);

            await gradeService.AddGradeAsync(1, 1, 1, 6);

            Assert.That((await repo.All<Grade>().FirstOrDefaultAsync(g => g.Id == 1))?.Type, Is.EqualTo(6));
        }

        [Test]
        public async Task Test_MethodGetGradesByStudentIdAndSubjectNameShouldFindGradeProperly()
        {
            repo = new Repository(dbContext);
            gradeService = new GradeService(repo);

            var student = new Student()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "f2eb23b1-adad-4225-b50f-e5c4cc309146"
                },
                Class = new Class()
                {
                    Id = 1,
                    Name = ""
                }
            };

            var teacher = new Teacher()
            {
                Id = 1,
                User = new ApplicationUser()
                {
                    Id = "fa57882a-1a8d-495c-9b48-07943b983dac"
                }
            };
            await repo.AddAsync(student);
            await repo.AddAsync(teacher);

            await gradeService.AddGradeAsync(1, 1, 1, 6);
            await gradeService.AddGradeAsync(2, 1, 1, 6);
            await gradeService.AddGradeAsync(2, 1, 2, 6);

            var result = (await gradeService.GetGradesByStudentIdAndSubjectName(1, "English language")).Count;

            Assert.That(result, Is.EqualTo(1));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
