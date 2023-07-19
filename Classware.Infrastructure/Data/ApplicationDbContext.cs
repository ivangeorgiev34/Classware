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
					Id = 1,
					Name = "English language"
				},
				new Subject()
				{
					Id = 2,
					Name = "Bulgarian language"
				},
				new Subject()
				{
					Id = 3,
					Name = "Mathematics"
				},
				new Subject()
				{
					Id = 4,
					Name = "Physical education"
				},
				new Subject()
				{
					Id = 5,
					Name = "Information technologies"
				},
				new Subject()
				{
					Id = 6,
					Name = "Informatics"
				},
				new Subject()
				{
					Id = 7,
					Name = "Geography"
				},
				new Subject()
				{
					Id = 8,
					Name = "History"
				},
				new Subject()
				{
					Id = 9,
					Name = "Physics"
				},
				new Subject()
				{
					Id = 10,
					Name = "Biology"
				},
				new Subject()
				{
					Id = 11,
					Name = "Chemistry"
				});
		}
		
	}
}
