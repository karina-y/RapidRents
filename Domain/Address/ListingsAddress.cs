using RapidRents.Web.Domain.Listings;
using System.Collections.Generic;

namespace RapidRents.Web.Domain.Address
{
    public class ListingsAddress : Address
    {
        public List<Listing> Listings { get; set; }
    }
}
