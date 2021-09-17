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
    public class TeachersController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public TeachersController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeacherMst()
        {
            return await _context.Teacher.ToListAsync();
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacherMst(int id)
        {
            var teacherMst = await _context.Teacher.FindAsync(id);

            if (teacherMst == null)
            {
                return NotFound();
            }

            return teacherMst;
        }

        // PUT: api/Teachers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacherMst(int id, Teacher teacherMst)
        {
            if (id != teacherMst.ID)
            {
                return BadRequest();
            }

            _context.Entry(teacherMst).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherMstExists(id))
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

        // POST: api/Teachers
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacherMst(Teacher teacherMst)
        {
            _context.Teacher.Add(teacherMst);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeacherMst", new { id = teacherMst.ID }, teacherMst);
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Teacher>> DeleteTeacherMst(int id)
        {
            var teacherMst = await _context.Teacher.FindAsync(id);
            if (teacherMst == null)
            {
                return NotFound();
            }

            _context.Teacher.Remove(teacherMst);
            await _context.SaveChangesAsync();

            return teacherMst;
        }

        private bool TeacherMstExists(int id)
        {
            return _context.Teacher.Any(e => e.ID == id);
        }
    }
}
