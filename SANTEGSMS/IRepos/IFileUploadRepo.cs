using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IFileUploadRepo
    {
        Task<FileUploadRespModel> uploadFilesAsync(FileUploadReqModel obj);
        Task<GenericRespModel> getAllSupportedFileExtensionsAsync();
        Task<GenericRespModel> getAllAppTypesAsync();
        Task<GenericRespModel> getAppTypesByIdAsync(long appId);
        Task<GenericRespModel> getAllFolderTypesByAppIdAsync(long appId);
        Task<GenericRespModel> getFolderTypeByIdAsync(long folderId);

        //Reusables
        string getFileTypeAsync(string fileExtension);
        IList<string> allFileExtensionsAsync();
    }
}
