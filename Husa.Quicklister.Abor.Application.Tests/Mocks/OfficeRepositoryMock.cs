namespace Husa.Quicklister.Abor.Application.Tests.Mocks
{
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Moq;

    public class OfficeRepositoryMock : Mock<IOfficeRepository>
    {
        public OfficeRepositoryMock()
        {
            this.Setup();
        }

        private void Setup()
        {
            this.Setup(r => r.AddAsync(It.IsAny<Office>())).ReturnsAsync(TestModelProvider.GetOfficeEntity());
        }
    }
}
