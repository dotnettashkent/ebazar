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
        private readonly string VIDEOS = "videos";
        private readonly string ROOTPATH;
        public FileService(IWebHostEnvironment environment, DbHub<AppDbContext> dbHub)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _hostUrl = "https://api.ebazar.admin-eltop.uz";
            this.dbHub = dbHub;
            ROOTPATH = environment.WebRootPath;
        }

        public async Task<bool> DeleteVideoAsync(string subpath)
        {
            string path = Path.Combine(ROOTPATH, subpath);
            if (File.Exists(path))
            {
                await Task.Run(() =>
                {
                    File.Delete(path);
                });
                return true;
            }
            return false;
        }

        public async Task<string> UplaodVideoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            string newImageName = MediaHelper.MakeVideoName(file.FileName);
            string subpath = Path.Combine(MEDIA, VIDEOS, newImageName);
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

            return $"{_hostUrl}/"+subpath;
        }
        public async Task<bool> DeleteOneImage(string imageFileName, string token)
        {
            try
            {
                var valid = ValidateToken(token);
                if (!IsAdminUser(valid))
                {
                    throw new CustomException("User does not have permission to create a product.");
                }
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploads");
                var filePath = Path.Combine(uploadsFolder, imageFileName);

                if (File.Exists(filePath))
                {
                    // Delete the file
                    File.Delete(filePath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log any exceptions
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
