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
    public class StudentServiceUnitTests
    {
        private IRepository repo;
        private ApplicationDbContext dbContext;
        private IStudentService studentService;
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
        public async Task Test_MethodAddStudentAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);
            studentService = new StudentService(repo, subjectService);

            var student = new Student()
            {
                Id = 1,
                Class = new Class()
                {
                    Id = 1,
                    Name = "Test"
                },
                User = new ApplicationUser()
                {
                    Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
                },
            };

            await studentService.AddStudentAsync(student);

            Assert.That((await repo.All<Student>().FirstOrDefaultAsync(s => s.Id == student.Id))?.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_MethodAssignSubjectsAsyncShouldThrowExceptionWhenSubjectIdsAreNull()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);
            studentService = new StudentService(repo, subjectService);

            var student = new Student()
            {
                Id = 1,
                Class = new Class()
                {
                    Id = 1,
                    Name = "Test"
                },
                User = new ApplicationUser()
                {
                    Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
                },
            };

            await studentService.AddStudentAsync(student);

            Assert.ThrowsAsync<NullReferenceException>(async () => await studentService.AssignSubjectsAsync(new List<int>(), 1));
        }

        [Test]
        public async Task Test_MethodAssignSubjectsAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);
            studentService = new StudentService(repo, subjectService);

            var student = new Student()
            {
                Id = 1,
                Class = new Class()
                {
                    Id = 1,
                    Name = "Test"
                },
                User = new ApplicationUser()
                {
                    Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
                },
            };

            await studentService.AddStudentAsync(student);

            var subjectIds = new List<int>()
            {
                1,
                1,
                2
            };

            await studentService.AssignSubjectsAsync(subjectIds, student.Id);

            Assert.That(student.StudentSubjects.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Test_MethodDeleteStudentByIdAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);
            studentService = new StudentService(repo, subjectService);

            var student = new Student()
            {
                Id = 1,
                Class = new Class()
                {
                    Id = 1,
                    Name = "Test"
                },
                User = new ApplicationUser()
                {
                    Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
                },
            };

            await studentService.AddStudentAsync(student);

            await studentService.DeleteStudentByIdAsync(student.Id);

            var result = (await repo.All<Student>().FirstOrDefaultAsync(s => s.Id == student.Id))?.IsActive;

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_MethodGetAllStudentsAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);
            studentService = new StudentService(repo, subjectService);

            var student = new Student()
            {
                Id = 1,
                Class = new Class()
                {
                    Id = 1,
                    Name = "Test"
                },
                User = new ApplicationUser()
                {
                    Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
                },
            };

            var secondStudent = new Student()
            {
                Id = 2,
                Class = new Class()
                {
                    Id = 2,
                    Name = "Test2"
                },
                User = new ApplicationUser()
                {
                    Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
                }
            };

            await studentService.AddStudentAsync(student);

            await studentService.AddStudentAsync(secondStudent);

            await studentService.DeleteStudentByIdAsync(student.Id);

            var result = (await studentService.GetAllStudentsAsync()).Count();

            var expected = 1;

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task Test_MethodGetStudentByIdAsyncShouldThrowExceptionWhenCannotFindStudent()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);
            studentService = new StudentService(repo, subjectService);

            var student = new Student()
            {
                Id = 1,
                Class = new Class()
                {
                    Id = 1,
                    Name = "Test"
                },
                User = new ApplicationUser()
                {
                    Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
                },
            };

            var secondStudent = new Student()
            {
                Id = 2,
                Class = new Class()
                {
                    Id = 2,
                    Name = "Test2"
                },
                User = new ApplicationUser()
                {
                    Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
                }
            };

            await studentService.AddStudentAsync(student);

            await studentService.AddStudentAsync(secondStudent);

            Assert.ThrowsAsync<InvalidOperationException>(async () => await studentService.GetStudentByIdAsync(0));
        }

        [Test]
        public async Task Test_MethodGetStudentByIdAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);
            studentService = new StudentService(repo, subjectService);

            var student = new Student()
            {
                Id = 1,
                Class = new Class()
                {
                    Id = 1,
                    Name = "Test"
                },
                User = new ApplicationUser()
                {
                    Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
                },
            };

            var secondStudent = new Student()
            {
                Id = 2,
                Class = new Class()
                {
                    Id = 2,
                    Name = "Test2"
                },
                User = new ApplicationUser()
                {
                    Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
                }
            };

            await studentService.AddStudentAsync(student);

            await studentService.AddStudentAsync(secondStudent);

            var result = (await studentService.GetStudentByIdAsync(1)).Class.Name;

            var expectedClassName = "Test";

            Assert.That(result, Is.EqualTo(expectedClassName));
        }

        [Test]
        public async Task Test_MethodGetStudentByUserIdAsyncShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);
            studentService = new StudentService(repo, subjectService);

            var student = new Student()
            {
                Id = 1,
                Class = new Class()
                {
                    Id = 1,
                    Name = "Test"
                },
                User = new ApplicationUser()
                {
                    Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
                },
            };

            var secondStudent = new Student()
            {
                Id = 2,
                Class = new Class()
                {
                    Id = 2,
                    Name = "Test2"
                },
                User = new ApplicationUser()
                {
                    Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
                }
            };

            await studentService.AddStudentAsync(student);

            await studentService.AddStudentAsync(secondStudent);

            var studentUserId = "ad6668e8-bc9f-4b61-aaf5-4557720604e1";

            var result = (await studentService.GetStudentByUserIdAsync(studentUserId)).Class.Name;

            var expectedClassName = "Test";

            Assert.That(result, Is.EqualTo(expectedClassName));
        }

        [Test]
        public async Task Test_MethodGetStudentByUserIdAsyncShouldThrowExceptionWhenCannotFindStudent()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);
            studentService = new StudentService(repo, subjectService);

            var student = new Student()
            {
                Id = 1,
                Class = new Class()
                {
                    Id = 1,
                    Name = "Test"
                },
                User = new ApplicationUser()
                {
                    Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
                },
            };

            var secondStudent = new Student()
            {
                Id = 2,
                Class = new Class()
                {
                    Id = 2,
                    Name = "Test2"
                },
                User = new ApplicationUser()
                {
                    Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
                }
            };

            await studentService.AddStudentAsync(student);

            await studentService.AddStudentAsync(secondStudent);

            Assert.ThrowsAsync<NullReferenceException>(async () => await studentService.GetStudentByUserIdAsync(""));
        }

        [Test]
        public async Task Test_MethodGetStudentsByClassIdAndSubjectNameShouldWorkProperly()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);
            studentService = new StudentService(repo, subjectService);

            var student = new Student()
            {
                Id = 1,
                ClassId = 1,
                Class = new Class()
                {
                    Id = 1,
                    Name = "Test"
                },
                User = new ApplicationUser()
                {
                    Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
                },
            };

            var secondStudent = new Student()
            {
                Id = 2,
                Class = new Class()
                {
                    Id = 2,
                    Name = "Test2"
                },
                User = new ApplicationUser()
                {
                    Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
                }
            };

            await studentService.AddStudentAsync(student);

            await studentService.AddStudentAsync(secondStudent);

            var subjectIds = new List<int>()
            {
                1,
                2
            };

            await studentService.AssignSubjectsAsync(subjectIds, 1);

            var subjectName= "English language";
            var classId = 1;

            var result = (await studentService.GetStudentsByClassIdAndSubjectName(classId, subjectName)).Count();

            var expectedResult = 1;

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task Test_MethodStudentHasASubjectAsyncShouldReturnTrueWhenStudentHasSubject()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);
            studentService = new StudentService(repo, subjectService);

            var student = new Student()
            {
                Id = 1,
                ClassId = 1,
                Class = new Class()
                {
                    Id = 1,
                    Name = "Test"
                },
                User = new ApplicationUser()
                {
                    Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
                },
            };

            var secondStudent = new Student()
            {
                Id = 2,
                Class = new Class()
                {
                    Id = 2,
                    Name = "Test2"
                },
                User = new ApplicationUser()
                {
                    Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
                }
            };

            await studentService.AddStudentAsync(student);

            await studentService.AddStudentAsync(secondStudent);

            var subjectIds = new List<int>()
            {
                1
            };

            var studentId = student.Id;

            await studentService.AssignSubjectsAsync(subjectIds, studentId);

            var subjectName = "English language";

            var result = await studentService.StudentHasASubjectAsync(student, subjectName);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task Test_MethodStudentHasASubjectAsyncShouldReturnFalseWhenStudentDoesntHaveSubject()
        {
            repo = new Repository(dbContext);
            subjectService = new SubjectService(repo);
            studentService = new StudentService(repo, subjectService);

            var student = new Student()
            {
                Id = 1,
                ClassId = 1,
                Class = new Class()
                {
                    Id = 1,
                    Name = "Test"
                },
                User = new ApplicationUser()
                {
                    Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
                },
            };

            var secondStudent = new Student()
            {
                Id = 2,
                Class = new Class()
                {
                    Id = 2,
                    Name = "Test2"
                },
                User = new ApplicationUser()
                {
                    Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
                }
            };

            await studentService.AddStudentAsync(student);

            await studentService.AddStudentAsync(secondStudent);

            var result = await studentService.StudentHasASubjectAsync(student, "");

            Assert.That(result, Is.False);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
