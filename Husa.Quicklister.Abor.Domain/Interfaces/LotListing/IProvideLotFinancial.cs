namespace Husa.Quicklister.Abor.Domain.Interfaces.LotListing
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public interface IProvideLotFinancial : IProvideAgentCommission, IProvideAgentBonusCommission
    {
        decimal? TaxRate { get; set; }
        ICollection<AcceptableFinancing> AcceptableFinancing { get; set; }

        ICollection<HoaIncludes> HoaIncludes { get; set; }
        bool HasHoa { get; set; }
        BillingFrequency? BillingFrequency { get; set; }
        HoaRequirement? HOARequirement { get; set; }

        bool HasAgentBonus { get; set; }
        bool HasBonusWithAmount { get; set; }
        DateTime? BonusExpirationDate { get; set; }
        bool HasBuyerIncentive { get; set; }
    }
}
