using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Helpers
{
    public class ServerPath
    {
        private readonly IWebHostEnvironment _env;

        public ServerPath(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string ServerFolderPath(long appId, string folderName)
        {
            string path = string.Empty;

            //the root path of the server to upload files
            //string serverRootPath = Directory.GetCurrentDirectory();
            string serverRootPath = @"C:\inetpub\wwwroot\";

            path = serverRootPath + @"SantegFilesRepository\SchoolDocuments\" + folderName;

            //string path = @"C:\inetpub\wwwroot\SoftlearnMedia\" + folderName;

            return path;
        }

        public static string ServerMainFolderName()
        {
            //the main folder to save all application files 
            //(this should be modified only if the folder name on the server was changed/edited)
            const string mainFolderName = "SantegFilesRepository";

            return mainFolderName;
        }

        public string ServerBaseURL()
        {
            string baseUrl = "http://161.97.77.250:8080";

            return baseUrl;
        }
    }
}
