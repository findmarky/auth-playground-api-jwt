using Microsoft.AspNetCore.Mvc;
using Auth.Playground.API.Models;
using Auth.Playground.API.Services;

namespace Auth.Playground.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
            {
                return BadRequest();
            }

            AuthenticationResultModel authenticateResult = _authenticationService.Authenticate(loginModel.UserName, loginModel.Password);
            if (authenticateResult.IsAuthenticated)
            {
                return Ok(new
                {
                    authenticateResult.Token
                });
            }

            return Unauthorized();
        }
    }
}
