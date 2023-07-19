namespace Husa.Quicklister.Abor.Application.Interfaces.Media
{
    using System;
    using System.Threading.Tasks;

    public interface IXmlMediaService
    {
        public Task ImportPlanMedia(Guid xmlPlanId);

        public Task ImportSubdivisionMedia(Guid xmlSubdivisionId);

        public Task ImportListingMedia(Guid xmlListingId, bool checkMediaLimit = false, bool useServiceBus = true);
    }
}
