namespace Husa.Quicklister.Abor.Application.Tests.Mocks
{
    using System;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Moq;

    public class ListingSaleRepositoryMock : Mock<IListingSaleRepository>
    {
        public ListingSaleRepositoryMock()
        {
            this.Setup();
        }

        private void Setup()
        {
            this.Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(Task.FromResult(TestModelProvider.GetListingSaleEntity(Guid.NewGuid(), true)));
        }
    }
}
