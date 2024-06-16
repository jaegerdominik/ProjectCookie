using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCookie.DAL.Entities;
using ProjectCookie.Services.BaseInterfaces;
using ProjectCookie.Services.Response;

namespace ProjectCookie.Controllers;

[ApiController]
[Route("api/join")]
public class JoinController : ControllerBase
{
    private readonly IGlobalService _globalService;
    
    public JoinController(IGlobalService globalService)
    {
        _globalService = globalService;
    }
    
            
    [HttpGet("users")]
    public async Task<List<User>> GetAll()
    {
        return await _globalService.UserService.Get();
    }

    [HttpGet("{id}")]
    public async Task<User> Get(string id)
    {
        int idAsInt = int.Parse(id);
        return await _globalService.UserService.Get(idAsInt);
    }
    
    // Update
    [HttpPost("UserUpdate/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponseModel<User>))]
    public async Task<ActionResult<ItemResponseModel<User>>> UserUpdate([Required][FromBody]User user, string id)
    {
        int idAsInt = int.Parse(id);
        ActionResult <ItemResponseModel<User>> response = await _globalService.UserService.Update(idAsInt, user);
        return response;
    }
}

