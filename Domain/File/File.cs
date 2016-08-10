using System;

namespace rapidRents.Web.Domain.File
{
    public class File : BaseFile
    {
        public string FileName { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

        public string UserId { get; set; }
    }
}
