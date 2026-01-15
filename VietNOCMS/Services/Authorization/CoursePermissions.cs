namespace VietNOCMS.Services
{
    public enum CoursePermission
    {
        View,           // Xem nội dung
        ManageContent,  // Sửa bài học, chương, tài liệu (Dành cho Owner hoặc TA được cấp quyền)
        Grade,          // Chấm điểm (Dành cho Owner hoặc TA)
        ManageSettings, // Sửa giá, đổi tên, xóa khóa học (Thường chỉ Owner)
    }
}