namespace Husa.Quicklister.Abor.Application.Tests.Mocks
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Moq;

    public class ListingSaleRequestRepositoryMock : Mock<ISaleListingRequestRepository>
    {
        public ListingSaleRequestRepositoryMock()
        {
            this.Setup(r => r.AddDocumentAsync(
                It.IsAny<SaleListingRequest>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(TestModelProvider.GetListingSaleRequestObject(Guid.NewGuid())))
                .Verifiable();

            this.Setup(r => r.UpdateDocumentAsync(
                It.IsAny<Guid>(),
                It.IsAny<SaleListingRequest>(),
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(TestModelProvider.GetListingSaleRequestEntity(Guid.NewGuid())))
                .Verifiable();
        }
    }
}
