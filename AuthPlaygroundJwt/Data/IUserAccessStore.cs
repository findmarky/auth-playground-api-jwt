namespace Auth.Playground.API.Data
{
    public interface IUserAccessStore
    {
        bool Authenticate(string userName, string password);
    }
}