namespace Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.ReverseProspect;
    using Husa.Quicklister.Abor.Domain.Enums;

    public static class TrackingReverseProspectProvider
    {
        public static TrackingReverseProspect GetReverseProspectEntity(Guid listingId, Guid userId, Guid companyId, string reportData, ReverseProspectStatus status)
        {
            return new TrackingReverseProspect(
                listingId,
                userId,
                companyId,
                reportData,
                status);
        }
    }
}
