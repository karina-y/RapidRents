using System.ComponentModel.DataAnnotations;

namespace RapidRents.Web.Models.Requests.Company
{
    public class CompanyAddRequests
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number should be in valid US format")]
        [Phone]
        public string Phone { get; set; }

        [Required, StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(100)]
        [Url]
        public string Url { get; set;}

        [Required, StringLength(20)]
        public string MonOpen { get; set; }

        [Required, StringLength(20)]
        public string MonClose { get; set; }

        [Required, StringLength(20)]
        public string TueOpen { get; set; }

        [Required, StringLength(20)]
        public string TueClose { get; set; }

        [Required, StringLength(20)]
        public string WedOpen { get; set; }

        [Required, StringLength(20)]
        public string WedClose { get; set; }

        [Required, StringLength(20)]
        public string ThuOpen { get; set; }

        [Required, StringLength(20)]
        public string ThuClose { get; set; }

        [Required, StringLength(20)]
        public string FriOpen { get; set; }

        [Required, StringLength(20)]
        public string FriClose { get; set; }

        [Required, StringLength(20)]
        public string SatOpen { get; set; }

        [Required, StringLength(20)]
        public string SatClose { get; set; }

        [Required, StringLength(20)]
        public string SunOpen { get; set; }

        [Required, StringLength(20)]
        public string SunClose { get; set; }
    }
}
