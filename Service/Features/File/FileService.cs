using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Shared.Features;

namespace Service.Features
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _hostUrl;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _hostUrl = "http://188.166.57.12:8080";
        }

        public async Task<Tuple<int, string>> SaveImage(IFormFile imageFile)
        {
            try
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploads");

                // Ensure the "Uploads" folder exists and create it if not
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Check allowed extensions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                if (Array.IndexOf(allowedExtensions, ext.ToLower()) == -1)
                {
                    string msg = $"Only {string.Join(",", allowedExtensions)} extensions are allowed";
                    return new Tuple<int, string>(0, msg);
                }

                string uniqueString = Guid.NewGuid().ToString();
                // Generate unique filename
                var newFileName = uniqueString + ext;

                var filePath = Path.Combine(uploadsFolder, newFileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                var imageUrl = $"{_hostUrl}/uploads/{newFileName}"; // Construct full image URL

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
                // Log any exceptions
                Console.WriteLine($"Error deleting image: {ex.Message}");
                return false;
            }
        }
    }
}
