using System.ComponentModel.DataAnnotations;
using dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using services.Authentication;
using services.Response;
using utils.interfaces;


namespace api.Controllers;


public class UserController : BaseController<User>
{

    private IGlobalService service;
    
    public UserController(IGlobalService service, IHttpContextAccessor accessor, IAquariumLogger logger) : base(service.UserService, accessor, logger)
    {
        this.service = service;

    }
    
    
    // Login
    [HttpPost("UserLogin")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationInformation))]
    
    public async Task<AuthenticationInformation> UserLogin([Required][FromBody]User user)
    {
        AuthenticationInformation response = await this.service.UserService.Login(user.Email, user.Password);
        return response;
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

