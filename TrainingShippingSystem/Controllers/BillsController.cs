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
        private readonly BillBLL billBLL = new BillBLL();


        // GET: api/Bills
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetBills()
        {
            try
            {
                var bills = billBLL.GetBills();

                return Ok(bills);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


        // GET: api/Bills/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetBillByID(int id)
        {
            try
            {
                var bill = billBLL.GetBillByID(id);

                if (bill == null)
                    return NotFound();

                return Ok(bill);
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


        // POST: api/Bills
        [HttpPost]
        [Route("")]
        public IHttpActionResult InsertBill([FromBody] Bill bill)
        {
            try
            {
                int newID = billBLL.InsertBill(bill);

                bill.ID = newID;

                return Content(HttpStatusCode.Created, bill);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // PUT: api/Bills/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateBill(
            int id,
            [FromBody] Bill bill)
        {
            try
            {
                if (bill == null)
                    return BadRequest("Bill data is required.");

                bill.ID = id;

                bool updated = billBLL.UpdateBill(bill);

                if (!updated)
                    return NotFound();

                return Ok(bill);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // DELETE: api/Bills/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteBill(int id)
        {
            try
            {
                bool deleted = billBLL.DeleteBill(id);

                if (!deleted)
                    return NotFound();

                return Ok(new
                {
                    message = "Bill deleted successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // GET: api/Bills/search?search=BL2026
        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchBills(string search)
        {
            try
            {
                var bills = billBLL.SearchBills(search);

                return Ok(bills);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}