using System;

namespace RapidRents.Web.Domain.Company
{
    public class Company
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string MonOpen { get; set; }

        public string MonClose { get; set; }

        public string TueOpen { get; set; }

        public string TueClose { get; set; }

        public string WedOpen { get; set; }

        public string WedClose { get; set; }

        public string ThuOpen { get; set; }

        public string ThuClose { get; set; }

        public string FriOpen { get; set; }

        public string FriClose { get; set; }

        public string SatOpen { get; set; }

        public string SatClose { get; set; }

        public string SunOpen { get; set; }

        public string SunClose { get; set; }

    }
}
