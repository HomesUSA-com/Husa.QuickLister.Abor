namespace Husa.Quicklister.Abor.Application.Interfaces.Uploader
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.ReverseProspect.Api.Contracts.Response;

    public interface IUploaderService
    {
        Task<CommandResult<ReverseProspectData>> GetReverseProspectListing(Guid listingId, bool usingDatabase = true, CancellationToken cancellationToken = default);
    }
}
