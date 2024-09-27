namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using System;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Models.Listing;
    using Microsoft.Extensions.Logging;
    using QLExtensions = Husa.Quicklister.Extensions.Application.Services.SaleListings;
    public class CallForwardService : QLExtensions.CallForwardService<SaleListing, IListingSaleRepository>
    {
        public CallForwardService(
            IListingSaleRepository listingRepository,
            IUserContextProvider userContextProvider,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<CallForwardService> logger)
            : base(listingRepository, userContextProvider, serviceSubscriptionClient, logger)
        {
        }

        protected override string GetCommunityPhone(SaleListing listing)
        {
            var phone = string.Empty;
            var community = listing.SaleProperty.Community;
            if (community != null)
            {
                if (!string.IsNullOrEmpty(community.Showing.ContactPhone))
                {
                    phone = community.Showing.ContactPhone;
                }
                else if (!string.IsNullOrEmpty(community.ProfileInfo.OfficePhone))
                {
                    phone = community.ProfileInfo.OfficePhone;
                }
                else if (!string.IsNullOrEmpty(community.ProfileInfo.BackupPhone))
                {
                    phone = community.ProfileInfo.BackupPhone;
                }
            }

            return phone;
        }

        protected override (bool Centralized, string Phone) GetCentralizedPhone(SaleListing listing, CompanyDetail company) =>
            listing.ListType switch
            {
                ListType.Residential => (company.PhoneLeadInfo.IsCentralizedForSale, company.PhoneLeadInfo.CentralizeLeadPhone),
                ListType.Lease => (company.PhoneLeadInfo.IsCentralizedForLease, company.PhoneLeadInfo.CentralizeLeadPhoneForLease),
                ListType.Lots => (company.PhoneLeadInfo.IsCentralizedForLot, company.PhoneLeadInfo.CentralizeLeadPhoneForLot),
                _ => (false, string.Empty),
            };

        protected override ListingDetails GetListingDetails(SaleListing listing)
        {
            if (listing == null)
            {
                throw new ArgumentNullException(nameof(listing));
            }

            return new ListingDetails()
            {
                Address = listing.SaleProperty.Address,
                City = listing.SaleProperty.AddressInfo.City.GetEnumDescription(),
                ZipCode = listing.SaleProperty.AddressInfo.ZipCode,
                Subdivision = listing.SaleProperty.AddressInfo.Subdivision,
            };
        }
    }
}
