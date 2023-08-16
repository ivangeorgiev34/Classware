using Classware.Models.Chat;

namespace Classware.Areas.Administrator.Models.Chat
{
	public class AllMessagesViewModel
	{
        public AllMessagesViewModel()
        {
            this.Messages = new List<ChatViewModel>();
        }
        public int Page { get; set; }
        public int TotalMessages { get; set; }
        public List<ChatViewModel> Messages { get; set; }
    }
}
