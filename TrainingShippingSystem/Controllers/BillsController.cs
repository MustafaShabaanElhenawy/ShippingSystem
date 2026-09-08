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
        public BillsController()
        {
            billsBLL = new BillsBLL();
        }
        // =========================================
        // GET ALL BILLS
        // =========================================
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetBills()
        {
            try
            {
                var bills =
                    billsBLL.GetBills();
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
        // =========================================
        // GET BILL BY ID
        // =========================================
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetBillByID(int id)
        {
            try
            {
                var bill =
                    billsBLL.GetBillByID(id);
                if (bill == null)
                {
                    return NotFound();
                }
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
        // =========================================
        // INSERT BILL
        // =========================================
        [HttpPost]
        [Route("")]
        public IHttpActionResult InsertBill(Bill bill)
        {
            try
            {
                int id =
                    billsBLL.InsertBill(bill);
                return Content(
                    HttpStatusCode.Created,
                    new
                    {
                        ID = id,
                        Message =
                            "Bill created successfully."
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
        // =========================================
        // UPDATE BILL
        // =========================================
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateBill(
            int id,
            Bill bill)
        {
            try
            {
                if (bill == null)
                {
                    return BadRequest(
                        "Bill data is required."
                    );
                }
                bill.ID = id;
                int rowsAffected =
                    billsBLL.UpdateBill(bill);
                if (rowsAffected == 0)
                {
                    return NotFound();
                }
                return Ok(
                    new
                    {
                        Message =
                            "Bill updated successfully."
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
        // =========================================
        // DELETE BILL
        // =========================================
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteBill(int id)
        {
            try
            {
                int rowsAffected =
                    billsBLL.DeleteBill(id);
                if (rowsAffected == 0)
                {
                    return NotFound();
                }
                return Ok(
                    new
                    {
                        Message =
                            "Bill deleted successfully."
                    }
                );
            }
            catch (Exception)
            {
                return Content(
                    HttpStatusCode.InternalServerError,
                    "Something went wrong. Please try again."
                );
            }
        }
        // =========================================
        // ADVANCED SEARCH
        // =========================================
        [HttpGet]
        [Route("AdvancedSearch")]
        public IHttpActionResult AdvancedSearch(
            string billNumber = null,
            int? clientID = null,
            int? voyageID = null)
        {
            try
            {
                var bills =
                    billsBLL.AdvancedSearchBills(
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
        // =========================================
        // FILTER + SEARCH + PAGINATION + SORTING
        // =========================================
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
                var result =
                    billsBLL.FilterBills(
                        billNumber,
                        clientID,
                        voyageID,
                        pageNumber,
                        pageSize,
                        sortBy,
                        sortDirection
                    );
                return Ok(
                    new
                    {
                        Bills = result.Bills,
                        TotalRecords =
                            result.TotalRecords
                    }
                );
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