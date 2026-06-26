using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.API.Controllers;

[ApiController]
[Route("api/redis")]
public class RedisTestController : ControllerBase
{
    private readonly IRedisCacheService _redis;

    public RedisTestController(
        IRedisCacheService redis)
    {
        _redis = redis;
    }

    [HttpGet]
    public async Task<IActionResult> Test()
    {
        await _redis.SetAsync(
            "test",
            "Hello Redis",
            TimeSpan.FromMinutes(5));

        var value =
            await _redis.GetAsync<string>("test");

        return Ok(value);
    }
}