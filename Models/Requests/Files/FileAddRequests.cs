using System.ComponentModel.DataAnnotations;

namespace RapidRents.Web.Models
{
    public class FileAddRequests
    {
        [Required]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "A File Path is required.")]
        public string FilePath { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "A File Name is required.")]
        public string FileName { get; set; }
        
    }
}
