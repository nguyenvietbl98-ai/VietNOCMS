using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietNOCMS.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        // Thuộc về cuộc hội thoại nào
        public int ConversationId { get; set; }
        [ForeignKey("ConversationId")]
        public virtual Conversation Conversation { get; set; }

        // Ai là người gửi
        public int SenderId { get; set; }
        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        [Required]
        public string Content { get; set; } // Nội dung tin nhắn

        public DateTime SentAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false; // Đã xem chưa
    }
}