using System;
using System.Net;
using System.Web.Http;
using TrainingShippingSystem.BLL;

namespace TrainingShippingSystem.Controllers
{
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ApiController
    {
        private readonly DashboardBLL dashboardBLL;

        public DashboardController()
        {
            dashboardBLL = new DashboardBLL();
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetDashboardStatistics()
        {
            try
            {
                var statistics = dashboardBLL.GetDashboardStatistics();

                if (statistics == null)
                {
                    return NotFound();
                }

                return Ok(statistics);
            }
            catch (Exception)
            {
                return Content(
                    HttpStatusCode.InternalServerError,
                    "Something went wrong. Please try again."
                );
            }
        }
    }
}