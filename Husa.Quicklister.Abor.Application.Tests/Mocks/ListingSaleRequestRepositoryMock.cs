namespace Husa.Quicklister.Abor.Application.Tests.Mocks
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Moq;

    public class ListingSaleRequestRepositoryMock : Mock<ISaleListingRequestRepository>
    {
        public ListingSaleRequestRepositoryMock()
        {
            this.Setup(r => r.AddListingSaleRequestAsync(
                It.IsAny<SaleListingRequest>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(TestModelProvider.GetListingSaleRequestObject(Guid.NewGuid())))
                .Verifiable();

            this.Setup(r => r.UpdateListingSaleRequestAsync(
                It.IsAny<string>(),
                It.IsAny<SaleListingRequest>(),
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(TestModelProvider.GetListingSaleRequestEntity(Guid.NewGuid())))
                .Verifiable();
        }
    }
}
