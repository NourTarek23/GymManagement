using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Attachment;

public class AttachmentService : IAttachmentService
{
    private readonly long _maxFileSize = 5 * 1024 * 1024;
    private readonly string[] _extensions = [".jpg", ".jpeg", ".png"];
    private readonly IWebHostEnvironment _env;

    public AttachmentService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public bool Delete(string folderName, string fileName)
    {
        try
        {
            var filePath = Path.Combine(_env.ContentRootPath, folderName, fileName);
            if (!File.Exists(filePath)) return false;
          
            File.Delete(filePath);
            return true;

        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public (Stream stream, string contentType)? GetFile(string folderName, string fileName)
    {
        if (string.IsNullOrWhiteSpace(folderName) || string.IsNullOrWhiteSpace(fileName)) return null;
        var filePath = Path.Combine(_env.ContentRootPath, folderName, fileName);
        if (!File.Exists(filePath)) return null;

        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        var extension = Path.GetExtension(filePath);

        var contentType = extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _=> "application/octet-stream"
        };

        return (stream, contentType);
    }

    public async Task<string?> UploadAsync(Stream fileStream, string folderName, string fileName, CancellationToken ct)
    {
        if (fileStream is null || !fileStream.CanRead) return null;
        if (fileStream.Length == 0) return null;
        if (fileStream.Length > _maxFileSize) return null;

        var fileExt = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(fileExt) || !_extensions.Contains(fileExt)) return null;

        var uploadsFoalder = Path.Combine(_env.ContentRootPath, folderName);
        // create directory in specific path unless it exists
        Directory.CreateDirectory(uploadsFoalder);

        var storedFileName = $"{Guid.NewGuid()}{fileName}";

        var filePath = Path.Combine(uploadsFoalder, storedFileName);


        try
        {
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            await fileStream.CopyToAsync(fs, ct);   
            return storedFileName;
        }
        catch (Exception ex)
        {
            return null;
        }

    }
}
