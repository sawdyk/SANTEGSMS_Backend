using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class LoginReqModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }

    public class StudentLoginReqModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }

}
