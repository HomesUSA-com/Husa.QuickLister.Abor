namespace Husa.Quicklister.Abor.Domain.Extensions.JsonImport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using JsonEnums = Husa.JsonImport.Domain.Enums;

    public static class JsonImportEnumExtensions
    {
        public static MarketStatuses ToMarket(this JsonEnums.ListStatus value)
            => value switch
            {
                JsonEnums.ListStatus.Active => MarketStatuses.Active,
                JsonEnums.ListStatus.Pending => MarketStatuses.Pending,
                JsonEnums.ListStatus.Cancelled => MarketStatuses.Canceled,
                JsonEnums.ListStatus.Sold => MarketStatuses.Closed,
                _ => throw new NotImplementedException(),
            };

        public static SaleTerms? ToMarket(this JsonEnums.AcceptableFinancing value)
            => value switch
            {
                JsonEnums.AcceptableFinancing.Cash => SaleTerms.Cash,
                JsonEnums.AcceptableFinancing.FHA => SaleTerms.FHA,
                JsonEnums.AcceptableFinancing.TexasVet => SaleTerms.TexasVet,
                JsonEnums.AcceptableFinancing.Conventional => SaleTerms.Conventional,
                JsonEnums.AcceptableFinancing.USDA => SaleTerms.UsdaEligible,
                JsonEnums.AcceptableFinancing.VA => SaleTerms.VA,
                _ => null,
            };

        public static ICollection<SaleTerms> ToMarket(this ICollection<JsonEnums.AcceptableFinancing> value)
            => value.Select(x => x.ToMarket()).Where(x => x != null).Select(x => x.Value).ToList();

        public static ConstructionStage? ToMarket(this JsonEnums.ConstructionStage value)
            => value switch
            {
                JsonEnums.ConstructionStage.Incomplete => ConstructionStage.Incomplete,
                JsonEnums.ConstructionStage.Complete => ConstructionStage.Complete,
                _ => null,
            };
    }
}
