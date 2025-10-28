using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GymManagementBLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] AllowedExtentions = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly long MaxSize = 5 * 1024 * 1024; // 5 MB 
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AttachmentService(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
        }
        public string? Upload(string FolderName, IFormFile File)
        {
            try
            {
                if (FolderName is null || File is null || File.Length == 0) return null;
                if (File.Length > MaxSize) return null;
                var FileExtention = Path.GetExtension(File.FileName).ToLower();
                if (!AllowedExtentions.Contains(FileExtention)) return null;

                var FolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", FolderName);
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                var FileName = Guid.NewGuid().ToString() + FileExtention;

                var FilePath = Path.Combine(FolderPath, FileName);
                using var FileStream = new FileStream(FilePath, FileMode.Create);
                File.CopyTo(FileStream);
                return FileName;

            }catch(Exception ex)
            {
                Console.WriteLine($"Failed to Upload File to Folder {FolderName} with Exception : {ex}");
                return null;
            }
        }


        public bool Delete(string FolderName, string FileName)
        {
            try
            {
                if (string.IsNullOrEmpty(FolderName) || string.IsNullOrEmpty(FileName)) return false;
                var FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", FolderName, FileName);
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Delete File {FileName} from Folder {FolderName} with Exception : {ex}");
                return false;
            }
        }
    }
}
