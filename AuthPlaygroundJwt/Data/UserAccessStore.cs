using System.Collections.Generic;

namespace Auth.Playground.API.Data
{
    public class UserAccessStore : IUserAccessStore
    {
        private readonly Dictionary<string, string> _testUserNames = new Dictionary<string, string>
        {
            {"Bob", "password123" }, { "Sue", "password456" }, {"Jim", "password789"}
        };

        public bool Authenticate(string userName, string password)
        {
            return _testUserNames.ContainsKey(userName) && _testUserNames[userName] == password;
        }
    }
}
