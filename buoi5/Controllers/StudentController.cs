using buoi5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace buoi5.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDBContext _context;

        public StudentController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: /Student/Index
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.Include(s => s.Grade).ToListAsync();
            return View(students);
        }

        // GET: /Student/Create
        public IActionResult Create()
        {
            ViewBag.GradeId = new SelectList(_context.Grades, "GradeId", "GradeName");
            return View(new Student());
        }

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (student == null)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm học sinh thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu: " + ex.Message);
                    TempData["ErrorMessage"] = "Thêm học sinh thất bại: " + ex.Message;
                }
            }

            ViewBag.GradeId = new SelectList(_context.Grades, "GradeId", "GradeName", student.GradeId);
            return View(student);
        }

        // GET: /Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            ViewBag.GradeId = new SelectList(_context.Grades, "GradeId", "GradeName", student.GradeId);
            return View(student);
        }

        // POST: /Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (student == null || id != student.StudentId)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa học sinh thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu: " + ex.Message);
                    TempData["ErrorMessage"] = "Sửa học sinh thất bại: " + ex.Message;
                }
            }

            ViewBag.GradeId = new SelectList(_context.Grades, "GradeId", "GradeName", student.GradeId);
            return View(student);
        }

        // GET: /Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var student = await _context.Students.Include(s => s.Grade).FirstOrDefaultAsync(s => s.StudentId == id);
            if (student == null) return NotFound();
            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa học sinh thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}