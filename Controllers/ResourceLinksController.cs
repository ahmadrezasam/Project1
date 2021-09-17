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
    public class ResourceLinksController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ResourceLinksController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/ResourceLinks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceLinks>>> GetResourceLink()
        {
            return await _context.ResourceLinks.ToListAsync();
        }

        //GET: api/ResourceLinks/5
        [HttpGet("{id}")]
        public async Task<ResourceLinks> GetResourceLink(int id)
        {
            return await _context.ResourceLinks
                .Include(l => l.Tools)
                .SingleOrDefaultAsync(l => l.Id == id);
        }

        // PUT: api/ResourceLinks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResourceLink(int id, ResourceLinks resourceLink)
        {
            if (id != resourceLink.Id)
            {
                return BadRequest();
            }

            _context.Entry(resourceLink).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceLinkExists(id))
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

        // POST: api/ResourceLinks
        [HttpPost]
        public async Task<ActionResult<ResourceLinks>> PostResourceLink(ResourceLinks resourceLink)
        {
            _context.ResourceLinks.Add(resourceLink);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResourceLink", new { id = resourceLink.Id }, resourceLink);
        }

        // DELETE: api/ResourceLinks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResourceLinks>> DeleteResourceLink(int id)
        {
            var resourceLink = await _context.ResourceLinks.FindAsync(id);
            if (resourceLink == null)
            {
                return NotFound();
            }

            _context.ResourceLinks.Remove(resourceLink);
            await _context.SaveChangesAsync();

            return resourceLink;
        }

        private bool ResourceLinkExists(int id)
        {
            return _context.ResourceLinks.Any(e => e.Id == id);
        }
    }
}
