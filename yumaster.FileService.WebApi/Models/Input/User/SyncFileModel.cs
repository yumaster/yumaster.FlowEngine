using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace yumaster.FileService.WebApi.Models.Input.User
{
    public class SyncFileModel
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string FileToken { get; set; }
    }
}
