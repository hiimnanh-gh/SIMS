using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIMS.Models;
using SIMS.ViewModels;
using System.Threading.Tasks;

namespace SIMS.Controllers.Auth
{
    public class AuthController : Controller
    {
        private readonly SIMSDbContext _context;

        public AuthController(SIMSDbContext context)
        {
            _context = context;
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {

            var user = await _context.Users
                                      .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Set session data here
                HttpContext.Session.SetInt32("RoleID", user.RoleId);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserPassword", user.Password);
                HttpContext.Session.SetInt32("RoleID", user.RoleId);

                // Redirect after successful login
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View();
        }

        // GET: Register
        public IActionResult Register()
        {
            // Lấy danh sách roles từ database
            ViewBag.Roles = new SelectList(_context.Roles.ToList(), "RoleId", "RoleName");
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Truyền lại danh sách roles vào ViewBag khi có lỗi
                ViewBag.Roles = new SelectList(_context.Roles.ToList(), "RoleId", "RoleName");
                return View(model);
            }

            // Kiểm tra email đã tồn tại chưa
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                ViewBag.Roles = new SelectList(_context.Roles.ToList(), "RoleId", "RoleName");
                return View(model);
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password, // Lưu thẳng mật khẩu
                RoleId = model.RoleId,  // Lưu RoleId được chọn
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }


        // GET: Auth/Logout
        public IActionResult Logout()
        {
            // Clear the session when logging out
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
