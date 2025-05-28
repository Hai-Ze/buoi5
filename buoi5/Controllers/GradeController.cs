using buoi5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace buoi5.Controllers
{
    public class GradeController : Controller
    {
        private readonly ApplicationDBContext _context;

        public GradeController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: /Grade/Index
        public async Task<IActionResult> Index()
        {
            var grades = await _context.Grades.Include(g => g.Students).ToListAsync();
            return View(grades);
        }

        // GET: /Grade/Create
        public IActionResult Create()
        {
            ViewBag.Students = _context.Students.ToList() ?? new List<Student>();
            return View(new Grade());
        }

        // POST: /Grade/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grade grade, int[] selectedStudents)
        {
            if (grade == null)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (grade.Students == null)
                    {
                        grade.Students = new List<Student>();
                    }

                    if (selectedStudents != null && selectedStudents.Any())
                    {
                        var selectedStudentsList = await _context.Students
                            .Where(s => selectedStudents.Contains(s.StudentId))
                            .ToListAsync();
                        grade.Students.AddRange(selectedStudentsList);
                    }

                    _context.Add(grade);
                    await _context.SaveChangesAsync();

                    if (grade.Students.Any())
                    {
                        foreach (var student in grade.Students)
                        {
                            student.GradeId = grade.GradeId;
                            _context.Update(student);
                        }
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Thêm lớp thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu: " + ex.Message);
                    TempData["ErrorMessage"] = "Thêm lớp thất bại: " + ex.Message;
                }
            }

            ViewBag.Students = _context.Students.ToList() ?? new List<Student>();
            return View(grade);
        }

        // GET: /Grade/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var grade = await _context.Grades.Include(g => g.Students).FirstOrDefaultAsync(g => g.GradeId == id);
            if (grade == null) return NotFound();
            ViewBag.Students = _context.Students.ToList() ?? new List<Student>();
            return View(grade);
        }

        // POST: /Grade/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Grade grade, int[] selectedStudents)
        {
            if (grade == null || id != grade.GradeId)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingGrade = await _context.Grades.Include(g => g.Students).FirstOrDefaultAsync(g => g.GradeId == id);
                    if (existingGrade == null) return NotFound();

                    existingGrade.GradeName = grade.GradeName;
                    existingGrade.Students.Clear();

                    if (selectedStudents != null && selectedStudents.Any())
                    {
                        var newStudents = await _context.Students
                            .Where(s => selectedStudents.Contains(s.StudentId))
                            .ToListAsync();
                        existingGrade.Students.AddRange(newStudents);
                    }

                    foreach (var student in existingGrade.Students)
                    {
                        student.GradeId = existingGrade.GradeId;
                        _context.Update(student);
                    }

                    _context.Update(existingGrade);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa lớp thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu: " + ex.Message);
                    TempData["ErrorMessage"] = "Sửa lớp thất bại: " + ex.Message;
                }
            }

            ViewBag.Students = _context.Students.ToList() ?? new List<Student>();
            return View(grade);
        }

        // GET: /Grade/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var grade = await _context.Grades.Include(g => g.Students).FirstOrDefaultAsync(g => g.GradeId == id);
            if (grade == null) return NotFound();
            return View(grade);
        }

        // POST: /Grade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grade = await _context.Grades.Include(g => g.Students).FirstOrDefaultAsync(g => g.GradeId == id);
            if (grade != null)
            {
                foreach (var student in grade.Students)
                {
                    student.GradeId = 0;
                    _context.Update(student);
                }
                _context.Grades.Remove(grade);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa lớp thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}