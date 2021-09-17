using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystemCore.Models;

namespace StudentManagementSystemCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IConfigurationDbContext _identityConfig;

        public ToolsController(ApplicationDBContext context, IConfigurationDbContext identityConfig)
        {
            _context = context;
            _identityConfig = identityConfig;
        }

        // GET: api/Tools
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tools>>> GetTool()
        {
            return await _context.Tools.ToListAsync();
        }

        // GET: api/Tools/5
        [HttpGet("{id}")]
        public async Task<Tools> GetTools(int id)
        {
            return await _context.Tools.FindAsync(id);

        }

        // PUT: api/Tools/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTools(int id, Tools tools)
        {
            if (id != tools.Id)
            {
                return BadRequest();
            }

            _context.Entry(tools).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToolsExists(id))
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

        // POST: api/Tools
        [HttpPost]
        public async Task<ActionResult<Tools>> PostTools(Tools tools)
        {
            tools.ClientId = CryptoRandom.CreateUniqueId(8);
            tools.DeploymentId = CryptoRandom.CreateUniqueId(8);
            _context.Tools.Add(tools);
            await _context.SaveChangesAsync();
            var client = new IdentityServer4.Models.Client
            {
                ClientId = tools.ClientId,
                ClientName = tools.Name,
                AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
                AllowedScopes = Config.LtiScopes,
                ClientSecrets = new List<IdentityServer4.Models.Secret>
                {
                    new IdentityServer4.Models.Secret
                    {
                        Type = LtiAdvantage.IdentityServer4.Validation.Constants.SecretTypes.PublicPemKey,
                        Value = tools.PublicKey
                    }
                },
                RedirectUris = { tools.LaunchUrl },
                RequireConsent = false
            };
            var entity = client.ToEntity();
            await _identityConfig.Clients.AddAsync(entity);
            await _identityConfig.SaveChangesAsync();
            return CreatedAtAction("GetTools", new { id = tools.Id }, tools);
        }

        // DELETE: api/Tools/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tools>> DeleteTools(int id)
        {
            var tools = await _context.Tools.FindAsync(id);
            if (tools == null)
            {
                return NotFound();
            }

            _context.Tools.Remove(tools);
            await _context.SaveChangesAsync();

            return tools;
        }

        private bool ToolsExists(int id)
        {
            return _context.Tools.Any(e => e.Id == id);
        }
    }
}
