namespace Husa.Quicklister.Abor.Application.Interfaces.Request
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Request;

    public interface ISaleListingRequestService : ExtensionsInterfaces.IListingRequestService<SaleListingRequest, ShowingTimeContact>
    {
        Task<SaleListingRequest> UpdateRequestAsync(SaleListingRequest request, ListingSaleRequestDto listingSaleRequestDto, CancellationToken cancellationToken = default);

        Task UpdateListingRequestAsync(Guid listingRequestId, ListingSaleRequestDto listingSaleRequestDto, CancellationToken cancellationToken = default);
    }
}
