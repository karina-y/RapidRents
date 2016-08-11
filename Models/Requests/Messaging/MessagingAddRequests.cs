using System.ComponentModel.DataAnnotations;

namespace RapidRents.Web.Models.Requests.Messaging
{
    public class MessagingAddRequests
    {
        [Required, StringLength(100)]
        public string TypeId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(2000)]
        public string Message { get; set; }
    }
}
