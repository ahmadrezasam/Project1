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
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public StudentsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentMst()
        {
            return await _context.Student.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentMst(int id)
        {
            var studentMst = await _context.Student.FindAsync(id);

            if (studentMst == null)
            {
                return NotFound();
            }

            return studentMst;
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentMst(int id, Student studentMst)
        {
            if (id != studentMst.ID)
            {
                return BadRequest();
            }

            _context.Entry(studentMst).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentMstExists(id))
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

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudentMst(Student studentMst)
        {
            _context.Student.Add(studentMst);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentMst", new { id = studentMst.ID }, studentMst);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudentMst(int id)
        {
            var studentMst = await _context.Student.FindAsync(id);
            if (studentMst == null)
            {
                return NotFound();
            }

            _context.Student.Remove(studentMst);
            await _context.SaveChangesAsync();

            return studentMst;
        }

        private bool StudentMstExists(int id)
        {
            return _context.Student.Any(e => e.ID == id);
        }
    }
}
