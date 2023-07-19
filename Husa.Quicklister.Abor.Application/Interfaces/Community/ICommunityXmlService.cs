namespace Husa.Quicklister.Abor.Application.Interfaces.Community
{
    using System;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Interfaces.Xml;

    public interface ICommunityXmlService : IProfileXmlService
    {
        Task ApproveCommunity(Guid communityId);
    }
}
