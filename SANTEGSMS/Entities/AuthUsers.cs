using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class AuthUsers
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Salt { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
