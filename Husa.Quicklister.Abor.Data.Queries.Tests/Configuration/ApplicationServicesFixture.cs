namespace Husa.Quicklister.Abor.Data.Queries.Tests.Configuration
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Configuration;

    public class ApplicationServicesFixture
    {
        public ApplicationServicesFixture()
        {
            this.Mapper = Bootstrapper.ConfigureAutoMapper();
        }

        public IMapper Mapper { get; }
    }
}
