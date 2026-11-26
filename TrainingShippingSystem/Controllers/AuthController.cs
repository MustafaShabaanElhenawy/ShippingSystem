using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Web.Http;
using TrainingShippingSystem.BLL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.Controllers
{
    [RoutePrefix("api/Auth")]
    public class AuthController : ApiController
    {
        private readonly AuthBLL authBLL = new AuthBLL();
        private readonly AuditLogBLL auditLogBLL = new AuditLogBLL();

        // POST: api/Auth/Register
        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public IHttpActionResult Register(RegisterRequest request)
        {
            try
            {
                User user = authBLL.Register(request);

                try
                {
                    auditLogBLL.LogAction(new AuditLog
                    {
                        UserName = user.Username,
                        ModuleName = "Users",
                        ActionType = "CREATE",
                        RecordID = user.ID,
                        Description = "User registered: " + user.Username
                    }, new List<AuditLogDetail>());
                }
                catch (Exception)
                {
                }

                return Content(
                    HttpStatusCode.Created,
                    new
                    {
                        message = "Account created successfully. Please sign in.",
                        username = user.Username
                    });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return Content(
                        HttpStatusCode.Conflict,
                        new { message = "Username already exists." });
                }

                return InternalServerError();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // POST: api/Auth/Login
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IHttpActionResult Login(LoginRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Login data is required.");

                string token;
                User user = authBLL.Login(request.Username, request.Password, out token);

                if (user == null)
                {
                    return Content(
                        HttpStatusCode.Unauthorized,
                        new { message = "Invalid username or password." });
                }

                try
                {
                    auditLogBLL.LogAction(new AuditLog
                    {
                        UserName = user.Username,
                        ModuleName = "Auth",
                        ActionType = "LOGIN",
                        RecordID = user.ID,
                        Description = "User logged in: " + user.Username
                    }, new List<AuditLogDetail>());
                }
                catch (Exception)
                {
                }

                return Ok(new
                {
                    token = token,
                    username = user.Username,
                    fullName = user.FullName,
                    role = user.Role
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

        // POST: api/Auth/Logout
        [HttpPost]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            var auth = Request.Headers.Authorization;

            if (auth != null && !string.IsNullOrEmpty(auth.Parameter))
                AuthBLL.Logout(auth.Parameter);

            return Ok();
        }
    }
}