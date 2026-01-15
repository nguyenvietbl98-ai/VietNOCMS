using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietNOCMS.Models
{
    public class CourseCollaborator
    {
        [Key]
        public int Id { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null!;

        public int CollaboratorId { get; set; }
        [ForeignKey("CollaboratorId")]
        public virtual User Collaborator { get; set; } = null!;

        // Vai trò: "Assistant" (Trợ giảng) hoặc "Substitute" (Dạy thay)
        public string Role { get; set; } = "Assistant";

        // Phân quyền
        public bool CanManageContent { get; set; } = false;
        public bool CanGrade { get; set; } = true;

        // --- CÁC TRƯỜNG MỚI ---

        // Trạng thái: 
        // "Pending_Invite" (GV mời, chờ User đồng ý)
        // "Pending_Approval" (User xin vào, chờ GV duyệt)
        // "Accepted" (Đã tham gia)
        // "Rejected" (Đã từ chối)
        public string Status { get; set; } = "Pending_Invite";

        // Lời nhắn (Ví dụ: "Em muốn xin làm trợ giảng" hoặc "Mời thầy A vào dạy giúp")
        public string? Message { get; set; }

        public DateTime InvitedAt { get; set; } = DateTime.Now;
        public DateTime? ResponseAt { get; set; } // Ngày duyệt/từ chối
    }
}