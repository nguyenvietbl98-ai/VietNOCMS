using Microsoft.AspNetCore.Authentication; 
using Microsoft.AspNetCore.Authorization;  
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;        
using System.Security.Claims;              
using VietNOCMS.Data;                    
using VietNOCMS.Models;
using VietNOCMS.ViewModels;
using static VietNOCMS.Models.SettingViewModel;


// Models như User, ViewModel

namespace VietNOCMS.Controllers
{
    public class AccountController : Controller
    {
        //dependency
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger; 
        private readonly IWebHostEnvironment _environment;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context; 
            _logger = logger;   
           
                _webHostEnvironment = webHostEnvironment;
        }

     
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
         
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            try
            {
              
                var email = model.Email?.Trim().ToLower();
                var password = model.Password?.Trim();

             
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email && u.IsActive);

                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    user.LastLoginAt = DateTime.Now;
                    await _context.SaveChangesAsync();

                
                    var claims = new List<Claim>
            {
                        // 
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(2)
                    };

                    await HttpContext.SignInAsync("CookieAuth", claimsPrincipal, authProperties);

                  
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserName", user.FullName);
                    HttpContext.Session.SetString("UserRole", user.Role);
                    HttpContext.Session.SetString("UserAvatar", user.Avatar ?? "");

                    _logger.LogInformation($"User {user.Email} logged in successfully.");

                 
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return user.Role switch
                    {
                        "Admin" => RedirectToAction("Dashboard", "Home"),
                        "Instructor" => RedirectToAction("Dashboard", "Home"),
                        _ => RedirectToAction("Index", "Home")
                    };
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không chính xác");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi đăng nhập. Vui lòng thử lại.");
            }

            return View(model);
        }


    
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
        
            if (!model.AgreeToTerms)
            {
                ModelState.AddModelError("AgreeToTerms", "Bạn phải đồng ý với điều khoản sử dụng.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                try
                {
                
                    if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                    {
                        ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                        return View(model);
                    }

                
                    var user = new User
                    {
                        FullName = model.FullName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Role = "Student",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync(); 

               
                    if (model.Role == "Teacher")
                    {
                        string cvFileName = null;

                        if (model.CV != null && model.CV.Length > 0)
                        {
                        
                            string wwwrootPath = _webHostEnvironment.WebRootPath;
                            if (string.IsNullOrEmpty(wwwrootPath))
                            {
                                wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                            }

                            string uploadsFolder = Path.Combine(wwwrootPath, "uploads", "cvs");

                        
                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }
                         
                            string extension = Path.GetExtension(model.CV.FileName);
                            cvFileName = $"{user.UserId}_{Guid.NewGuid()}{extension}";
                            string filePath = Path.Combine(uploadsFolder, cvFileName);

                       
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await model.CV.CopyToAsync(stream);
                            }
                        }

                        var request = new InstructorRequest
                        {
                            UserId = user.UserId,
                            Bio = model.InstructorDescription ?? "Chưa cập nhật",
                            CvUrl = cvFileName,
                            Status = "Pending",
                            RequestedAt = DateTime.Now
                        };

                        _context.InstructorRequests.Add(request);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = "Đăng ký giảng viên thành công! Vui lòng chờ duyệt.";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Đăng ký thành công!";
                    }

                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                   
                    ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError("", "Chi tiết: " + ex.InnerException.Message);
                    }
                }
            }

            return View(model);
        }


      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth"); 
            HttpContext.Session.Clear();                

            _logger.LogInformation("User logged out.");

            return RedirectToAction("Index", "Home");
        }

      
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Setting()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

           
            var purchases = await _context.Enrollments
                .Where(e => e.StudentId == userId)
                .Include(e => e.Course)
                .OrderByDescending(e => e.EnrollmentAt)
                .Select(e => new EnrollmentHistoryItem
                {
                    CourseName = e.Course.CourseName,
                    Date = e.EnrollmentAt,
                    Amount = e.PaidAmount,
                    Status = e.PaymentStatus
                }).ToListAsync();

          
            var activities = await _context.LessonProgresses
                .Where(p => p.Enrollment.StudentId == userId)
                .Include(p => p.Lesson).ThenInclude(l => l.Chapter).ThenInclude(c => c.Course)
                .OrderByDescending(p => p.LastAccessedAt)
                .Take(5) 
                .Select(p => new LearningHistoryItem
                {
                    CourseName = p.Lesson.Chapter.Course.CourseName,
                    LessonName = p.Lesson.LessonName,
                    AccessTime = p.LastAccessedAt
                }).ToListAsync();

            var model = new SettingViewModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                Avatar = user.Avatar, 
                PurchaseHistory = purchases,
                RecentActivities = activities
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile avatarFile)
        {
            if (avatarFile == null || avatarFile.Length == 0)
                return Json(new { success = false, message = "Chưa chọn file!" });

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Users.FindAsync(userId);

         
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "avatars");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            var fileName = $"{userId}_{Guid.NewGuid()}{Path.GetExtension(avatarFile.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatarFile.CopyToAsync(stream);
            }

        
            if (!string.IsNullOrEmpty(user.Avatar))
            {
                var oldPath = Path.Combine(uploadPath, user.Avatar);
                if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
            }

         
            user.Avatar = fileName;
            HttpContext.Session.SetString("UserAvatar", fileName);
            await _context.SaveChangesAsync();

            return Json(new { success = true, newUrl = "/uploads/avatars/" + fileName });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfileInfo(string fullName, string phone)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Users.FindAsync(userId);

            if (user != null)
            {
                user.FullName = fullName;
                user.PhoneNumber = phone;
                await _context.SaveChangesAsync();

              

                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Lỗi không tìm thấy user" });
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

       
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

                    if (string.IsNullOrEmpty(userId))
                    {
                        return RedirectToAction(nameof(Login));
                    }

                    var user = await _context.Users.FindAsync(int.Parse(userId)); 

                    if (user == null)
                    {
                        return NotFound();
                    }


                    if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.PasswordHash))
                    {
                        ModelState.AddModelError("OldPassword", "Mật khẩu cũ không chính xác");
                        return View(model);
                    }

                  
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"User {user.Email} changed password successfully.");

                    TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                    return RedirectToAction(nameof(Setting));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during password change");
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi đổi mật khẩu. Vui lòng thử lại.");
                }
            }

            return View(model);
        }
        [HttpGet]
      

    
        public IActionResult AccessDenied()
        {
            return View(); 
        }
        public async Task<IActionResult> Profile(int? id) 
        {
          
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

          
            if (id == null)
            {
                if (string.IsNullOrEmpty(currentUserId)) return Challenge(); 
                id = int.Parse(currentUserId);
            }

           
            var instructor = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id && u.Role == "Instructor");

            if (instructor == null) return NotFound();

           
            var courses = await _context.Courses
                .Include(c => c.Category)
                .Where(c => c.InstructorId == id.Value && c.IsPublished)
                .ToListAsync();

            var viewModel = new InstructorProfileViewModel
            {
                Instructor = instructor,
                Courses = courses,
                TotalStudents = courses.Sum(c => c.EnrollmentCount)
            };

            return View(viewModel);
        }
    }
}
