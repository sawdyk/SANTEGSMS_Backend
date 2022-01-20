using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Security.BasicAuth
{
    public class UsersService : IUsersService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<Users> _users = new List<Users>
        {
            new Users { Id = 1, FirstName = "Test", LastName = "User", Username = "santegLive", Password = "santegLive321#$" }
        };

        public async Task<Users> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            return user;
        }

        public async Task<IEnumerable<Users>> GetAll()
        {
            return await Task.Run(() => _users);
        }
    }
}
