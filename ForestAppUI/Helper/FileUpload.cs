using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ForestAppUI.Helper
{
    public static class FileUpload
    {
        public static async Task<string> SaveFileAsync(this IFormFile file, string WebRootPath)
        {
            var path = "/uploads/" + Guid.NewGuid() + file.FileName;
            using FileStream fileStream = new(WebRootPath + path, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return path;
        }
    }
}
