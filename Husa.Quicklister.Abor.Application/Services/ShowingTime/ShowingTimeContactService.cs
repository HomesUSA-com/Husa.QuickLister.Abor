namespace Husa.Quicklister.Abor.Application.Services.ShowingTime
{
    using AutoMapper;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Services.ShowingTime;

    public class ShowingTimeContactService : ShowingTimeContactServiceBase<ShowingTimeContact>
    {
        public ShowingTimeContactService(
            IMapper mapper,
            IUserContextProvider userContextProvider,
            IShowingTimeContactRepository contactRepository)
            : base(contactRepository, userContextProvider, mapper)
        {
        }
    }
}
