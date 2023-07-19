namespace Husa.Quicklister.Abor.Application.Tests.Mocks
{
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Moq;

    public class ListingSaleMock : Mock<SaleListing>
    {
        public ListingSaleMock()
        {
            this.Setup();
        }

        private void Setup()
        {
            var listingSale = new Mock<SaleListing>();
            listingSale.SetupAllProperties();
        }
    }
}
