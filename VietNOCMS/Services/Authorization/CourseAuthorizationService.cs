using Microsoft.EntityFrameworkCore;
using VietNOCMS.Data;


namespace VietNOCMS.Services
{
    public interface ICourseAuthorizationService
    {
       
        Task<bool> AuthorizeAsync(int userId, int courseId, CoursePermission permission);

      
        Task<bool> AuthorizeLessonAsync(int userId, int lessonId, CoursePermission permission);
    }

    public class CourseAuthorizationService : ICourseAuthorizationService
    {
        private readonly ApplicationDbContext _context;

        public CourseAuthorizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AuthorizeAsync(int userId, int courseId, CoursePermission permission)
        {
         
            var course = await _context.Courses
                .Select(c => new { c.CourseId, c.InstructorId })
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null) return false;

          
            if (course.InstructorId == userId) return true;

           
            var collaborator = await _context.CourseCollaborators
                .Where(cc => cc.CourseId == courseId && cc.CollaboratorId == userId && cc.Status == "Accepted")
                .Select(cc => new { cc.CanManageContent, cc.CanGrade })
                .FirstOrDefaultAsync();

            if (collaborator == null) return false; 

           
            switch (permission)
            {
                case CoursePermission.ManageContent:
                    return collaborator.CanManageContent; 

                case CoursePermission.Grade:
                    return collaborator.CanGrade;       

                case CoursePermission.View:
                    return true;                      

                case CoursePermission.ManageSettings:
                    return false;                        

                default:
                    return false;
            }
        }

        public async Task<bool> AuthorizeLessonAsync(int userId, int lessonId, CoursePermission permission)
        {
         
            var courseId = await _context.Lessons
                .Include(l => l.Chapter)
                .Where(l => l.LessonId == lessonId)
                .Select(l => l.Chapter.CourseId)
                .FirstOrDefaultAsync();

            if (courseId == 0) return false;

            return await AuthorizeAsync(userId, courseId, permission);
        }
    }
}