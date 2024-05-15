namespace Husa.Quicklister.Abor.Data.Documents.Extensions
{
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Request;

    public static class RequestQueryExtensions
    {
        public static TStatusResult ToProjectionStatusFieldsQueryResult<TStatusFields, TStatusResult>(this TStatusFields statusField)
            where TStatusFields : StatusFieldsRecord
            where TStatusResult : ListingStatusFieldsQueryResult, new()
        {
            if (statusField == null)
            {
                return new();
            }

            return new()
            {
                PendingDate = statusField.PendingDate,
                ClosedDate = statusField.ClosedDate,
                EstimatedClosedDate = statusField.EstimatedClosedDate,
                CancelledReason = statusField.CancelledReason,
                ClosePrice = statusField.ClosePrice,
                AgentId = statusField.AgentId,
                HasBuyerAgent = statusField.HasBuyerAgent,
                HasSecondBuyerAgent = statusField.HasSecondBuyerAgent,
                AgentIdSecond = statusField.AgentIdSecond,
                BackOnMarketDate = statusField.BackOnMarketDate,
                OffMarketDate = statusField.OffMarketDate,
                HasContingencyInfo = statusField.HasContingencyInfo,
                SaleTerms = statusField.SaleTerms,
                SellConcess = statusField.SellConcess,
            };
        }
    }
}
