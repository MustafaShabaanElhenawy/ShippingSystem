using System;
using System.Web.Http;
using TrainingShippingSystem.BLL;
using TrainingShippingSystem.Filters;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.Controllers
{
    [AdminOnly]
    [RoutePrefix("api/AuditLogs")]
    public class AuditLogsController : ApiController
    {
        private readonly AuditLogBLL auditLogBLL;

        public AuditLogsController()
        {
            auditLogBLL = new AuditLogBLL();
        }

        // GET: api/AuditLogs
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAuditLogs(
            [FromUri] AuditLogFilter filter)
        {
            try
            {
                if (filter == null)
                    filter = new AuditLogFilter();

                AuditLogPagedResult result =
                    auditLogBLL.GetAuditLogs(filter);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // GET: api/AuditLogs/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetAuditLogByID(int id)
        {
            try
            {
                AuditLog auditLog =
                    auditLogBLL.GetAuditLogByID(id);

                if (auditLog == null)
                    return NotFound();

                var details =
                    auditLogBLL.GetAuditLogDetails(id);

                return Ok(new
                {
                    AuditLog = auditLog,
                    Details = details
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // GET: api/AuditLogs/Users
        [HttpGet]
        [Route("Users")]
        public IHttpActionResult GetAuditUsers()
        {
            try
            {
                return Ok(auditLogBLL.GetAuditUsers());
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // GET: api/AuditLogs/Modules
        [HttpGet]
        [Route("Modules")]
        public IHttpActionResult GetAuditModules()
        {
            try
            {
                return Ok(auditLogBLL.GetAuditModules());
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}