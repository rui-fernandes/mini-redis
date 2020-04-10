namespace MiniRedis.Controllers
{
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using MiniRedis.Application;

    [ApiController]
    [Route("[controller]")]
    public class RedisController : Controller
    {
        private readonly ICacheService service;
        private readonly Regex validator;

        public RedisController(ICacheService service)
        {
            this.service = service;
            this.validator = new Regex("[a-zA-Z0-9-_]");
        }

        [HttpGet("{key}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get(string key)
        {
            if (!this.validator.Match(key).Success)
            {
                return this.BadRequest("Key can only be the characters from the set [a-zA-Z0-9-_].");
            }

            var item = this.service.Get(key);

            if (item == null)
            {
                return this.NotFound();
            }

            return this.Ok(item);
        }

        [HttpGet("count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Count()
        {
            var size = this.service.GetSize();

            return this.Ok(size);
        }

        [HttpPost("{key}/increment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Increment(string key)
        {
            if (!this.validator.Match(key).Success)
            {
                return this.BadRequest("Key can only be the characters from the set [a-zA-Z0-9-_].");
            }

            this.service.Increment(key);

            return this.Ok();
        }

        [HttpDelete("{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(string key)
        {
            if (!this.validator.Match(key).Success)
            {
                return this.BadRequest("Key can only be the characters from the set [a-zA-Z0-9-_].");
            }

            _ = this.service.Delete(key);

            return this.Ok();
        }

        [HttpPost("{key}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Set(
            string key,
            [FromQuery] int? seconds,
            [FromBody] string value)
        {
            if (!this.validator.Match(key).Success)
            {
                return this.BadRequest("Key can only be the characters from the set [a-zA-Z0-9-_].");
            }

            if (!this.validator.Match(value).Success)
            {
                return this.BadRequest("Value can only be the characters from the set [a-zA-Z0-9-_].");
            }

            this.service.Set(key, seconds, value);

            return this.CreatedAtAction(nameof(Get), new { key = key }, null);
        }

        [HttpPost("{key}/{score}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SortedSet(
            string key,
            int score,
            [FromBody] string member)
        {
            if (!this.validator.Match(key).Success)
            {
                return this.BadRequest("Key can only be the characters from the set [a-zA-Z0-9-_].");
            }

            if (!this.validator.Match(member).Success)
            {
                return this.BadRequest("Value can only be the characters from the set [a-zA-Z0-9-_].");
            }

            this.service.SortedSet(key, score, member);

            return this.CreatedAtAction(nameof(Get), new { key = key }, null);
        }

        [HttpGet("sortedCardinality/{key}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SortedCardinality(string key)
        {
            if (!this.validator.Match(key).Success)
            {
                return this.BadRequest("Key can only be the characters from the set [a-zA-Z0-9-_].");
            }

            var size = this.service.SortedCardinality(key);

            return this.Ok(size);
        }

        [HttpGet("rankMember/{key}/{member}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SortedCardinality(string key, string member)
        {
            if (!this.validator.Match(key).Success)
            {
                return this.BadRequest("Key can only be the characters from the set [a-zA-Z0-9-_].");
            }

            var size = this.service.RankMember(key, member);

            return this.Ok(size);
        }
    }
}
