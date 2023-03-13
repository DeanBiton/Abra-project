using Microsoft.AspNetCore.Mvc;
using RestAPI.Services;
using RestAPI.DTO;
namespace RestAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RandomController : ControllerBase
{
    private readonly RandomService _randomService;

    public RandomController()
    {
        _randomService = new RandomService();
    }

    [HttpGet("user/{gender}")]
    async public Task<IActionResult> GetUser(string gender)
    {
        try
        {
            if(!gender.Equals("male") && !gender.Equals("female"))
                throw new ArgumentException("Invalid gender");

            var user = await _randomService.GetUsersData(gender);
            return Ok(user);
        }
        catch(Exception ex)
        {
            return BadRequest(new ErrorDTO(ex.Message));
        }
    }

    [HttpGet("popularCountry")]
    async public Task<IActionResult> GetMostPopularCountry()
    {
        try
        {
            string country = await _randomService.GetMostPopularCountry();
            return Ok(country);
        }
        catch(Exception ex)
        {
            return StatusCode(500, new ErrorDTO(ex.Message));
        }
    }

    [HttpGet("mail")]
    async public Task<IActionResult> GetListOfMails()
    {
        try
        {
            List<string> mails = await _randomService.GetListOfMails();
            return Ok(mails);
        }
        catch(Exception ex)
        {
            return StatusCode(500, new ErrorDTO(ex.Message));
        }
    }

    [HttpGet("oldest")]
    async public Task<IActionResult> GetOldestUser()
    {
        try
        {
            DTOBase user = await _randomService.GetTheOldestUser();
            return Ok(user);
        }
        catch(Exception ex)
        {
            return StatusCode(500, new ErrorDTO(ex.Message));
        }
    }
}