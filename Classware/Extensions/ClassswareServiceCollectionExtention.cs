using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Models;

namespace Classware.Extensions
{
	public static class ClassswareServiceCollectionExtention
	{
		/// <summary>
		/// Adds all the services to the IoC of the application
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IRepository, Repository>();
			services.AddScoped<IStudentService, StudentService>();
			services.AddScoped<IClassService, ClassService>();
			services.AddScoped<ISubjectService, SubjectService>();
			services.AddScoped<ITeacherService, TeacherService>();
			services.AddScoped<IProfileService, ProfileService>();
			services.AddScoped<IGradeService, GradeService>();
			services.AddScoped<IRemarkService, RemarkService>();
			services.AddScoped<IComplimentService, ComplimentService>();
			services.AddScoped<IMessageService, MessageService>();

			return services;
		}
	}
}
