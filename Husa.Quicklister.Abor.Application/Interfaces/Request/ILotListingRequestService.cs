namespace Husa.Quicklister.Abor.Application.Interfaces.Request
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Request;

    public interface ILotListingRequestService : ExtensionsInterfaces.IListingRequestService<LotListingRequest, ShowingTimeContact>
    {
        Task<LotListingRequest> UpdateRequestAsync(LotListingRequest request, LotListingRequestDto listingRequestDto, CancellationToken cancellationToken = default);

        Task UpdateListingRequestAsync(Guid listingRequestId, LotListingRequestDto listingRequestDto, CancellationToken cancellationToken = default);
    }
}
