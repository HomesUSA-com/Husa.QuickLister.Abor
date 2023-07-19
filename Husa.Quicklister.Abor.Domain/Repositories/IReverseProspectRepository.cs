namespace Husa.Quicklister.Abor.Domain.Repositories
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.ReverseProspect;

    public interface IReverseProspectRepository : IRepository<TrackingReverseProspect>
    {
        Task<TrackingReverseProspect> GetReverseProspectByTrackingId(Guid listingId);
    }
}
