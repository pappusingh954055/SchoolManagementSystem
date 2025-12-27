using Microsoft.AspNetCore.Http;

namespace Student.API.Helpers;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;

    private static readonly string[] AllowedExtensions =
        [".jpg", ".jpeg", ".png"];

    public FileStorageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveStudentPhotoAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new InvalidOperationException("Invalid file");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            throw new InvalidOperationException("Only JPG and PNG files are allowed");

        var uploadPath = Path.Combine(
            _env.WebRootPath,
            "uploads",
            "students");

        Directory.CreateDirectory(uploadPath);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(uploadPath, fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/students/{fileName}";
    }
}
