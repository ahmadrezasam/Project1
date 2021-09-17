using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer4.EntityFramework.Entities;
//using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.Extensions;
using LtiAdvantage;
using LtiAdvantage.IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudentManagementSystemCore.Controllers;
using StudentManagementSystemCore.Models;
using LtiAdvantage.IdentityServer4;
using IdentityModel.Internal;
using System.Text;

namespace StudentManagementSystemCore.LTI
{

    [ApiController]
    [Route("lti/launch")]
    public class LTI : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<LTI> _logger;

        public LTI(
                    ApplicationDBContext context,
                    ILogger<LTI> logger)
        {
            _context = context;
            _logger = logger;

        }

        public async Task<IActionResult> OnGetAsync(int id, string messageType, string courseId, string studentId)
        {
            Tools tool;

            if (messageType == LtiAdvantage.Constants.Lti.LtiResourceLinkRequestMessageType)
            {
                var resourceLink = await _context.GetResourceLinkAsync(id);
                if (resourceLink == null)
                {
                    _logger.LogError("Resource link not found.");
                    return BadRequest();
                }

                tool = resourceLink.Tools;
            }
            else
            {
                tool = await _context.GetToolAsync(id);
            }


            if (tool == null)
            {
                _logger.LogError("Tool not found.");
                return BadRequest();
            }

            var values = new
            {
                iss = Request.HttpContext.GetIdentityServerIssuerUri(),
                target_link_uri = tool.LaunchUrl,
                login_hint = studentId,
                lti_message_hint = JsonConvert.SerializeObject(new { id, messageType, courseId })
            };

            var url = new RequestUrl(tool.LoginUrl.Replace("\r\n", string.Empty)).Create(values);

            _logger.LogInformation($"Launching {tool.Name} using GET {url}");
            return Redirect(url);

            // Uncomment to use form POST to initiate login
            // _logger.LogInformation($"Launching {resourceLink.Title} using POST {tool.LoginUrl}");
            // return Post(tool.LoginUrl, values);
        }

    }
}
