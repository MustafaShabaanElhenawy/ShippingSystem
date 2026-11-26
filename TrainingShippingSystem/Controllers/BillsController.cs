using System;
using System.Net;
using System.Web.Http;
using TrainingShippingSystem.BLL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.Controllers
{
    [RoutePrefix("api/Bills")]
    public class BillsController : ApiController
    {
        private readonly BillsBLL billsBLL;
        private readonly AuditLoggingService auditLoggingService;

        public BillsController()
        {
            billsBLL = new BillsBLL();
            auditLoggingService = new AuditLoggingService();
        }

        private string GetAuditUserName()
        {
            if (User != null &&
                User.Identity != null &&
                User.Identity.IsAuthenticated &&
                !string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                return User.Identity.Name;
            }

            return "Anonymous";
        }

        // GET ALL BILLS
        // GET: api/Bills
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetBills()
        {
            try
            {
                var bills = billsBLL.GetBills();
                return Ok(bills);
            }
            catch (Exception)
            {
                return Content(
                    HttpStatusCode.InternalServerError,
                    "Something went wrong. Please try again."
                );
            }
        }

        // GET BILL BY ID
        // GET: api/Bills/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetBillByID(int id)
        {
            try
            {
                var bill = billsBLL.GetBillByID(id);

                if (bill == null)
                    return NotFound();

                return Ok(bill);
            }
            catch (Exception)
            {
                return Content(
                    HttpStatusCode.InternalServerError,
                    "Something went wrong. Please try again."
                );
            }
        }

        // INSERT BILL
        // POST: api/Bills
        [HttpPost]
        [Route("")]
        public IHttpActionResult InsertBill(Bill bill)
        {
            try
            {
                int id = billsBLL.InsertBill(bill);

                var createdBill = billsBLL.GetBillByID(id) ?? bill;

                auditLoggingService.LogCreate(
                    GetAuditUserName(),
                    "Bills",
                    id,
                    "Created bill: " + bill.BillNumber,
                    AuditLogChangeTracker.ForCreate(createdBill)
                );

                return Content(
                    HttpStatusCode.Created,
                    new
                    {
                        ID = id,
                        Message = "Bill created successfully."
                    }
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Content(
                    HttpStatusCode.InternalServerError,
                    "Something went wrong. Please try again."
                );
            }
        }

        // UPDATE BILL
        // PUT: api/Bills/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateBill(int id, Bill bill)
        {
            try
            {
                if (bill == null)
                    return BadRequest("Bill data is required.");

                var existingBill = billsBLL.GetBillByID(id);

                if (existingBill == null)
                    return NotFound();

                bill.ID = id;

                int rowsAffected = billsBLL.UpdateBill(bill);

                if (rowsAffected == 0)
                    return NotFound();

                var updatedBill = billsBLL.GetBillByID(id);

                var changedDetails =
                    AuditLogChangeTracker.Compare(
                        existingBill,
                        updatedBill
                    );

                if (changedDetails.Count > 0)
                {
                    auditLoggingService.LogUpdate(
                        GetAuditUserName(),
                        "Bills",
                        id,
                        "Updated bill: " + updatedBill.BillNumber,
                        changedDetails
                    );
                }

                return Ok(new
                {
                    Message = "Bill updated successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Content(
                    HttpStatusCode.InternalServerError,
                    "Something went wrong. Please try again."
                );
            }
        }

        // DELETE BILL
        // DELETE: api/Bills/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteBill(int id)
        {
            try
            {
                var existingBill = billsBLL.GetBillByID(id);

                if (existingBill == null)
                    return NotFound();

                int rowsAffected = billsBLL.DeleteBill(id);

                if (rowsAffected == 0)
                    return NotFound();

                auditLoggingService.LogDelete(
                    GetAuditUserName(),
                    "Bills",
                    id,
                    "Deleted bill: " + existingBill.BillNumber,
                    AuditLogChangeTracker.ForDelete(existingBill)
                );

                return Ok(new
                {
                    Message = "Bill deleted successfully."
                });
            }
            catch (Exception)
            {
                return Content(
                    HttpStatusCode.InternalServerError,
                    "Something went wrong. Please try again."
                );
            }
        }

        // ADVANCED SEARCH
        // GET: api/Bills/AdvancedSearch
        [HttpGet]
        [Route("AdvancedSearch")]
        public IHttpActionResult AdvancedSearch(
            string billNumber = null,
            int? clientID = null,
            int? voyageID = null)
        {
            try
            {
                var bills = billsBLL.AdvancedSearchBills(
                    billNumber,
                    clientID,
                    voyageID
                );

                return Ok(bills);
            }
            catch (Exception)
            {
                return Content(
                    HttpStatusCode.InternalServerError,
                    "Something went wrong. Please try again."
                );
            }
        }

        // FILTER + SEARCH + PAGINATION + SORTING
        // GET: api/Bills/Filter
        [HttpGet]
        [Route("Filter")]
        public IHttpActionResult FilterBills(
            string billNumber = null,
            int? clientID = null,
            int? voyageID = null,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "ID",
            string sortDirection = "DESC")
        {
            try
            {
                var result = billsBLL.FilterBills(
                    billNumber,
                    clientID,
                    voyageID,
                    pageNumber,
                    pageSize,
                    sortBy,
                    sortDirection
                );

                return Ok(new
                {
                    Bills = result.Bills,
                    TotalRecords = result.TotalRecords
                });
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