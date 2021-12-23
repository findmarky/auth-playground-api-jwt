using Auth.Playground.API.Models;

namespace Auth.Playground.API.Services
{
    public interface IAuthenticationService
    {
        AuthenticationResultModel Authenticate(string userName, string password);
    }
}