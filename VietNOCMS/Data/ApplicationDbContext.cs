using Microsoft.EntityFrameworkCore;
using VietNOCMS.Models;

namespace VietNOCMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

     
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<LessonProgress> LessonProgresses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<InstructorRequest> InstructorRequests { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<CourseCollaborator> CourseCollaborators { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

          
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId); 
                entity.HasIndex(e => e.Email).IsUnique(); 
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.Role).HasDefaultValue("Student");
            });

      
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

        
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId);

                entity.HasOne(c => c.Instructor)
                    .WithMany()
                    .HasForeignKey(c => c.InstructorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Category)
                    .WithMany(cat => cat.Courses)
                    .HasForeignKey(c => c.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

       
            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.HasKey(e => e.ChapterId);

                entity.HasOne(ch => ch.Course)
                    .WithMany(c => c.Chapters)
                    .HasForeignKey(ch => ch.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        
            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.Property(e => e.LessonId).ValueGeneratedOnAdd();
                entity.HasKey(e => e.LessonId);

                entity.HasOne(l => l.Chapter)
                    .WithMany(ch => ch.Lessons)
                    .HasForeignKey(l => l.ChapterId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.EnrollmentId);

                entity.HasOne(e => e.Student)
                    .WithMany()
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);

             
                entity.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
            });

          
            modelBuilder.Entity<LessonProgress>(entity =>
            {
                entity.HasKey(e => e.ProgressId);

                entity.HasOne(lp => lp.Enrollment)
                    .WithMany(e => e.LessonProgresses)
                    .HasForeignKey(lp => lp.EnrollmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(lp => lp.Lesson)
                    .WithMany(l => l.LessonProgresses)
                    .HasForeignKey(lp => lp.LessonId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

         
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.ReviewId);

                entity.HasOne(r => r.Student)
                    .WithMany()
                    .HasForeignKey(r => r.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Course)
                    .WithMany(c => c.Reviews)
                    .HasForeignKey(r => r.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        
            modelBuilder.Entity<InstructorRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.HasOne(ir => ir.User)
                    .WithMany()
                    .HasForeignKey(ir => ir.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ir => ir.ReviewedByAdmin)
                    .WithMany()
                    .HasForeignKey(ir => ir.ReviewedByAdminId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

         
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId);

                entity.HasOne(n => n.User)
                    .WithMany()
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            

          
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FullName = "Administrator",
                    Email = "admin@vietnocms.com",
                    PhoneNumber = "0123456789",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    Role = "Admin"
                },
                new User
                {
                    UserId = 2,
                    FullName = "Nguyễn Văn A",
                    Email = "instructor@vietnocms.com",
                    PhoneNumber = "0987654321",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Instructor@123"),
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    Role = "Instructor"
                },
                new User
                {
                    UserId = 3,
                    FullName = "Trần Thị B",
                    Email = "student@vietnocms.com",
                    PhoneNumber = "0369852147",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    Role = "Student"
                }
            );

         
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Lập trình Web", Description = "Các khóa học về phát triển web", Icon = "bi-code-slash", IsActive = true },
                new Category { CategoryId = 2, CategoryName = "Thiết kế UI/UX", Description = "Các khóa học về thiết kế giao diện", Icon = "bi-palette", IsActive = true },
                new Category { CategoryId = 3, CategoryName = "Marketing", Description = "Các khóa học về Digital Marketing", Icon = "bi-megaphone", IsActive = true },
                new Category { CategoryId = 4, CategoryName = "Kinh doanh", Description = "Các khóa học về quản trị kinh doanh", Icon = "bi-briefcase", IsActive = true },
                new Category { CategoryId = 5, CategoryName = "Ngoại ngữ", Description = "Các khóa học ngoại ngữ", Icon = "bi-translate", IsActive = true }
            );
            // Cấu hình cho Chat
            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.User1)
                .WithMany()
                .HasForeignKey(c => c.User1Id)
                .OnDelete(DeleteBehavior.Restrict); // Xóa User không tự động xóa Chat để tránh lỗi SQL

            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.User2)
                .WithMany()
                .HasForeignKey(c => c.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);



        }
    }
}