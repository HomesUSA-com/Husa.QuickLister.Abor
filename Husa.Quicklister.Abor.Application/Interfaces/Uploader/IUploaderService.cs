namespace Husa.Quicklister.Abor.Application.Interfaces.Uploader
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Application.Models.ReverseProspect;

    public interface IUploaderService
    {
        Task<CommandResult<ReverseProspectInformationDto>> GetReverseProspectListing(Guid listingId, bool usingDatabase = true, CancellationToken cancellationToken = default);
    }
}
