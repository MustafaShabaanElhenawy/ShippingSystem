using System;
using System.Net;
using System.Web.Http;
using TrainingShippingSystem.BLL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.Controllers
{
    [RoutePrefix("api/Voyages")]
    public class VoyagesController : ApiController
    {
        private readonly VoyageBLL voyageBLL = new VoyageBLL();


        // GET: api/Voyages
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetVoyages()
        {
            try
            {
                var voyages = voyageBLL.GetVoyages();

                return Ok(voyages);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


        // GET: api/Voyages/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetVoyageByID(int id)
        {
            try
            {
                var voyage = voyageBLL.GetVoyageByID(id);

                if (voyage == null)
                    return NotFound();

                return Ok(voyage);
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


        // POST: api/Voyages
        [HttpPost]
        [Route("")]
        public IHttpActionResult InsertVoyage([FromBody] Voyage voyage)
        {
            try
            {
                int newID = voyageBLL.InsertVoyage(voyage);

                voyage.ID = newID;

                return Content(HttpStatusCode.Created, voyage);
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


        // PUT: api/Voyages/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateVoyage(int id, [FromBody] Voyage voyage)
        {
            try
            {
                if (voyage == null)
                    return BadRequest("Voyage data is required.");

                voyage.ID = id;

                bool updated = voyageBLL.UpdateVoyage(voyage);

                if (!updated)
                    return NotFound();

                return Ok(voyage);
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


        // DELETE: api/Voyages/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteVoyage(int id)
        {
            try
            {
                bool deleted = voyageBLL.DeleteVoyage(id);

                if (!deleted)
                    return NotFound();

                return Ok(new
                {
                    message = "Voyage deleted successfully."
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


        // GET: api/Voyages/search?search=Ocean
        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchVoyages(string search)
        {
            try
            {
                var voyages = voyageBLL.SearchVoyages(search);

                return Ok(voyages);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}