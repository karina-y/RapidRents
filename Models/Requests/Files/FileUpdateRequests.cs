using System.ComponentModel.DataAnnotations;

namespace Sabio.Web.Models.Requests.Files
{
    public class FileUpdateRequests: FileAddRequests
    {
        [Required]
        public int Id { get; set; }
    }
}
