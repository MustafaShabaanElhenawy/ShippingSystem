using System;
using System.Net;
using System.Web.Http;
using TrainingShippingSystem.BLL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.Controllers
{
    [RoutePrefix("api/Containers")]
    public class ContainersController : ApiController
    {
        private readonly ContainerBLL containerBLL =
            new ContainerBLL();


        // GET: api/Containers
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetContainers()
        {
            try
            {
                var containers =
                    containerBLL.GetContainers();

                return Ok(containers);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


        // GET: api/Containers/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetContainerByID(int id)
        {
            try
            {
                var container =
                    containerBLL.GetContainerByID(id);

                if (container == null)
                    return NotFound();

                return Ok(container);
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


        // POST: api/Containers
        [HttpPost]
        [Route("")]
        public IHttpActionResult InsertContainer(
            [FromBody] Container container)
        {
            try
            {
                int newID =
                    containerBLL.InsertContainer(container);

                container.ID = newID;

                return Content(
                    HttpStatusCode.Created,
                    container);
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


        // PUT: api/Containers/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateContainer(
            int id,
            [FromBody] Container container)
        {
            try
            {
                if (container == null)
                    return BadRequest(
                        "Container data is required.");

                container.ID = id;

                bool updated =
                    containerBLL.UpdateContainer(container);

                if (!updated)
                    return NotFound();

                return Ok(container);
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


        // DELETE: api/Containers/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteContainer(int id)
        {
            try
            {
                bool deleted =
                    containerBLL.DeleteContainer(id);

                if (!deleted)
                    return NotFound();

                return Ok(new
                {
                    message =
                        "Container deleted successfully."
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


        // GET: api/Containers/search?search=ABCU
        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchContainers(
            string search)
        {
            try
            {
                var containers =
                    containerBLL.SearchContainers(search);

                return Ok(containers);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}