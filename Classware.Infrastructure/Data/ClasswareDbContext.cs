using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Infrastructure.Data
{
	public class ClasswareDbContext : IdentityDbContext<ApplicationUser>
	{
		public ClasswareDbContext(DbContextOptions options) : base(options)
		{
		}
		public ClasswareDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}

		public DbSet<Student> Students { get; set; }

		public DbSet<Teacher> Teachers { get; set; }

		public DbSet<Subject> Subjects { get; set; }

		public DbSet<Class> Classes { get; set; }

		public DbSet<Compliment> Compliments { get; set; }

		public DbSet<Remark> Remarks { get; set; }

		public DbSet<Grade> Grades { get; set; }


	}
}
