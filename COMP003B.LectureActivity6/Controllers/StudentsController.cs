using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using COMP003B.LectureActivity6.Data; 
using COMP003B.LectureActivity6.Models;

namespace COMP003B.LectureActivity6.Controllers
{
    public class StudentsController : Controller
    {
        private readonly WebDevAcademyContext _context;

        public StudentsController(WebDevAcademyContext context)
        {
            _context = context;
        }
        //Get:students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }
        //Get:students/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
                return NotFound();
            
            //get students enrolled in the course
            ViewBag.Courses =
                from s in _context.Students
                join e in _context.Enrollments on s.StudentId equals e.StudentId
                join c in _context.Courses on e.CourseId equals c.CourseId
                where s.StudentId == id
                select c;

            return View(student);
        }
        
        //GET: students/create
        public IActionResult Create()
        {
            return View();
        }
        //Post: students/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,Name,Email,Age")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }
        //Get students/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            return View(student);
        }
        //Post: Students/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,Name,Email,Age")] Student student)
        {
            if (id != student.StudentId)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Students.Any(e => e.StudentId == id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }
        //Get: Students Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
                return NotFound();

            return View(student);
        }
        //Post: students Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
                _context.Students.Remove(student);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}