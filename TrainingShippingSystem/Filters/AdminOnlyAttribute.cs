using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TrainingShippingSystem.Filters
{
    // Allows only users whose role is "Admin".
    // Signed in but not admin -> 403. Not signed in -> 401.
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        public AdminOnlyAttribute()
        {
            Roles = "Admin";
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var principal = actionContext.RequestContext.Principal;

            if (principal != null && principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.Forbidden,
                    new { message = "You do not have permission to access this resource." });
            }
            else
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
        }
    }
}