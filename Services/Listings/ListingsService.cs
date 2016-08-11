using RapidRents.Data;
using RapidRents.Web.Domain.Listings;
using RapidRents.Web.Models.Requests.Listings;
using RapidRents.Web.Services.Addresses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace RapidRents.Web.Services.Listings
{
    public class ListingsService : BaseService, IListingsService
    {
        private IAddressService _addressService = null;

        public ListingsService(IAddressService s)
        {
            _addressService = s;
        }

        public int InsertNewListing(ListingsAddRequest model)
        {
            int id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Listings_Insert"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@RentCost", model.RentCost);
                    paramCollection.AddWithValue("@LeaseTerms", model.LeaseTerms);
                    paramCollection.AddWithValue("@AvailabilityDate", model.AvailabilityDate);
                    paramCollection.AddWithValue("@Status", model.Status);
                    paramCollection.AddWithValue("@HasParking", model.HasParking);
                    paramCollection.AddWithValue("@UtilitiesIncluded", model.UtilitiesIncluded);
                    paramCollection.AddWithValue("@Description", model.Description);
                    paramCollection.AddWithValue("@Headline", model.Headline);

                    SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                    p.Direction = ParameterDirection.Output;

                    paramCollection.Add(p);
                }, returnParameters: delegate (SqlParameterCollection param)
                {
                    int.TryParse(param["@Id"].Value.ToString(), out id);
                }
               );

            return id;
        }

        public void UpdateListingsRecord(ListingsUpdateRequest model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Listings_Update"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@id", model.id);
                    paramCollection.AddWithValue("@RentCost", model.RentCost);
                    paramCollection.AddWithValue("@LeaseTerms", model.LeaseTerms);
                    paramCollection.AddWithValue("@AvailabilityDate", model.AvailabilityDate);
                    paramCollection.AddWithValue("@Status", model.Status);
                    paramCollection.AddWithValue("@HasParking", model.HasParking);
                    paramCollection.AddWithValue("@UtilitiesIncluded", model.UtilitiesIncluded);
                    paramCollection.AddWithValue("@Description", model.Description);
                    paramCollection.AddWithValue("@Headline", model.Headline);
                });
        }

        public List<Listing> GetAllListings()
        {
            List<Listing> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_Select2"
               , inputParamMapper: null
               , map: delegate (IDataReader reader, short set)
               {
                   Listing p = new Listing();
                   MapListing<Listing>(reader);

                   List<KeyValuePair<int, string>> utilitiesKvp = new List<KeyValuePair<int, string>>();
                   string[] utilities = ((UtilityTypes)p.UtilitiesIncluded).ToString()
                                                       .Split(',').ToArray();
                   foreach (string utility in utilities)
                   {
                       int key = (int)Enum.Parse(typeof(UtilityTypes), utility);
                       utilitiesKvp.Add(new KeyValuePair<int, string>(key, utility));
                   }
                   p.Utilities = utilitiesKvp;
                   p.UtilitiesList = ((UtilityTypes)p.UtilitiesIncluded).ToJsonDictionary();

                   if (list == null)
                   {
                       list = new List<Listing>();
                   }
                   list.Add(p);
               }
               );
            return list;
        }

        public List<Listing> GetFeatured()
        {
            List<Listing> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_SelectFeatured"
               , inputParamMapper: null
               , map: delegate (IDataReader reader, short set)
               {
                   Listing p = new Listing();

                   if (list == null)
                   {
                       list = new List<Listing>();
                   }
                   p = MapListing<Listing>(reader);
                   list.Add(p);
               });

            return list;
        }

        public List<Listing> GetLatest()
        {
            List<Listing> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_SelectLatest"
                , inputParamMapper: null
                , map: delegate (IDataReader reader, short set)
                {
                    Listing p = new Listing();

                    if (list == null)
                    {
                        list = new List<Listing>();
                    }
                    p = MapListing<Listing>(reader);
                    list.Add(p);
                });
            return list;
        }

        public Listing GetListingsById(int id)
        {
            Listing p = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_Select_By_Id"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", id);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   p = MapListing<Listing>(reader);

                   if (p == null)
                   {
                       p = new Listing();
                   }
               });
            return p;
        }

        public T MapListing<T>(IDataReader reader, int actualStartingIndex = 0) where T : Listing, new()
        {
            T s = new T();
            int startingIndex = actualStartingIndex;

            s.Id = reader.GetSafeInt32(startingIndex++);
            s.RentCost = reader.GetSafeInt32(startingIndex++);
            s.LeaseTerms = reader.GetSafeInt32(startingIndex++);
            s.AvailabilityDate = reader.GetSafeDateTime(startingIndex++);
            s.Status = reader.GetSafeInt32(startingIndex++);
            s.HasParking = reader.GetSafeBool(startingIndex++);
            s.UtilitiesIncluded = reader.GetSafeInt32(startingIndex++);
            s.DateAdded = reader.GetSafeDateTime(startingIndex++);
            s.DateModified = reader.GetSafeDateTime(startingIndex++);
            s.Description = reader.GetSafeString(startingIndex++);
            s.Headline = reader.GetSafeString(startingIndex++);
            s.Featured = reader.GetSafeBool(startingIndex++);

            return s;
        }

        public void DeleteListingsById(int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Listings_Delete_By_Id"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                });
        }

        public PagedList<Listing> GetAllPropListings(int pageIndex, int pageSize)
        {
            List<Listing> list = null;

            PagedList<Listing> page = null;
            int totalCount = 0;

            Listing p = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_By_Page"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@pageindex", pageIndex);
                   paramCollection.AddWithValue("@pagesize", pageSize);

               }, map: delegate (IDataReader reader, short set)
               {
                   p = MapListing<Listing>(reader);

                   int startingIndex = 12;

                   if (totalCount == 0)
                   {
                       totalCount = reader.GetSafeInt32(startingIndex++);
                   }

                   List<KeyValuePair<int, string>> utilitiesKvp = new List<KeyValuePair<int, string>>();
                   string[] utilities = ((UtilityTypes)p.UtilitiesIncluded).ToString()
                                                       .Split(',').ToArray();
                   foreach (string utility in utilities)
                   {
                       int key = (int)Enum.Parse(typeof(UtilityTypes), utility);
                       utilitiesKvp.Add(new KeyValuePair<int, string>(key, utility));
                   }
                   p.Utilities = utilitiesKvp;
                   p.UtilitiesList = ((UtilityTypes)p.UtilitiesIncluded).ToJsonDictionary();

                   if (list == null)
                   {
                       list = new List<Listing>();
                       page = new PagedList<Listing>(list, pageIndex, pageSize, totalCount);
                       page.PagedItems = list;
                   }
                   list.Add(p);
               });
            return page;
        }

        public PagedList<Listing> GetFavoriteListings(string UserId)
        {
            List<Listing> list = null;

            PagedList<Listing> page = null;

            Listing p = null;
            int totalCount = 0;
            int pageIndex = 0;
            int pageSize = 0;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Favorite_Listing_By_User"
             , inputParamMapper: delegate (SqlParameterCollection paramCollection)
             {
                 paramCollection.AddWithValue("@userid", UserId);
             }
             , map: delegate (IDataReader reader, short set)
             {
                 p = MapListing<Listing>(reader);
                 List<KeyValuePair<int, string>> utilitiesKvp = new List<KeyValuePair<int, string>>();
                 string[] utilities = ((UtilityTypes)p.UtilitiesIncluded).ToString()
                                                     .Split(',').ToArray();
                 foreach (string utility in utilities)
                 {
                     int key = (int)Enum.Parse(typeof(UtilityTypes), utility);
                     utilitiesKvp.Add(new KeyValuePair<int, string>(key, utility));
                 }
                 p.Utilities = utilitiesKvp;
                 p.UtilitiesList = ((UtilityTypes)p.UtilitiesIncluded).ToJsonDictionary();

                 if (list == null)
                 {
                     list = new List<Listing>();
                 }
                 list.Add(p);
             });

            if (list == null)
            {
                Console.WriteLine("there is nothing");
            }
            else
            {
                totalCount = list.Count;
                pageSize = totalCount;
                page = new PagedList<Listing>(list, pageIndex, pageSize, totalCount);

                page.PagedItems = list;
            }
            return page;
        }

        public List<Listing> GetListingsByRentCost(ListingsSearchRequest model)
        {
            List<Listing> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_SearchByRentCost"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@max", model.Max);
                   paramCollection.AddWithValue("@min", model.Min);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   Listing p = new Listing();

                   MapListing<Listing>(reader);

                   if (list == null)
                   {
                       list = new List<Listing>();
                   }
                   list.Add(p);
               }
               );
            return list;
        }

        public List<Listing> GetListingsByAvailabilityDate(ListingsSearchRequest model)
        {
            List<Listing> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_SearchByAvailabilityDate"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@AvailabilityDate", model.AvailabilityDate);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   Listing p = new Listing();

                   MapListing<Listing>(reader);

                   if (list == null)
                   {
                       list = new List<Listing>();
                   }
                   list.Add(p);
               }
               );
            return list;
        }

        public List<Listing> GetListingsByHasParking(ListingsSearchRequest model)
        {
            List<Listing> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_SearchByHasParking"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@HasParking", model.HasParking);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   Listing p = new Listing();

                   MapListing<Listing>(reader);

                   if (list == null)
                   {
                       list = new List<Listing>();
                   }
                   list.Add(p);
               }
               );
            return list;
        }

        public List<Listing> GetListingsByLeaseTerms(ListingsSearchRequest model)
        {
            List<Listing> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_SearchByLeaseTerms"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@LeaseTerms", model.LeaseTerms);
                }
               , map: delegate (IDataReader reader, short set)
               {
                   Listing p = new Listing();
                   MapListing<Listing>(reader);

                   if (list == null)
                   {
                       list = new List<Listing>();
                   }
                   list.Add(p);
               }
               );
            return list;
        }

        public int PLW_Insert(PLWAddRequest model)
        {
            int id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.PLW_Insert"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@AddressId", model.AddressId);

                    SqlParameter ids = new SqlParameter("@FileId", SqlDbType.Structured);

                    if (model.FileIds != null && model.FileIds.Any())
                    {
                        ids.Value = new IntIdTable(model.FileIds);
                    }

                    paramCollection.Add(ids);
                    paramCollection.AddWithValue("@RentCost", model.RentCost);
                    paramCollection.AddWithValue("@LeaseTerms", model.LeaseTerms);
                    paramCollection.AddWithValue("@AvailabilityDate", model.AvailabilityDate);
                    paramCollection.AddWithValue("@Status", model.Status);
                    paramCollection.AddWithValue("@HasParking", model.HasParking);
                    paramCollection.AddWithValue("@UtilitiesIncluded", model.UtilitiesIncluded);
                    paramCollection.AddWithValue("@Description", model.Description);
                    paramCollection.AddWithValue("@Headline", model.Headline);

                    SqlParameter p = new SqlParameter("@Id", SqlDbType.Int);
                    p.Direction = ParameterDirection.Output;

                    paramCollection.Add(p);


                }, returnParameters: delegate (SqlParameterCollection param)
                {
                    int.TryParse(param["@Id"].Value.ToString(), out id);
                }
               );

            return id;
        }

        public Listing GetListingByAddressId(int id)
        {
            SuperListing s = null;
            Domain.Address.Address a = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_SelectByAddressId2"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", id);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   if (set == 0)
                   {
                       s = MapListing<SuperListing>(reader);

                       var utilitiesKvp = new List<KeyValuePair<int, string>>();
                       string[] utilities = ((UtilityTypes)s.UtilitiesIncluded).ToString()
                                                           .Split(',').ToArray();
                       foreach (string utility in utilities)
                       {
                           int key = (int)Enum.Parse(typeof(UtilityTypes), utility);
                           utilitiesKvp.Add(new KeyValuePair<int, string>(key, utility));
                       }
                       s.Utilities = utilitiesKvp;
                       s.UtilitiesList = ((UtilityTypes)s.UtilitiesIncluded).ToJsonDictionary();
                   }
                   else if (set == 1)
                   {
                       a = _addressService.MapAddress(reader, 1);
                       s.Location = a;
                   }
               });
            return s;
        }

        public Listing GetListingByLiId(int id)
        {
            return GetSuper(id);
        }



        private Listing GetSuper(int id)
        {
            SuperListing s = null;
            Domain.Address.Address a = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_SelectListingByLiId"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", id);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   if (set == 0)
                   {
                       s = MapListing<SuperListing>(reader);

                       var utilitiesKvp = new List<KeyValuePair<int, string>>();
                       string[] utilities = ((UtilityTypes)s.UtilitiesIncluded).ToString().Split(',').ToArray();
                       foreach (string utility in utilities)
                       {
                           int key = (int)Enum.Parse(typeof(UtilityTypes), utility);
                           utilitiesKvp.Add(new KeyValuePair<int, string>(key, utility));
                       }
                       s.Utilities = utilitiesKvp;
                       s.UtilitiesList = ((UtilityTypes)s.UtilitiesIncluded).ToJsonDictionary();
                   }
                   else if (set == 1)
                   {
                       a = _addressService.MapAddress(reader, 1);
                       s.Location = a;
                   }
               });
            return s;
        }

        public PagedList<Domain.Address.Address> GetAllMapListingsAddresses(int pageIndex, int pageSize)
        {
            Domain.Address.Address singleAddress = null;
            Listing singleListing = null;

            List<Domain.Address.Address> addressList = null;

            PagedList<Domain.Address.Address> addressPage = null;

            Dictionary<int, List<Listing>> dict = null;

            int totalCount = 0;


            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_SelectPageByAddressId"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@pageindex", pageIndex);
                   paramCollection.AddWithValue("@pagesize", pageSize);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   if (set == 0)
                   {
                       singleAddress = _addressService.MapAddressListings<Domain.Address.ListingsAddress>(reader);
                       int startingIndex = 11;

                       if (totalCount == 0)
                       {
                           totalCount = reader.GetSafeInt32(startingIndex++);
                       }

                       if (addressList == null)
                       {
                           addressList = new List<Domain.Address.Address>();
                           addressPage = new PagedList<Domain.Address.Address>(addressList, pageIndex, pageSize, totalCount);
                       }
                       addressList.Add(singleAddress);
                   }

                   else if (set == 1)
                   {
                       int addressId = reader.GetSafeInt32(0);

                       singleListing = MapListing<Listing>(reader, 1);

                       if (totalCount == 0)
                       {
                           int startingIndex = 13;
                           totalCount = reader.GetSafeInt32(startingIndex++);
                       }

                       if (dict == null)
                       {
                           dict = new Dictionary<int, List<Listing>>();
                       }

                       if (!dict.ContainsKey(addressId))
                       {
                           dict[addressId] = new List<Listing>();
                       }

                       if (singleListing != null)
                       {
                           dict[addressId].Add(singleListing);
                       }
                   }
               });

            if (dict != null)
            {
                foreach (Domain.Address.ListingsAddress currentAddress in addressList)
                {
                    if (dict.ContainsKey(currentAddress.Id))
                    {
                        currentAddress.Listings = dict[currentAddress.Id];
                    }
                }
            }

            return addressPage;
        }


        public List<Domain.Address.Address> GetPopularListingsAddressesMap(decimal lat, decimal lng, int miles)
        {
            Domain.Address.Address singleAddress = null;
            Listing singleListing = null;

            List<Domain.Address.Address> addressList = null;

            Dictionary<int, List<Listing>> dict = null;

            int totalCount = 0;


            DataProvider.ExecuteCmd(GetConnection, "dbo.Listings_SelectPopularListings"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   
                   paramCollection.AddWithValue("@Lat", lat);
                   paramCollection.AddWithValue("@Lng", lng);
                   paramCollection.AddWithValue("@Miles", miles);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   if (set == 0)
                   {
                       singleAddress = _addressService.MapAddressListings<Domain.Address.ListingsAddress>(reader);
                       if (addressList == null)
                       {
                           addressList = new List<Domain.Address.Address>();
                          
                       }


                       addressList.Add(singleAddress);
                   }

                   else if (set == 1)
                   {
                       int addressId = reader.GetSafeInt32(0);

                       singleListing = MapListing<Listing>(reader, 1);

                       if (totalCount == 0)
                       {
                           int startingIndex = 13;
                           totalCount = reader.GetSafeInt32(startingIndex++);
                       }

                       if (dict == null)
                       {
                           dict = new Dictionary<int, List<Listing>>();
                       }

                       if (!dict.ContainsKey(addressId))
                       {
                           dict[addressId] = new List<Listing>();
                       }

                       if (singleListing != null)
                       {
                           dict[addressId].Add(singleListing);
                       }
                   }
               });

            if (dict != null)
            {
                foreach (Domain.Address.ListingsAddress currentAddress in addressList)
                {
                    if (dict.ContainsKey(currentAddress.Id))
                    {
                        currentAddress.Listings = dict[currentAddress.Id];
                    }
                }
            }
              return addressList;
        }
    }
}
