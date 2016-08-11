using RapidRents.Web.Domain.Listings;
using RapidRents.Web.Models.Requests.Listings;
using System.Collections.Generic;
using System.Data;

namespace RapidRents.Web.Services.Listings
{
    public interface IListingsService
    {
        Listing GetListingsById(int id);
        Listing GetListingByAddressId(int Id);
        Listing GetListingByLiId(int id);

        List<Listing> GetAllListings();
        List<Listing> GetFeatured();
        List<Listing> GetLatest();

        List<Listing> GetListingsByRentCost(ListingsSearchRequest model);
        List<Listing> GetListingsByAvailabilityDate(ListingsSearchRequest model);
        List<Listing> GetListingsByHasParking(ListingsSearchRequest model);
        List<Listing> GetListingsByLeaseTerms(ListingsSearchRequest model);

        PagedList<Listing> GetAllPropListings(int pageNumber, int pageSize);
        PagedList<Listing> GetFavoriteListings(string UserId);
        PagedList<Domain.Address.Address> GetAllMapListingsAddresses(int pageIndex, int pageSize);
        List<Domain.Address.Address> GetPopularListingsAddressesMap(decimal lat, decimal lng, int miles);
        int PLW_Insert(PLWAddRequest model);
        int InsertNewListing(ListingsAddRequest model);
        void UpdateListingsRecord(ListingsUpdateRequest model);
        void DeleteListingsById(int id);

        T MapListing<T>(IDataReader reader, int actualStartingIndex = 0) where T : Listing, new();
    }
}
