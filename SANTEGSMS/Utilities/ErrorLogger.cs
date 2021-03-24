using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Utilities
{
    public class ErrorLogger
    {
        public ErrorLog logError(Exception exMessage)
        {
            var error = new ErrorLog
            {
                ErrorMessage = exMessage.Message,
                ErrorSource = exMessage.Source,
                ErrorStackTrace = exMessage.StackTrace,
                ErrorDate = DateTime.Now
            };

            return error;
        }
    }
}
