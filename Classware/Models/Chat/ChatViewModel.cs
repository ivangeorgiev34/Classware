namespace Classware.Models.Chat
{
	public class ChatViewModel
	{
		public int Id { get; set; }

		public string FullName { get; set; } = null!;

		public string Email { get; set; } = null!;

		public string Title { get; set; } = null!;

		public string Description { get; set; } = null!;
	}
}
