using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Services.Email
{
    public class EmailTemplate
    {
        public string EmailHtmlTemplate(string code)
        {
            string template = "<h4>Welcome to SOFTLEARN use the code below to activate your account</h4>" +
                "<table border='1px'>" +
                "<thead>" +
                "<tr>" +
                "<th>Code</th>" +
                "</tr>" +
                "<tbody>" +
                "<tr>" +
                "<td>" + code + "</td>" +
                "</tr>" +
                "</tbody>" +
                "</table>";

            return template;
        }
    }
}
