using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public FileService(IWebHostEnvironment environment, DbHub<AppDbContext> dbHub, IConfiguration configuration)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _hostUrl = "http://178.62.226.232";
            this.dbHub = dbHub;
            _configuration = configuration;
        }

        public async Task<Tuple<int, string>> SaveImage(IFormFile imageFile)
        {
            try
            {
                // Use the web root path
                var basePath = _environment.WebRootPath;
                var uploadsFolder = Path.Combine(basePath, "Uploads");

                // Ensure the "Uploads" folder exists and create it if not
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Check allowed extensions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext.ToLower()))
                {
                    string msg = $"Only {string.Join(",", allowedExtensions)} extensions are allowed";
                    return new Tuple<int, string>(0, msg);
                }

                // Generate unique filename
                string uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + ext;
                var filePath = Path.Combine(uploadsFolder, newFileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                var imageUrl = $"{_hostUrl}/Uploads/{newFileName}";

                return new Tuple<int, string>(1, imageUrl);
            }
            catch (Exception ex)
            {
                // Log any exceptions
                Console.WriteLine($"Error saving image: {ex.Message}");
                return new Tuple<int, string>(0, "Error has occurred");
            }
        }



        public async Task<bool> DeleteImage(string imageFileName)
        {
            try
            {
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
                return false;
            }
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
