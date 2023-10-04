namespace Husa.Quicklister.Abor.Application.Interfaces.Media
{
    using System;
    using System.Threading.Tasks;

    public interface IMediaService
    {
        Task ProcessData(Guid listingId, DateTime updatedOn);
    }
}
