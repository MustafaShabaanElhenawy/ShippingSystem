using System;
using System.Data.SqlClient;
using System.Web.Http;
using TrainingShippingSystem.BLL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.Controllers
{
    [RoutePrefix("api/Clients")]
    public class ClientsController : ApiController
    {
        private readonly ClientBLL clientBLL;

        public ClientsController()
        {
            clientBLL = new ClientBLL();
        }


        // GET: api/Clients
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetClients()
        {
            try
            {
                var clients =
                    clientBLL.GetAllClients();

                return Ok(clients);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


        // GET: api/Clients/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetByID(int id)
        {
            try
            {
                var client =
                    clientBLL.GetClientByID(id);

                if (client == null)
                {
                    return NotFound();
                }

                return Ok(client);
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


        // GET: api/Clients/search?search=Ahmed
        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchClients(string search)
        {
            try
            {
                var clients =
                    clientBLL.SearchClients(search);

                return Ok(clients);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


        // POST: api/Clients
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post(Client client)
        {
            try
            {
                int newID =
                    clientBLL.AddClient(client);

                var createdClient =
                    clientBLL.GetClientByID(newID);

                return Created(
                    Request.RequestUri +
                    "/" +
                    newID,
                    createdClient);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 ||
                    ex.Number == 2627)
                {
                    return BadRequest(
                        "Email already exists.");
                }

                return InternalServerError();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


        // PUT: api/Clients/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(
            int id,
            Client client)
        {
            try
            {
                if (client == null)
                {
                    return BadRequest(
                        "Client data is required.");
                }

                client.ID = id;

                var existingClient =
                    clientBLL.GetClientByID(id);

                if (existingClient == null)
                {
                    return NotFound();
                }

                bool updated =
                    clientBLL.UpdateClient(client);

                if (!updated)
                {
                    return NotFound();
                }

                var updatedClient =
                    clientBLL.GetClientByID(id);

                return Ok(updatedClient);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 ||
                    ex.Number == 2627)
                {
                    return BadRequest(
                        "Email already exists.");
                }

                return InternalServerError();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


        // DELETE: api/Clients/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var existingClient =
                    clientBLL.GetClientByID(id);

                if (existingClient == null)
                {
                    return NotFound();
                }

                bool deleted =
                    clientBLL.DeleteClient(id);

                if (!deleted)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        message =
                            "Client deleted successfully."
                    });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                // Foreign Key violation
                if (ex.Number == 547)
                {
                    return BadRequest(
                        "Cannot delete this client because it is linked to existing bills.");
                }

                return InternalServerError();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}