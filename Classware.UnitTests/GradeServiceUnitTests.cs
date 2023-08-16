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

			await gradeService.AddGradeAsync("cd121bd1-f3a0-4042-b9fe-97355fd8a1fd", "a284b28d-e540-45db-a1ff-3ddf6a9cb986", "e60f5411-9d93-458c-82c6-1e45cc1888a6", 6);

			Assert.That(repo.All<Grade>().First()?.Type, Is.EqualTo(6));
		}

		[Test]
		public async Task Test_MethodDeleteGradeByIdAsyncThrowsExceptionWhenGradeCannotBeFound()
		{
			repo = new Repository(dbContext);
			gradeService = new GradeService(repo);

			await gradeService.AddGradeAsync("8e27b970-bbb5-4f55-b102-9c33caf7e1b2", "27be7717-6478-4ff2-8448-4a0b7de518eb", "e60f5411-9d93-458c-82c6-1e45cc1888a6", 6);

			Assert.ThrowsAsync<NullReferenceException>(async () => await gradeService.DeleteGradeByIdAsync("a582b781-46cf-43dd-9a9b-857241418fdd"));
		}

		[Test]
		public async Task Test_MethodDeleteGradeByIdAsyncShouldDeleteGradeProperly()
		{
			repo = new Repository(dbContext);
			gradeService = new GradeService(repo);

			await gradeService.AddGradeAsync("1f72e0c4-f53d-4942-97ed-76d7440c154a", "49520b08-6ec6-4d7d-a978-8766154c766a", "e60f5411-9d93-458c-82c6-1e45cc1888a6", 6);

			var gradeId = repo.All<Grade>().First().Id.ToString();

			await gradeService.DeleteGradeByIdAsync(gradeId);

			Assert.That((await repo.All<Grade>().FirstOrDefaultAsync(g => g.Id == new Guid(gradeId)))?.IsActive, Is.EqualTo(false));
		}

		[Test]
		public async Task Test_MethodEditGradeByIdAsyncShouldEditGradeProperly()
		{
			repo = new Repository(dbContext);
			gradeService = new GradeService(repo);

			var student = new Student()
			{
				Id = new Guid("010afbc9-e925-4291-ac6a-585524344580"),
				User = new ApplicationUser()
				{
					Id = "f2eb23b1-adad-4225-b50f-e5c4cc309146"
				},
				Class = new Class()
				{
					Id = new Guid("6dd0308e-adb6-4746-9ccf-8ced26935713"),
					Name = ""
				}
			};

			var teacher = new Teacher()
			{
				Id = new Guid("48db782b-1c72-41fe-9895-08334732a8a2"),
				User = new ApplicationUser()
				{
					Id = "fa57882a-1a8d-495c-9b48-07943b983dac"
				}
			};
			await repo.AddAsync(student);
			await repo.AddAsync(teacher);

			await gradeService.AddGradeAsync(student.Id.ToString(), teacher.Id.ToString(), "e60f5411-9d93-458c-82c6-1e45cc1888a6", 6);

			var gradeId = repo.All<Grade>().First().Id.ToString();

			await gradeService.EditGradeByIdAsync(gradeId, 5);

			Assert.That((await repo.All<Grade>().FirstOrDefaultAsync(g => g.Id == new Guid(gradeId)))?.Type, Is.EqualTo(5));
		}

		[Test]
		public async Task Test_MethodGetGradeByIdAsyncShouldТhrowExceptionWhenCannotFindGrade()
		{
			repo = new Repository(dbContext);
			gradeService = new GradeService(repo);

			var student = new Student()
			{
				Id = new Guid("010afbc9-e925-4291-ac6a-585524344580"),
				User = new ApplicationUser()
				{
					Id = "f2eb23b1-adad-4225-b50f-e5c4cc309146"
				},
				Class = new Class()
				{
					Id = new Guid("6dd0308e-adb6-4746-9ccf-8ced26935713"),
					Name = ""
				}
			};

			var teacher = new Teacher()
			{
				Id = new Guid("48db782b-1c72-41fe-9895-08334732a8a2"),
				User = new ApplicationUser()
				{
					Id = "fa57882a-1a8d-495c-9b48-07943b983dac"
				}
			};
			await repo.AddAsync(student);
			await repo.AddAsync(teacher);

			await gradeService.AddGradeAsync(student.Id.ToString(), teacher.Id.ToString(), "e60f5411-9d93-458c-82c6-1e45cc1888a6", 6);

			Assert.ThrowsAsync<NullReferenceException>(async () => await gradeService.GetGradeByIdAsync("fa5369d5-bd37-46a4-9593-41821af72453"));
		}

		[Test]
		public async Task Test_MethodGetGradeByIdAsyncShouldFindGradeProperly()
		{
			repo = new Repository(dbContext);
			gradeService = new GradeService(repo);

			var student = new Student()
			{
				Id = new Guid("010afbc9-e925-4291-ac6a-585524344580"),
				User = new ApplicationUser()
				{
					Id = "f2eb23b1-adad-4225-b50f-e5c4cc309146"
				},
				Class = new Class()
				{
					Id = new Guid("6dd0308e-adb6-4746-9ccf-8ced26935713"),
					Name = ""
				}
			};

			var teacher = new Teacher()
			{
				Id = new Guid("48db782b-1c72-41fe-9895-08334732a8a2"),
				User = new ApplicationUser()
				{
					Id = "fa57882a-1a8d-495c-9b48-07943b983dac"
				}
			};
			await repo.AddAsync(student);
			await repo.AddAsync(teacher);

			await gradeService.AddGradeAsync(student.Id.ToString(), teacher.Id.ToString(), "e60f5411-9d93-458c-82c6-1e45cc1888a6", 6);

			var gradeId = repo.All<Grade>().First().Id.ToString();

			Assert.That((await repo.All<Grade>().FirstOrDefaultAsync(g => g.Id == new Guid(gradeId)))?.Type, Is.EqualTo(6));
		}

		[Test]
		public async Task Test_MethodGetGradesByStudentIdAndSubjectNameShouldFindGradeProperly()
		{
			repo = new Repository(dbContext);
			gradeService = new GradeService(repo);

			var student = new Student()
			{
				Id = new Guid("010afbc9-e925-4291-ac6a-585524344580"),
				User = new ApplicationUser()
				{
					Id = "f2eb23b1-adad-4225-b50f-e5c4cc309146"
				},
				Class = new Class()
				{
					Id = new Guid("6dd0308e-adb6-4746-9ccf-8ced26935713"),
					Name = ""
				}
			};

			var teacher = new Teacher()
			{
				Id = new Guid("48db782b-1c72-41fe-9895-08334732a8a2"),
				User = new ApplicationUser()
				{
					Id = "fa57882a-1a8d-495c-9b48-07943b983dac"
				}
			};
			await repo.AddAsync(student);
			await repo.AddAsync(teacher);

			await gradeService.AddGradeAsync(student.Id.ToString(), teacher.Id.ToString(), "e60f5411-9d93-458c-82c6-1e45cc1888a6", 6);
			await gradeService.AddGradeAsync(student.Id.ToString(), teacher.Id.ToString(), "e60f5411-9d93-458c-82c6-1e45cc1888a6", 6);
			await gradeService.AddGradeAsync(student.Id.ToString(), teacher.Id.ToString(), "e60f5411-9d93-458c-82c6-1e45cc1888a6", 6);

			var gradeId = repo.All<Grade>().First().Id.ToString();

			var result = (await gradeService.GetGradesByStudentIdAndSubjectName(student.Id.ToString(), "English language")).Count;

			Assert.That(result, Is.EqualTo(3));
		}

		[TearDown]
		public void TearDown()
		{
			dbContext.Dispose();
		}
	}
}
