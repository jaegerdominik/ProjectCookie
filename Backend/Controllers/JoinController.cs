using Microsoft.AspNetCore.Mvc;
using ProjectCookie.DAL.Entities;
using ProjectCookie.Services.BaseInterfaces;
using ProjectCookie.Services.MQTT;

namespace ProjectCookie.Controllers;

[ApiController]
[Route("api/join")]
public class JoinController : ControllerBase
{
    private readonly IGlobalService _globalService;
    private readonly MqttDriver _driver;

    public JoinController(IGlobalService globalService, MqttDriver driver)
    {
        _globalService = globalService;
        _driver = driver;
        
        _driver.StartAsync(new CancellationToken());
    }
    
            
    [HttpGet("users")]
    public async Task<List<User>> GetAll()
    {
        return await _globalService.UserService.Get();
    }

    [HttpGet("{id}")]
    public async Task<User?> Get(string id)
    {
        int idAsInt = int.Parse(id);
        return await _globalService.UserService.Get(idAsInt);
    }

    [HttpPut("{username}")]
    public async Task<IActionResult> CreateUser(string username)
    {
        await _driver.Publish("adswe_mqtt_cookie_user", username);
        return Ok();
    }
}
