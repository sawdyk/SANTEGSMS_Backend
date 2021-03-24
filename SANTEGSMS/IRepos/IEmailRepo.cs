using SANTEGSMS.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IEmailRepo
    {
        void SendEmail(EmailMessage message);
    }
}
