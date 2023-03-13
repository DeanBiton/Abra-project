using Microsoft.AspNetCore.Mvc;
using RestAPI.Services;
using RestAPI.DTO;
using RestAPI.Models;

namespace RestAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController()
    {
        _userService = new UserService();
    }

    [HttpPost("")]
    public IActionResult CreateNewUser(CreateUser request)
    {
        try
        {
            var user = _userService.CreateNewUser(request);
            return Ok(user);
        }
        catch(Exception ex)
        {
            return BadRequest(new ErrorDTO(ex.Message));
        }
    }

    [HttpGet("")]
    public IActionResult GetNewUser()
    {
        try
        {
            var user = _userService.GetNewUser();
            return Ok(user);
        }
        catch(Exception ex)
        {
            return BadRequest(new ErrorDTO(ex.Message));
        }
    }

    [HttpPut()]
    public IActionResult UpdateUserData(CreateUser request)
    {
        try
        {
            var user = _userService.UpdateUserData(request);
            return Ok(user);
        }
        catch(Exception ex)
        {
            return BadRequest(new ErrorDTO(ex.Message));
        }
    }
}