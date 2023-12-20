namespace Husa.Quicklister.Abor.Api.Client.Interfaces
{
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using QlExtension = Husa.Quicklister.Extensions.Api.Client.Interfaces;

    public interface IAlert : QlExtension.IAlert<AlertDetailResponse>
    {
    }
}
