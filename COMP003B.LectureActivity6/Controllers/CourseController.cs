using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using COMP003B.LectureActivity6.Data; 
using COMP003B.LectureActivity6.Models;

namespace COMP003B.LectureActivity6.Controllers
{
    public class CourseController : Controller
    {
        private readonly WebDevAcademyContext _context;

        public CourseController(WebDevAcademyContext context)
        {
            _context = context;
        }
        //Get:Courses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.ToListAsync());
        }
        //Get:Courses/Details
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
                return NotFound();
            
            //get students enrolled in the course
            ViewBag.Students =
                from s in _context.Students
                join e in _context.Enrollments on s.StudentId equals e.StudentId
                join c in _context.Courses on e.CourseId equals c.CourseId
                where c.CourseId == id
                select s;

            return View(course);
        }
        
        //GET: courses/create
        public IActionResult Create()
        {
            return View();
        }
        //Post: courses/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Title")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }
        //Get Courses/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            return View(course);
        }
        //Post: Courses/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Title")] Course course)
        {
            if (id != course.CourseId)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Courses.Any(e => e.CourseId == id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }
        //Get: Courses Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
                return NotFound();

            return View(course);
        }
        //Post: course Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
                _context.Courses.Remove(course);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}