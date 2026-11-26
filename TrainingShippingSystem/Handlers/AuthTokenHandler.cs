////using System.Net.Http;
////using System.Security.Principal;
////using System.Threading;
////using System.Threading.Tasks;
////using System.Web.Http;
////using TrainingShippingSystem.BLL;

////namespace TrainingShippingSystem.Handlers
////{
////    public class AuthTokenHandler : DelegatingHandler
////    {
////        protected override Task<HttpResponseMessage> SendAsync(
////            HttpRequestMessage request, CancellationToken cancellationToken)
////        {
////            var auth = request.Headers.Authorization;

////            if (auth != null && auth.Scheme == "Bearer" &&
////                !string.IsNullOrWhiteSpace(auth.Parameter))
////            {
////                string username = AuthBLL.GetUserNameByToken(auth.Parameter);

////                if (username != null)
////                {
////                    request.GetRequestContext().Principal =
////                        new GenericPrincipal(new GenericIdentity(username, "Token"), null);
////                }
////            }

////            return base.SendAsync(request, cancellationToken);
////        }
////    }
////}


//using System.Net.Http;
//using System.Security.Principal;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web.Http;
//using TrainingShippingSystem.BLL;

//namespace TrainingShippingSystem.Handlers
//{
//    // Reads "Authorization: Bearer <token>" and, if the token is valid,
//    // sets the request's User (name + role).
//    public class AuthTokenHandler : DelegatingHandler
//    {
//        protected override Task<HttpResponseMessage> SendAsync(
//            HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            var auth = request.Headers.Authorization;

//            if (auth != null &&
//                auth.Scheme == "Bearer" &&
//                !string.IsNullOrWhiteSpace(auth.Parameter))
//            {
//                string username;
//                string role;

//                if (AuthBLL.TryGetSession(auth.Parameter, out username, out role))
//                {
//                    request.GetRequestContext().Principal =
//                        new GenericPrincipal(
//                            new GenericIdentity(username, "Token"),
//                            new[] { role });
//                }
//            }

//            return base.SendAsync(request, cancellationToken);
//        }
//    }
//}



using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using TrainingShippingSystem.BLL;

namespace TrainingShippingSystem.Handlers
{
    // Reads "Authorization: Bearer <token>" and, if the session is valid,
    // sets the request's User (name + role) so [Authorize] / [AdminOnly]
    // and GetAuditUserName() work in every controller.
    public class AuthTokenHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var auth = request.Headers.Authorization;

            if (auth != null &&
                auth.Scheme == "Bearer" &&
                !string.IsNullOrWhiteSpace(auth.Parameter))
            {
                string username;
                string role;

                if (AuthBLL.TryGetSession(auth.Parameter, out username, out role))
                {
                    request.GetRequestContext().Principal =
                        new GenericPrincipal(
                            new GenericIdentity(username, "Token"),
                            new[] { role });
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}