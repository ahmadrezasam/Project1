//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using IdentityServer4.Extensions;
//using IdentityServer4.Models;
//using IdentityServer4.Services;
//using IdentityServer4.Validation;
//using LtiAdvantage;
//using LtiAdvantage.IdentityServer4;
//using LtiAdvantage.Lti;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Routing;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json.Linq;
//using StudentManagementSystemCore.Models;

//namespace StudentManagementSystemCore.LTI
//{
//    public class LTIProfileService : IProfileService
//    {
//        private readonly ApplicationDBContext _context;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly LinkGenerator _linkGenerator;
//        private readonly ILogger<LTIProfileService> _logger;

//        public LTIProfileService(ApplicationDBContext context,IHttpContextAccessor httpContextAccessor,           
//            LinkGenerator linkGenerator,ILogger<LTIProfileService> logger)
//        {
//            _context = context;
//            _httpContextAccessor = httpContextAccessor;
//            _linkGenerator = linkGenerator;
//            _logger = logger;
//        }

//        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
//        {
//            if (context.ValidatedRequest is ValidatedAuthorizeRequest request)
//            {
//                _logger.LogDebug("Getting LTI Advantage claims for identity token for subject: {subject} and client: {clientId}",
//                    context.Subject.GetSubjectId(),
//                    request.Client.ClientId);

//                // LTI Advantage authorization requests include an lti_message_hint parameter
//                var ltiMessageHint = request.Raw["lti_message_hint"];
//                if (ltiMessageHint.IsMissing())
//                {
//                    _logger.LogInformation("Not an LTI request.");
//                    return;
//                }

//                // LTI Advantage authorization requests include the the user id in the LoginHint
//                // (also available in the Subject). In this sample platform, the user id is for one
//                // of the tenants' people.
//                if (!int.TryParse(request.LoginHint, out var studentId))
//                {
//                    _logger.LogError("Cannot convert login hint to person id.");
//                }

//                // In this sample platform, the lti_message_hint is a JSON object that includes the
//                // message type (LtiResourceLinkRequest or DeepLinkingRequest), the tenant's course
//                // id, and either the resource link id or the tool id depending on the type of message.
//                // For example, "{"id":3,"messageType":"LtiResourceLinkRequest","courseId":"1"}"
//                var message = JToken.Parse(ltiMessageHint);
//                var id = message.Value<int>("id");
//                // In this sample platform, each application user is a tenant.
//                var student = await _context.GetStudentAsync(studentId);
//                var course = message.Value<int?>("courseId").HasValue ? course.student : null;


//                var messageType = message.Value<string>("messageType");
//                switch (messageType)
//                {
//                    case Constants.Lti.LtiResourceLinkRequestMessageType:
//                        {
//                            var resourceLink = await _context.GetResourceLinkAsync(id);

//                            context.IssuedClaims = GetResourceLinkRequestClaims(
//                                resourceLink, student, course);

//                            break;
//                        }
//                    case Constants.Lti.LtiDeepLinkingRequestMessageType:
//                        {
//                            var tool = await _context.GetToolAsync(id);

//                            //context.IssuedClaims = GetDeepLinkingRequestClaims(
//                            //    tool, person, course, user.Platform);

//                            break;
//                        }
//                    default:
//                        _logger.LogError($"{nameof(messageType)}=\"{messageType}\" not supported.");

//                        break;
//                }
//            }
//        }

//        public Task IsActiveAsync(IsActiveContext context)
//        {
//            return Task.CompletedTask;
//        }

//        private List<Claim> GetResourceLinkRequestClaims(
//            ResourceLinks resourceLink,
//            Student student,
//            Courses course)
//        {
//            var httpRequest = _httpContextAccessor.HttpContext.Request;

//            var request = new LtiResourceLinkRequest
//            {
//                DeploymentId = resourceLink.Tools.DeploymentId,
//                FamilyName = student.Name,
//                GivenName = student.Name,
//                LaunchPresentation = new LaunchPresentationClaimValueType
//                {
//                    DocumentTarget = DocumentTarget.Window,
//                    Locale = CultureInfo.CurrentUICulture.Name,
//                    ReturnUrl = $"{httpRequest.Scheme}://{httpRequest.Host}"
//                },
//                Lis = new LisClaimValueType
//                {
//                    //PersonSourcedId = student.ID,
//                    CourseSectionSourcedId = course?.SisId
//                },
//                Lti11LegacyUserId = student.ID.ToString(),
//                //Platform = new PlatformClaimValueType
//                //{
//                //    ContactEmail = platform.ContactEmail,
//                //    Description = platform.Description,
//                //    Guid = platform.Id.ToString(),
//                //    Name = platform.Name,
//                //    ProductFamilyCode = platform.ProductFamilyCode,
//                //    Url = platform.Url,
//                //    Version = platform.Version
//                //},
//                ResourceLink = new ResourceLinkClaimValueType
//                {
//                    Id = resourceLink.Id.ToString(),
//                    Title = resourceLink.Title,
//                    Description = resourceLink.Description
//                },
//                TargetLinkUri = resourceLink.Tools.LaunchUrl
//            };

//            // Add the context if the launch is from a course.
//            if (course == null)
//            {
//                // Remove context roles
//                request.Roles = request.Roles.Where(r => !r.ToString().StartsWith("Context")).ToArray();
//            }
//            else
//            {
//                request.Context = new ContextClaimValueType
//                {
//                    Id = course.Id.ToString(),
//                    Title = course.Name,
//                    Type = new[] { ContextType.CourseSection }
//                };

//            }

//            // Collect custom properties
//            if (!resourceLink.Tools.CustomProperties.TryConvertToDictionary(out var custom))
//            {
//                custom = new Dictionary<string, string>();
//            }
//            if (resourceLink.CustomProperties.TryConvertToDictionary(out var linkDictionary))
//            {
//                foreach (var property in linkDictionary)
//                {
//                    if (custom.ContainsKey(property.Key))
//                    {
//                        custom[property.Key] = property.Value;
//                    }
//                    else
//                    {
//                        custom.Add(property.Key, property.Value);
//                    }
//                }
//            }

//            // Prepare for custom property substitutions
//            var substitutions = new CustomPropertySubstitutions
//            {
//                LtiUser = new LtiUser
//                {
//                    Username = student.Name
//                }
//            };

//            request.Custom = substitutions.ReplaceCustomPropertyValues(custom);

//            return new List<Claim>(request.Claims);
//        }



//    }
//}
