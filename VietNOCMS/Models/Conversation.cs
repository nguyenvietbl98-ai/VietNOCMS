using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietNOCMS.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        // --- BỔ SUNG 2 DÒNG NÀY ---
        public string Type { get; set; } = "Direct"; // Loại: "Direct" (Chat riêng), "Support" (Hỗ trợ), "Recruitment"...
        public int? CourseId { get; set; } // Nếu chat về khóa học thì lưu ID, chat riêng thì null
        // --------------------------

        public int User1Id { get; set; }
        [ForeignKey("User1Id")]
        public virtual User User1 { get; set; }

        public int User2Id { get; set; }
        [ForeignKey("User2Id")]
        public virtual User User2 { get; set; }

        public DateTime LastMessageAt { get; set; } = DateTime.Now;

        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}