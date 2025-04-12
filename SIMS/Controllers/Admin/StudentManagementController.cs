using Microsoft.AspNetCore.Mvc;
using SIMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIMS.ViewModels;

namespace SIMS.Controllers.Admin
{
    public class StudentManagementController : Controller
    {
        private readonly SIMSDbContext _context;

        public StudentManagementController(SIMSDbContext context)
        {
            _context = context;
        }

        // GET: Admin/StudentManagement
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Major)
                .Include(s => s.User)
                .ToListAsync();

            return View(students);  // Pass the correct model to the view
        }


        // GET: Admin/StudentManagement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Major)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            var model = new CreateStudentViewModel
            {
                Majors = new SelectList(_context.Majors, "MajorId", "MajorName"),
                Classes = new SelectList(Enumerable.Empty<SelectListItem>(), "ClassId", "ClassName")
            };
            return View(model);
        }


        // POST: Student/Create
        [HttpPost]
        public IActionResult Create(CreateStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState không hợp lệ");

                // Đảm bảo danh sách Majors và Classes vẫn còn
                model.Majors = new SelectList(_context.Majors, "MajorId", "MajorName");
                model.Classes = new SelectList(_context.Classes.Where(c => c.MajorId == model.MajorId), "ClassId", "ClassName");

                return View(model);
            }


            try
            {
                Console.WriteLine("🟡 Kiểm tra trùng Email...");
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    Console.WriteLine("❌ Email đã tồn tại: " + model.Email);
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                    model.Majors = new SelectList(_context.Majors, "MajorId", "MajorName");
                    model.Classes = new SelectList(_context.Classes.Where(c => c.MajorId == model.MajorId), "ClassId", "ClassName");
                    return View(model);
                }

                Console.WriteLine("✅ Email chưa tồn tại, tạo User và Student...");

                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Password = "123",
                    RoleId = 3
                };

                _context.Users.Add(user);
                _context.SaveChanges();
                Console.WriteLine("🟢 Đã tạo user: ID = " + user.UserId);

                var student = new Student
                {
                    UserId = user.UserId,
                    MajorId = model.MajorId,
                    ClassId = model.ClassId
                };

                _context.Students.Add(student);
                _context.SaveChanges();
                Console.WriteLine("🟢 Đã tạo student: ID = " + student.StudentId);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lưu dữ liệu: " + ex.ToString());
                ModelState.AddModelError("", "Có lỗi xảy ra khi lưu dữ liệu.");

                model.Majors = new SelectList(_context.Majors, "MajorId", "MajorName");
                model.Classes = new SelectList(_context.Classes.Where(c => c.MajorId == model.MajorId), "ClassId", "ClassName");

                return View(model);
            }
        }



        // AJAX: Get classes theo MajorId
        public IActionResult GetClassesByMajor(int majorId)
        {
            var classes = _context.Classes
                .Where(c => c.MajorId == majorId)
                .Select(c => new { c.ClassId, c.ClassName })
                .ToList();

            return Json(classes);
        }




        // GET: Admin/StudentManagement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassName", student.ClassId);
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName", student.MajorId);
            return View(student);
        }

        // POST: Admin/StudentManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,UserId,MajorId,ClassId")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassName", student.ClassId);
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName", student.MajorId);
            return View(student);
        }

        // GET: Admin/StudentManagement/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            // Load the student along with its related enrollments and grades
            var student = await _context.Students
                .Include(s => s.Enrollments)  // Include enrollments
                .Include(s => s.Grades)       // Include grades
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            // Remove all related grades first
            foreach (var grade in student.Grades.ToList())
            {
                _context.Grades.Remove(grade);
            }

            // Remove all related enrollments
            foreach (var enrollment in student.Enrollments.ToList())
            {
                _context.Enrollments.Remove(enrollment);
            }

            // Now, delete the student
            _context.Students.Remove(student);

            // Save all changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // POST: Admin/StudentManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }

    }
}
