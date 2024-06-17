using Microsoft.AspNetCore.Mvc;
using ProjectCookie.DAL.Entities;
using ProjectCookie.Services.BaseInterfaces;
using ProjectCookie.Services.MQTT;

namespace ProjectCookie.Controllers;

[ApiController]
[Route("api/cookie")]
public class CookieController : ControllerBase
{
    private readonly IGlobalService _globalService;
    private readonly MqttDriver _driver;
    
    public CookieController(IGlobalService globalService, MqttDriver driver)
    {
        _globalService = globalService;
        _driver = driver;

        _driver.StartAsync(new CancellationToken());
    }
    
    
    [HttpGet("scores")]
    public async Task<IActionResult> GetScores()
    {
        List<Score> scores = await _globalService.ScoreService.Get();
        return Ok(scores);
    }
    
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        List<User> users = await _globalService.UserService.Get();
        return Ok(users);
    }
    
    [HttpPut("publish/{specialScoreString}")]
    public async Task<IActionResult> CreateUser(string specialScoreString)
    {
        await _driver.Publish("adswe_mqtt_cookie_score", specialScoreString);
        return Ok();
    }
}

