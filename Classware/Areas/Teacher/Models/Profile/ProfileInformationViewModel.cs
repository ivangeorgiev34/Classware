namespace Classware.Areas.Teacher.Models.Profile
{
	public class ProfileInformationViewModel
	{
        public string Id { get; set; }
        public string? UploadedProfilePicture { get; set; }

        public IFormFile? ProfilePicture { get; set; }

        public string? ProfilePictureName { get; set; }

        public string? FirstName { get; set; } 

		public string? MiddleName { get; set; } 

		public string? LastName { get; set; }

        public string? Username { get; set; }

        public int? Age { get; set; }

		public string? Gender { get; set; } 

	}
}
