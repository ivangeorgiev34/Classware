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

			await gradeService.AddGradeAsync("cd121bd1-f3a0-4042-b9fe-97355fd8a1fd", "a284b28d-e540-45db-a1ff-3ddf6a9cb986", "fa88e808-4423-403e-8ec6-4e7a6eb25a16", 6);

			Assert.That(repo.All<Grade>().First()?.Type, Is.EqualTo(6));
		}

		[Test]
		public async Task Test_MethodDeleteGradeByIdAsyncThrowsExceptionWhenGradeCannotBeFound()
		{
			repo = new Repository(dbContext);
			gradeService = new GradeService(repo);

			await gradeService.AddGradeAsync("8e27b970-bbb5-4f55-b102-9c33caf7e1b2", "27be7717-6478-4ff2-8448-4a0b7de518eb", "a582b781-46cf-43dd-9a9b-857241418fda", 6);

			Assert.ThrowsAsync<NullReferenceException>(async () => await gradeService.DeleteGradeByIdAsync("a582b781-46cf-43dd-9a9b-857241418fdd"));
		}

		[Test]
		public async Task Test_MethodDeleteGradeByIdAsyncShouldDeleteGradeProperly()
		{
			repo = new Repository(dbContext);
			gradeService = new GradeService(repo);

			await gradeService.AddGradeAsync("1f72e0c4-f53d-4942-97ed-76d7440c154a", "49520b08-6ec6-4d7d-a978-8766154c766a", "fff3f772-3714-40fc-ba95-bffd96cc8463", 6);

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

			await gradeService.AddGradeAsync("e12d12b5-2210-437c-a5e0-e7622060cdfd", "c524a0fb-cdb4-4233-b075-336bb59ed372", "b2aadb92-f6ee-4e3c-bf2d-fba0454ee83f", 6);

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

			await gradeService.AddGradeAsync("402b95ee-ed6c-4cf0-bb6f-505643020167", "4ccbd970-aecd-4fc4-8e7b-e06d4ee055f4", "0b9d3765-da85-423e-9b72-2ddad199177b", 6);

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

			await gradeService.AddGradeAsync("07dd2b1b-c80c-412b-a624-c65a602c7660", "ded1cfd3-2d86-4c0b-8bc7-dade140e3818", "04ac4c58-25f8-429e-a545-5bc5e2349e39", 6);

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

			await gradeService.AddGradeAsync("8f1f910e-1c89-42a5-bbf3-a0d6afc5461a", "4dd73806-aacc-4cde-a93e-472ba5ea5efd", "e60f5411-9d93-458c-82c6-1e45cc1888a6", 6);
			await gradeService.AddGradeAsync("493750c6-09d2-4c82-b100-debb616496af", "8d8b5992-563e-4365-9b01-17f667928bf3", "e60f5411-9d93-458c-82c6-1e45cc1888a6", 6);
			await gradeService.AddGradeAsync("c14108ab-5dd5-4c0d-bd66-6c4489f9d68b", "6cb4f8a5-8161-4856-bdd5-ff4528b7cfa6", "e60f5411-9d93-458c-82c6-1e45cc1888a6", 6);

			var gradeId = repo.All<Grade>().First().Id.ToString();

			var result = (await gradeService.GetGradesByStudentIdAndSubjectName(gradeId, "English language")).Count;

			Assert.That(result, Is.EqualTo(1));
		}

		[TearDown]
		public void TearDown()
		{
			dbContext.Dispose();
		}
	}
}
