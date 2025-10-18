using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime SentAt { get; set; }
        public string MessageContent { get; set; }
        public string MessageType { get; set; } = "Text";

        public ApplicationUser User { get; set; }
    }
}
