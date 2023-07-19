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

		public static class Message
		{
			public const string MESSAGE_TITLE_REQUIRED_ERROR_MESSAGE = "Title is required!";
			public const string MESSAGE_FULL_NAME_REQUIRED_ERROR_MESSAGE = "Full name is required!";
			public const string MESSAGE_DESCRIPTION_REQUIRED_ERROR_MESSAGE = "Description is required!";
			public const string MESSAGE_EMAIL_REQUIRED_ERROR_MESSAGE = "Email is required!";
			public const string MESSAGE_EMAIL_INVALID_ERROR_MESSAGE = "Invalid email!";
			public const string MESSAGE_IS_ANSWERED_REQUIRED_ERROR_MESSAGE = "Is answered is required!";
		}
	}
}
