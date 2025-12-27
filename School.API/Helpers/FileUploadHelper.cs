namespace School.API.Helpers
{
    public static class FileUploadHelper
    {
        public static async Task<string?> SaveSchoolPhotoAsync(
            IFormFile? file,
            IWebHostEnvironment env)
        {
            if (file == null || file.Length == 0)
                return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Only JPG and PNG files are allowed");

            var uploadFolder = Path.Combine(env.WebRootPath, "uploads", "schools");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadFolder, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/schools/{fileName}";
        }
    }

}
