using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCookie._src.dal;
using ProjectCookie._src.services.interfaces;
using ProjectCookie._src.utils.Logging;
using Services;
using Services.Response.Basis;

namespace ProjectCookie.Controllers;


public class UserController : BaseController<User>
{

    private IGlobalService service;
    
    public UserController(IGlobalService service, IHttpContextAccessor accessor, ICookieLogger logger) : base(service.UserService, accessor, logger)
    {
        this.service = service;

    }
    
    // Register
    [HttpPost("UserRegister")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel<User>))]
    
    public async Task<ActionResult<ItemResponseModel<User>>>  UserRegister([Required][FromBody]User user)
    {
        ActionResult <ItemResponseModel<User>> response = await this.service.UserService.Create(user);
        return response;
    }
    
    
    // Update
    [HttpPost("UserUpdate/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel<User>))]
    
    public async Task<ActionResult<ItemResponseModel<User>>>  UserUpdate([Required][FromBody]User user, string id)
    {
        ActionResult <ItemResponseModel<User>> response = await this.service.UserService.Update(id, user);
        return response;
    }


    
}

