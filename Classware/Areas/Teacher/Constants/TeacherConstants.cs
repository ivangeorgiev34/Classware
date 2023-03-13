using System.Security.Policy;

namespace Classware.Areas.Teacher.Constants
{
	public static class TeacherConstants
	{
		public static class AddGradeViewModel
		{
			public const int GRADE_TYPE_MIN_RANGE = 2;
			public const int GRADE_TYPE_MAX_RANGE = 6;
		}

		public static class AddRemarkViewModel
		{
			public const int DESCRIPTION_MIN_LENGTH = 0;
			public const int DESCRIPTION_MAX_LENGTH = 100;

			public const int TITLE_MIN_LENGTH = 10;
			public const int TITLE_MAX_LENGTH = 30;
		}

		public static class AddComplimentViewModel
		{
			public const int DESCRIPTION_MIN_LENGTH = 0;
			public const int DESCRIPTION_MAX_LENGTH = 100;

			public const int TITLE_MIN_LENGTH = 10;
			public const int TITLE_MAX_LENGTH = 30;
		}
	}
}
