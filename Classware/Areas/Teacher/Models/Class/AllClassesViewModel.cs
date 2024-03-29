﻿namespace Classware.Areas.Teacher.Models.Class
{
	public class AllClassesViewModel
	{
        public AllClassesViewModel()
        {
            this.Classes = new List<ClassViewModel>();

		}

        public int Page { get; set; }

        public int TotalClasses { get; set; }

        public string? SortBy { get; set; }

		public string? SortingStyle { get; set; }

		public List<ClassViewModel>? Classes { get; set; }
    }
}
