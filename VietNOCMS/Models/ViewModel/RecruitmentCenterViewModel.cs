using System.Collections.Generic;

namespace VietNOCMS.Models
{
    public class RecruitmentCenterViewModel
    {
        // Tab 1: Sàn tuyển dụng (Tất cả tin đang đăng của mọi người)
        public List<Course> MarketPosts { get; set; } = new List<Course>();

        // Tab 2: Quản lý tin của tôi (Danh sách khóa học của mình để Bật/Tắt tuyển dụng)
        public List<Course> MyCourses { get; set; } = new List<Course>();

        // Tab 3: Duyệt đơn (Danh sách người xin vào khóa học của mình)
        public List<CourseCollaborator> PendingApplications { get; set; } = new List<CourseCollaborator>();
        public Dictionary<int, string> MyStatus { get; set; } = new Dictionary<int, string>();
    }
}