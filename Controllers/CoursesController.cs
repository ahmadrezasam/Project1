using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystemCore.Models;

namespace StudentManagementSystemCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public CoursesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Courses>>> GetCourseMst()
        {
            return await _context.Courses.ToListAsync();
        }

        //api for LTI 1.3 LTI resourcelink launch
        //[HttpGet("resourcelink")]


        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Courses>> GetCourseMst(int id)
        {
            return await _context.Courses
                .Include(l => l.Students)
                .SingleOrDefaultAsync(l => l.Id == id);
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseMst(int id, Courses course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseMstExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<Courses>> PostCourseMst(Courses course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourseMst", new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Courses>> DeleteCourseMst(int id)
        {
            var courseMst = await _context.Courses.FindAsync(id);
            if (courseMst == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(courseMst);
            await _context.SaveChangesAsync();

            return courseMst;
        }

        private bool CourseMstExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
