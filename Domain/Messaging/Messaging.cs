using System;

namespace RapidRents.Web.Domain
{
    public class Messaging
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string TypeId { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }
    }
}
