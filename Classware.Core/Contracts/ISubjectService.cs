﻿using Classware.Infrastructure.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Core.Contracts
{
	public interface ISubjectService
	{
		Task AddSubjectAsync(Subject subject);

        Task<bool> SubjectExistsByNameAsync(string name);

		Task<IEnumerable<Subject>> GetAllSubjectsAsync();

		Task<ICollection<Subject>> GetAllSubjectsByIdsAsync(ICollection<string> subjectIds);

		Task<Subject> GetSubjectByNameAsync(string subjectName);
	} 
}
