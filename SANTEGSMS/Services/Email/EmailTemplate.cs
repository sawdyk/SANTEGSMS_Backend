using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Services.Email
{
    public class EmailTemplate
    {
        private readonly IWebHostEnvironment _env;

        public EmailTemplate(IWebHostEnvironment env)
        {
            this._env = env;
        }
        public string EmailAccountCreation(string codeGenerated)
        {
            // string body;  
            var webRoot = _env.WebRootPath; //get wwwroot Folder  

            //Get TemplateFile located at wwwroot/EmailTemplate/emailAccountCreation.html  
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "emailAccountCreation.html";

            string body = string.Empty;
            using (StreamReader SourceReader = File.OpenText(pathToFile))
            {
                //builder.HtmlBody = SourceReader.ReadToEnd();
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{code}", codeGenerated);
            body = body.Replace("{currentYear}", DateTime.Now.ToString("MM/dd/yyy"));

            return body;
        }


        public string EmailAccountActivation()
        {
            // string body;  
            var webRoot = _env.WebRootPath; //get wwwroot Folder  

            //Get TemplateFile located at wwwroot/EmailTemplate/emailAccountCreation.html  
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "emailAccountActivation.html";

            string body = string.Empty;
            using (StreamReader SourceReader = File.OpenText(pathToFile))
            {
                //builder.HtmlBody = SourceReader.ReadToEnd();
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{currentYear}", DateTime.Now.ToString("MM/dd/yyy"));

            return body;
        }

        public string EmailSchoolCreationNotify(string firstName, string lastName, string emailAddress, string schoolName, string schoolCode, string schoolType, string dateCreated)
        {
            // string body;  
            var webRoot = _env.WebRootPath; //get wwwroot Folder  

            //Get TemplateFile located at wwwroot/EmailTemplate/emailAccountCreation.html  
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "emailSuperAdminSchoolCreationNotify.html";

            string body = string.Empty;
            using (StreamReader SourceReader = File.OpenText(pathToFile))
            {
                //builder.HtmlBody = SourceReader.ReadToEnd();
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{FirstName}", firstName);
            body = body.Replace("{LastName}", lastName);
            body = body.Replace("{EmailAddress}", emailAddress);
            body = body.Replace("{SchoolName}", schoolName);
            body = body.Replace("{SchoolCode}", schoolCode);
            body = body.Replace("{SchoolType}", schoolType);
            body = body.Replace("{DateCreated}", dateCreated);
            body = body.Replace("{currentYear}", DateTime.Now.ToString("MM/dd/yyy"));

            return body;
        }

        public string EmailSchoolCreationApproved(string statusMessage, string notification)
        {
            // string body;  
            var webRoot = _env.WebRootPath; //get wwwroot Folder  

            //Get TemplateFile located at wwwroot/EmailTemplate/emailAccountCreation.html  
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "emailSuperAdminApprove.html";

            string body = string.Empty;
            using (StreamReader SourceReader = File.OpenText(pathToFile))
            {
                //builder.HtmlBody = SourceReader.ReadToEnd();
                body = SourceReader.ReadToEnd();
            }

            body = body.Replace("{StatusMessage}", statusMessage);
            body = body.Replace("{Notification}", notification);
            body = body.Replace("{currentYear}", DateTime.Now.ToString("MM/dd/yyy"));

            return body;
        }


        public string EmailForgotPassword(string password)
        {
            // string body;  
            var webRoot = _env.WebRootPath; //get wwwroot Folder  

            //Get TemplateFile located at wwwroot/EmailTemplate/emailAccountCreation.html  
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "emailForgotPassword.html";

            string body = string.Empty;
            using (StreamReader SourceReader = File.OpenText(pathToFile))
            {
                //builder.HtmlBody = SourceReader.ReadToEnd();
                body = SourceReader.ReadToEnd();
            }

            body = body.Replace("{Password}", password);
            body = body.Replace("{currentYear}", DateTime.Now.ToString("MM/dd/yyy"));

            return body;
        }

        public string EmailSchoolUsersApproved(string statusMessage, string notification, string firstName, string lastName)
        {
            // string body;  
            var webRoot = _env.WebRootPath; //get wwwroot Folder  

            //Get TemplateFile located at wwwroot/EmailTemplate/emailAccountCreation.html  
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "emailSchoolUsersApprove.html";

            string body = string.Empty;
            using (StreamReader SourceReader = File.OpenText(pathToFile))
            {
                //builder.HtmlBody = SourceReader.ReadToEnd();
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{FirstName}", firstName);
            body = body.Replace("{LastName}", lastName);
            body = body.Replace("{StatusMessage}", statusMessage);
            body = body.Replace("{Notification}", notification);
            body = body.Replace("{currentYear}", DateTime.Now.ToString("MM/dd/yyy"));

            return body;
        }
    }
}
