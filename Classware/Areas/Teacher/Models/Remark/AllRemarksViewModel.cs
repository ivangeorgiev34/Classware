﻿namespace Classware.Areas.Teacher.Models.Remark
{
	public class AllRemarksViewModel
	{
        public string? FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string? LastName { get; set; }

		public string? ClassName { get; set; }

		public string StudentId { get; set; } = null!;

		public string TeacherId { get; set; } = null!;

		public int TotalRemarks { get; set; }

        public int Page { get; set; }

        public ICollection<RemarkViewModel>? Remarks { get; set; }
    }
}
