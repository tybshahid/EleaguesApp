using System;
using Microsoft.AspNetCore.Http;

namespace EleaguesApp.API.Helpers
{
    public class ImageHelper
    {
        public bool Validate(IFormFile upload)
        {
            try
            {
                string fileName = upload.FileName; // getting File Name
                string fileContentType = upload.ContentType; // getting ContentType
                byte[] tempFileBytes = new byte[upload.Length]; // getting filebytes
                var data = upload.OpenReadStream().Read(tempFileBytes, 0, Convert.ToInt32(upload.Length));
                var types = FileUploadCheck.FileType.Image;  // Setting Image type
                var result = FileUploadCheck.isValidFile(tempFileBytes, types, fileContentType); // Validate Header

                if (result == true)
                {
                    int FileLength = 1024 * 1024 * 6; //FileLength 6 MB 
                    if (upload.Length > FileLength)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}