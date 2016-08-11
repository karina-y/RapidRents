using System;
using System.Collections.Generic;


namespace RapidRents.Web.Domain.Address
{
    public class Address : BaseAddress
    {
        public string UserId { get; set; }

        public string ZipCode { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }       

        public int RentCost { get; set; }   

        public List<Amenity> Amenities { get; set; }

        public string StateProvinceCode { get; set; }

        public decimal Geo { get; set; }

        public int ImportedAddressId { get; set; }

    }
}
