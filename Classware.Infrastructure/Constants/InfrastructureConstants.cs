using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Infrastructure.Constants
{
	public static class InfrastructureConstants
	{
		public static class Class
		{
			public const int CLASS_NAME_MIN_LENGTH = 2;
			public const int CLASS_NAME_MAX_LENGTH = 3;
		}

		public static class Subject
		{
			public const int SUBJECT_NAME_MAX_LENGTH = 40;
		}

		public static class Grade
		{
			public const int GRADE_TYPE_MIN_VALUE = 2;
			public const int GRADE_TYPE_MAX_VALUE = 6;
		}

		public static class Remark
		{
			public const int REMARK_TITLE_MIN_LENGTH = 10;
			public const int REMARK_TITLE_MAX_LENGTH = 30;

			public const int REMARK_DESCRIPTION_MAX_LENGTH = 100;
		}

		public static class Compliment
		{
			public const int COMPLIMENT_TITLE_MIN_LENGTH = 10;
			public const int COMPLIMENT_TITLE_MAX_LENGTH = 30;

			public const int COMPLIMENT_DESCRIPTION_MAX_LENGTH = 100;
		}
	}
}
