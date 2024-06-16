using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCookie.DAL.Entities;
using ProjectCookie.Services.BaseInterfaces;

namespace ProjectCookie.Controllers;

[ApiController]
[Route("api/cookie")]
public class CookieController : ControllerBase
{
    private readonly IGlobalService _globalService;
    
    public CookieController(IGlobalService globalService)
    {
        _globalService = globalService;
    }
    
    
    [HttpGet]
    public async Task<List<Score>> GetAll()
    {
        return await _globalService.ScoreService.Get();
    }
}

