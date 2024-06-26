using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Service.Data;
using Shared.Features;
using Shared.Infrastructures.Extensions;
using Stl.Fusion.EntityFramework;
using System.IdentityModel.Tokens.Jwt;

namespace Service.Features
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _hostUrl;
        private readonly DbHub<AppDbContext> dbHub;
        private readonly string MEDIA = "media";
        private readonly string ROOTPATH;
        //private readonly string local = "https://localhost:7267";
        public FileService(IWebHostEnvironment environment, DbHub<AppDbContext> dbHub)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _hostUrl = "https://api.ebazar.admin-eltop.uz";
            this.dbHub = dbHub;
            ROOTPATH = environment.WebRootPath;
        }

        public async Task<bool> DeleteMediaAsync(string subpath)
        {
            
            // Construct the full path to the image file
            var imagePath = Path.Combine(ROOTPATH, "media", subpath);

            // Check if the file exists
            if (File.Exists(imagePath))
            {
                // Delete the file
                await Task.Run(() => File.Delete(imagePath));
                return true;
            }

            return false;
        }

        public async Task<string> UploadMediaAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            string newImageName = MediaHelper.MakeMediaName(file.FileName);
            string subpath = Path.Combine(MEDIA, newImageName); // Changed to save directly under "media"
            string path = Path.Combine(ROOTPATH, subpath);

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string urlPath = subpath.Replace("\\", "/");
            return $"{_hostUrl}/" + urlPath;
        }


        public async Task<bool> DeleteOneImage(string imageFileName, string token)
        {
            try
            {
                var valid = ValidateToken(token);
                if (!IsAdminUser(valid))
                {
                    throw new CustomException("You have not permission");
                }
                var imagePath = Path.Combine(ROOTPATH, "media", imageFileName);

                // Check if the file exists
                if (File.Exists(imagePath))
                {
                    // Delete the file
                    await Task.Run(() => File.Delete(imagePath));
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image: {ex.Message}");
                return false;
            }
        }

        #region Token
        private bool IsAdminUser(string phoneNumber)
        {
            using var dbContext = dbHub.CreateDbContext();
            var user = dbContext.UsersEntities.FirstOrDefault(x => x.PhoneNumber == phoneNumber && x.Role == "Admin");
            return user != null;
        }
        private string ValidateToken(string token)
        {
            var jwtEncodedString = token.Substring(7);

            var secondToken = new JwtSecurityToken(jwtEncodedString);
            var json = secondToken.Payload.Values.FirstOrDefault();
            if (json == null)
                throw new CustomException("Payload is null");
            else
            {
                return json?.ToString() ?? string.Empty;
            }
        }


        #endregion
    }
}
