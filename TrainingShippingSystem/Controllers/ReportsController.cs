using System;
using System.Web.Http;
using TrainingShippingSystem.BLL;

namespace TrainingShippingSystem.Controllers
{
    public class ReportsController : ApiController
    {
        private readonly ShippingReportBLL reportBLL;

        public ReportsController()
        {
            reportBLL = new ShippingReportBLL();
        }

        [HttpGet]
        public IHttpActionResult GetShippingSummary()
        {
            try
            {
                var reports =
                    reportBLL.GetShippingSummary();

                return Ok(reports);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}