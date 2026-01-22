using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VietNOCMS.Data;
using VietNOCMS.Models;
using Microsoft.EntityFrameworkCore;

namespace VietNOCMS.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WalletController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");
            var userId = int.Parse(userIdStr);

            var user = await _context.Users.FindAsync(userId);

            var transactions = await _context.Wallet
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Take(10)
                .ToListAsync();

            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var monthlyStats = await _context.Wallet
                .Where(t => t.UserId == userId && t.CreatedAt >= startOfMonth)
                .ToListAsync();

            var viewModel = new WalletViewModel
            {
                CurrentBalance = user.Balance,
                Transactions = transactions,
                TotalDepositThisMonth = monthlyStats.Where(t => t.Type == "Deposit").Sum(t => t.Amount),
                TotalWithdrawThisMonth = monthlyStats.Where(t => t.Type == "Withdraw").Sum(t => t.Amount)
            };

            return View("Wallet", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(string Amount)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");
            var userId = int.Parse(userIdStr);

            // Xử lý chuỗi tiền tệ (bỏ dấu chấm/phẩy)
            decimal amountDec = decimal.Parse(Amount.Replace(".", "").Replace(",", ""));

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            // 1. Cộng tiền
            user.Balance += amountDec;

            // 2. Lưu lịch sử giao dịch
            var transaction = new Wallet
            {
                UserId = userId,
                Amount = amountDec,
                Type = "Deposit",
                Description = "Nạp tiền vào ví (Virtual)",
                Status = "Completed"
            };

            _context.Wallet.Add(transaction);

            await CreateNotification(
                userId: userId,
                title: "Nạp tiền thành công",
                message: $"Đã nạp thành công {amountDec:N0}₫ vào tài khoản.",
                type: NotificationType.Success,
                category: "Payment",
                url: "/Wallet/Index"
            );
            // --------------------------------------

            await _context.SaveChangesAsync(); 

            TempData["SuccessMessage"] = $"Đã nạp thành công {amountDec:N0}₫ vào tài khoản.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(string Amount, string Note)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");
            var userId = int.Parse(userIdStr);

            decimal amountDec = decimal.Parse(Amount.Replace(".", "").Replace(",", ""));

            var user = await _context.Users.FindAsync(userId);
            if (user.Balance < amountDec)
            {
                TempData["ErrorMessage"] = "Số dư không đủ để thực hiện rút tiền.";
                return RedirectToAction("Index");
            }

            // 1. Trừ tiền
            user.Balance -= amountDec;


            var transaction = new Wallet
            {
                UserId = userId,
                Amount = -amountDec,
                Type = "Withdraw",
                Description = "Rút tiền về ngân hàng: " + Note,
                Status = "Pending"
            };

            _context.Wallet.Add(transaction);

          
            await CreateNotification(
                userId: userId,
                title: "Yêu cầu rút tiền",
                message: $"Yêu cầu rút {amountDec:N0}₫ đang chờ duyệt.",
                type: NotificationType.Info, // Màu xanh dương hoặc Warning (Vàng)
                category: "Payment",         // Icon Tiền
                url: "/Wallet/Index"
            );
            // ------------------------------------------------

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Yêu cầu rút tiền đã được gửi và đang chờ duyệt.";
            return RedirectToAction("Index");
        }

        // Hàm phụ trợ tạo thông báo nhanh
        private async Task CreateNotification(int userId, string title, string message, NotificationType type, string category, string? url = null)
        {
            var noti = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,       // Enum: Info, Success, Warning, Error
                Category = category, // String: "System", "Payment", "Assignment"...
                RedirectUrl = url,
                CreatedAt = DateTime.Now,
                IsRead = false
            };
            _context.Notifications.Add(noti);
        }
    }
}