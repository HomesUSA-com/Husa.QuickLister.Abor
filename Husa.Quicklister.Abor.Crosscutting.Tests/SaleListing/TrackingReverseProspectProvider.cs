namespace Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.ReverseProspect;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public static class TrackingReverseProspectProvider
    {
        public static ReverseProspect GetReverseProspectEntity(Guid listingId, Guid userId, Guid companyId, string reportData, ReverseProspectStatus status)
        {
            return new ReverseProspect(
                listingId,
                userId,
                companyId,
                reportData,
                status);
        }
    }
}
