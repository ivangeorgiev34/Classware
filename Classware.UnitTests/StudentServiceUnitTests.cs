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
				Id = new Guid("f088aa44-4591-41ad-9376-259835910394"),
				Class = new Class()
				{
					Id = new Guid("5048d92e-5d2e-45f1-96af-a4f450e909b2"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			await studentService.AddStudentAsync(student);

			Assert.That((await repo.All<Student>().FirstOrDefaultAsync(s => s.Id == student.Id))?.Id.ToString(), Is.EqualTo("f088aa44-4591-41ad-9376-259835910394"));
		}

		[Test]
		public async Task Test_MethodAssignSubjectsAsyncShouldThrowExceptionWhenSubjectIdsAreNull()
		{
			repo = new Repository(dbContext);
			subjectService = new SubjectService(repo);
			studentService = new StudentService(repo, subjectService);

			var student = new Student()
			{
				Id = new Guid("73849b4c-b002-43a5-b772-685ddadba7d0"),
				Class = new Class()
				{
					Id = new Guid("fe0231a3-5258-4da1-aed7-74cbbc45226d"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			await studentService.AddStudentAsync(student);

			Assert.ThrowsAsync<NullReferenceException>(async () => await studentService.AssignSubjectsAsync(new List<string>(), "73849b4c-b002-43a5-b772-685ddadba7d0"));
		}

		[Test]
		public async Task Test_MethodAssignSubjectsAsyncShouldWorkProperly()
		{
			repo = new Repository(dbContext);
			subjectService = new SubjectService(repo);
			studentService = new StudentService(repo, subjectService);

			var student = new Student()
			{
				Id = new Guid("341c201c-8c70-488d-af63-6c27ae81001a"),
				Class = new Class()
				{
					Id = new Guid("8c8b2659-77a0-46d1-b7cf-00044bb47900"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			await studentService.AddStudentAsync(student);

			var subjectIds = new List<string>()
			{
				"e60f5411-9d93-458c-82c6-1e45cc1888a6",
				"9ecfb09c-eb90-4827-9e30-c3798db87f20",
				"e60f5411-9d93-458c-82c6-1e45cc1888a6",
				"18c2ee89-e19a-4c59-ab53-9a572e1840e2"
			};

			await studentService.AssignSubjectsAsync(subjectIds, student.Id.ToString());

			Assert.That(student.StudentSubjects.Count, Is.EqualTo(3));
		}

		[Test]
		public async Task Test_MethodDeleteStudentByIdAsyncShouldWorkProperly()
		{
			repo = new Repository(dbContext);
			subjectService = new SubjectService(repo);
			studentService = new StudentService(repo, subjectService);

			var student = new Student()
			{
				Id = new Guid("341c201c-8c70-488d-af63-6c27ae81001a"),
				Class = new Class()
				{
					Id = new Guid("8c8b2659-77a0-46d1-b7cf-00044bb47900"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			await studentService.AddStudentAsync(student);

			await studentService.DeleteStudentByIdAsync(student.Id.ToString());

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
				Id = new Guid("341c201c-8c70-488d-af63-6c27ae81001a"),
				Class = new Class()
				{
					Id = new Guid("8c8b2659-77a0-46d1-b7cf-00044bb47900"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			var secondStudent = new Student()
			{
				Id = new Guid("4b44b517-e6be-45e8-9007-457ef107f618"),
				Class = new Class()
				{
					Id = new Guid("7f77e923-2603-426e-a4c3-0bf8410859ee"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ee09c288-e78e-4be7-85e0-c5c537d46022"
				},
			};

			await studentService.AddStudentAsync(student);

			await studentService.AddStudentAsync(secondStudent);

			await studentService.DeleteStudentByIdAsync(student.Id.ToString());

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
				Id = new Guid("a2f6f7f1-253a-46bf-b613-a4bada059a2c"),
				Class = new Class()
				{
					Id = new Guid("62777727-ed06-458d-8879-b0c41f023402"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			var secondStudent = new Student()
			{
				Id = new Guid("7553f0e2-4f07-4cbe-a624-58a1f4b1bd49"),
				Class = new Class()
				{
					Id = new Guid("5b031d15-2e8d-4e72-b95e-985852486212"),
					Name = "Test2"
				},
				User = new ApplicationUser()
				{
					Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
				}
			};

			await studentService.AddStudentAsync(student);

			await studentService.AddStudentAsync(secondStudent);

			Assert.ThrowsAsync<InvalidOperationException>(async () => await studentService.GetStudentByIdAsync("99f44d20-e8e0-44f1-8527-85576f7a8606"));
		}

		[Test]
		public async Task Test_MethodGetStudentByIdAsyncShouldWorkProperly()
		{
			repo = new Repository(dbContext);
			subjectService = new SubjectService(repo);
			studentService = new StudentService(repo, subjectService);

			var student = new Student()
			{
				Id = new Guid("a2f6f7f1-253a-46bf-b613-a4bada059a2c"),
				Class = new Class()
				{
					Id = new Guid("62777727-ed06-458d-8879-b0c41f023402"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			var secondStudent = new Student()
			{
				Id = new Guid("7553f0e2-4f07-4cbe-a624-58a1f4b1bd49"),
				Class = new Class()
				{
					Id = new Guid("5b031d15-2e8d-4e72-b95e-985852486212"),
					Name = "Test2"
				},
				User = new ApplicationUser()
				{
					Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
				}
			};

			await studentService.AddStudentAsync(student);

			await studentService.AddStudentAsync(secondStudent);

			var result = (await studentService.GetStudentByIdAsync("a2f6f7f1-253a-46bf-b613-a4bada059a2c")).Class.Name;

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
				Id = new Guid("a2f6f7f1-253a-46bf-b613-a4bada059a2c"),
				Class = new Class()
				{
					Id = new Guid("62777727-ed06-458d-8879-b0c41f023402"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			var secondStudent = new Student()
			{
				Id = new Guid("7553f0e2-4f07-4cbe-a624-58a1f4b1bd49"),
				Class = new Class()
				{
					Id = new Guid("5b031d15-2e8d-4e72-b95e-985852486212"),
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
				Id = new Guid("a2f6f7f1-253a-46bf-b613-a4bada059a2c"),
				Class = new Class()
				{
					Id = new Guid("62777727-ed06-458d-8879-b0c41f023402"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			var secondStudent = new Student()
			{
				Id = new Guid("7553f0e2-4f07-4cbe-a624-58a1f4b1bd49"),
				Class = new Class()
				{
					Id = new Guid("5b031d15-2e8d-4e72-b95e-985852486212"),
					Name = "Test2"
				},
				User = new ApplicationUser()
				{
					Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
				}
			};

			await studentService.AddStudentAsync(student);

			await studentService.AddStudentAsync(secondStudent);

			Assert.ThrowsAsync<NullReferenceException>(async () => await studentService.GetStudentByUserIdAsync("5b031d15-2e8d-4e72-b95e-985852486212"));
		}

		[Test]
		public async Task Test_MethodGetStudentsByClassIdAndSubjectNameShouldWorkProperly()
		{
			repo = new Repository(dbContext);
			subjectService = new SubjectService(repo);
			studentService = new StudentService(repo, subjectService);

			var student = new Student()
			{
				Id = new Guid("1d3a5cc0-458d-4e48-9a97-aa268d0dea4f"),
				ClassId = new Guid("54f15b35-0649-435c-846e-3a87825b543d"),
				Class = new Class()
				{
					Id = new Guid("54f15b35-0649-435c-846e-3a87825b543d"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			var secondStudent = new Student()
			{
				Id = new Guid("8238a0d9-f0ee-458b-9875-93ff338e0ccd"),
				Class = new Class()
				{
					Id = new Guid("42a63f23-5642-41d4-9661-29cf73ce8621"),
					Name = "Test2"
				},
				User = new ApplicationUser()
				{
					Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
				}
			};

			await studentService.AddStudentAsync(student);

			await studentService.AddStudentAsync(secondStudent);

			var subjectIds = new List<string>()
			{
				"e60f5411-9d93-458c-82c6-1e45cc1888a6",
				"18c2ee89-e19a-4c59-ab53-9a572e1840e2"
			};

			await studentService.AssignSubjectsAsync(subjectIds, "1d3a5cc0-458d-4e48-9a97-aa268d0dea4f");

			var subjectName = "English language";
			var classId = "54f15b35-0649-435c-846e-3a87825b543d";

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
				Id = new Guid("1d3a5cc0-458d-4e48-9a97-aa268d0dea4f"),
				ClassId = new Guid("54f15b35-0649-435c-846e-3a87825b543d"),
				Class = new Class()
				{
					Id = new Guid("54f15b35-0649-435c-846e-3a87825b543d"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			var secondStudent = new Student()
			{
				Id = new Guid("8238a0d9-f0ee-458b-9875-93ff338e0ccd"),
				Class = new Class()
				{
					Id = new Guid("42a63f23-5642-41d4-9661-29cf73ce8621"),
					Name = "Test2"
				},
				User = new ApplicationUser()
				{
					Id = "2f53b253-d3fe-4888-a87b-bb2a0975ab38"
				}
			};

			await studentService.AddStudentAsync(student);

			await studentService.AddStudentAsync(secondStudent);

			var subjectIds = new List<string>()
			{
				"e60f5411-9d93-458c-82c6-1e45cc1888a6"
			};

			var studentId = student.Id;

			await studentService.AssignSubjectsAsync(subjectIds, student.Id.ToString());

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
				Id = new Guid("1d3a5cc0-458d-4e48-9a97-aa268d0dea4f"),
				ClassId = new Guid("54f15b35-0649-435c-846e-3a87825b543d"),
				Class = new Class()
				{
					Id = new Guid("54f15b35-0649-435c-846e-3a87825b543d"),
					Name = "Test"
				},
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			var secondStudent = new Student()
			{
				Id = new Guid("8238a0d9-f0ee-458b-9875-93ff338e0ccd"),
				Class = new Class()
				{
					Id = new Guid("42a63f23-5642-41d4-9661-29cf73ce8621"),
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

		[Test]
		public async Task Test_MethodStudentExistsByUserIdShouldReturnTrueWhenStudentIsFound()
		{
			repo = new Repository(dbContext);
			subjectService = new SubjectService(repo);
			studentService = new StudentService(repo, subjectService);

			var student = new Student()
			{
				Id = new Guid("1d3a5cc0-458d-4e48-9a97-aa268d0dea4f"),
				ClassId = new Guid("54f15b35-0649-435c-846e-3a87825b543d"),
				Class = new Class()
				{
					Id = new Guid("54f15b35-0649-435c-846e-3a87825b543d"),
					Name = "Test"
				},
				UserId = "ad6668e8-bc9f-4b61-aaf5-4557720604e1",
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			await studentService.AddStudentAsync(student);

			var expectedResult = await studentService.StudentExistsByUserId(student.Id.ToString());

			Assert.That(expectedResult, Is.True);
		}

		[Test]
		public async Task Test_MethodStudentExistsByUserIdShouldReturnFalseWhenStudentIsNotFound()
		{
			repo = new Repository(dbContext);
			subjectService = new SubjectService(repo);
			studentService = new StudentService(repo, subjectService);

			var student = new Student()
			{
				Id = new Guid("1d3a5cc0-458d-4e48-9a97-aa268d0dea4f"),
				ClassId = new Guid("54f15b35-0649-435c-846e-3a87825b543d"),
				Class = new Class()
				{
					Id = new Guid("54f15b35-0649-435c-846e-3a87825b543d"),
					Name = "Test"
				},
				UserId = "ad6668e8-bc9f-4b61-aaf5-4557720604e1",
				User = new ApplicationUser()
				{
					Id = "ad6668e8-bc9f-4b61-aaf5-4557720604e1"
				},
			};

			await studentService.AddStudentAsync(student);

			var expectedResult = await studentService.StudentExistsByUserId("54f15b35-0649-435c-846e-3a87825b543d");

			Assert.That(expectedResult, Is.False);
		}

		[TearDown]
		public void TearDown()
		{
			dbContext.Dispose();
		}
	}
}
