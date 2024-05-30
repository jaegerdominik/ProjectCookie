using System.Security.Claims;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCookie._src.services;
using ProjectCookie._src.utils.Logging;

namespace ProjectCookie.Controllers;


    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : Entity
    {
        protected Service<T> _Service = null;
        //protected Serilog.ILogger log = Logger.ContextLog<BaseController<T>>();
        protected String UserEmail = null;
        protected ClaimsPrincipal ClaimsPrincipal = null;
        protected Serilog.ILogger log = null;
        

        public BaseController(Service<T> service, IHttpContextAccessor accessor, ICookieLogger logger)
        {
            //log = logger.ContextLog<BaseController<T>>();

            this._Service = service;

            Task modelState = this._Service.SetModelState(this.ModelState);
            modelState.Wait();

            ClaimsPrincipal = accessor.HttpContext.User;

            var identity = (ClaimsIdentity)accessor.HttpContext.User.Identity;

            IEnumerable<Claim> claims = identity.Claims;
            
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (email != null)
            {
                UserEmail = email.Value;

                Task user = this._Service.Load(UserEmail);
                user.Wait();
            }
        }
        
        
        //Get All
        [HttpGet("All")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        
        public async Task<List<T>> GetAll()
        {
            return await _Service.Get();
        }

        //Get id
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<T> Get(String id)

        {
            return await _Service.Get(id);
        }


    }
