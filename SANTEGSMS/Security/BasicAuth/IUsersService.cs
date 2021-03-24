using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Security.BasicAuth
{
    public interface IUsersService
    {
        Task<Users> Authenticate(string username, string password);
        Task<IEnumerable<Users>> GetAll();
    }
}
