using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Classware.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Student> Students { get; set; }

		public DbSet<Teacher> Teachers { get; set; }

		public DbSet<Subject> Subjects { get; set; }

		public DbSet<Class> Classes { get; set; }

		public DbSet<Compliment> Compliments { get; set; }

		public DbSet<Remark> Remarks { get; set; }

		public DbSet<Grade> Grades { get; set; }

		public DbSet<StudentSubject> StudentSubjects { get; set; }

		public DbSet<Message> Messages { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			SeedSubjects(builder);

			base.OnModelCreating(builder);
		}

		private void SeedSubjects(ModelBuilder builder)
		{
			builder.Entity<StudentSubject>()
				.HasKey(ss => new {ss.SubjectId,ss.StudentId});

			builder.Entity<Subject>()
				.HasData(new Subject()
				{
					Id = new Guid("e60f5411-9d93-458c-82c6-1e45cc1888a6"),
					Name = "English language"
				},
				new Subject()
				{
					Id = new Guid("18c2ee89-e19a-4c59-ab53-9a572e1840e2"),
					Name = "Bulgarian language"
				},
				new Subject()
				{
					Id = new Guid("9ecfb09c-eb90-4827-9e30-c3798db87f20"),
					Name = "Mathematics"
				},
				new Subject()
				{
					Id = new Guid("7d1cf7b9-a923-47e0-9d6a-c1b2dc5ce07a"),
					Name = "Physical education"
				},
				new Subject()
				{
					Id = new Guid("72e19517-3d0f-4839-a5cd-41bb74d25056"),
					Name = "Information technologies"
				},
				new Subject()
				{
					Id = new Guid("40c0ffef-7986-4cbe-8fec-2e3f4578df36"),
					Name = "Informatics"
				},
				new Subject()
				{
					Id = new Guid("4933f0ba-4123-49f6-b1b0-2507a8924121"),
					Name = "Geography"
				},
				new Subject()
				{
					Id = new Guid("19e7b987-e9b8-4701-aad8-6f813d8f2885"),
					Name = "History"
				},
				new Subject()
				{
					Id = new Guid("d340c7d9-b6c4-46d0-acfa-d66327e78d6f"),
					Name = "Physics"
				},
				new Subject()
				{
					Id = new Guid("99726581-f158-4168-baf9-dd9c1e52be9c"),
					Name = "Biology"
				},
				new Subject()
				{
					Id = new Guid("0d0e04ac-0f9c-41f3-918a-00bbe3865846"),
					Name = "Chemistry"
				});
		}
		
	}
}
