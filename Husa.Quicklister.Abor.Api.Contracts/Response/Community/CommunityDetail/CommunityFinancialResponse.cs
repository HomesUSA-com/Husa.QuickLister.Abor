namespace Husa.Quicklister.Abor.Api.Contracts.Response.Community.CommunityDetail
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CommunityFinancialResponse
    {
        public bool IsMultipleTaxed { get; set; }
        public decimal? TaxRate { get; set; }
        public string TitleCompany { get; set; }
        public ICollection<ProposedTerms> ProposedTerms { get; set; }
        public HoaRequirement? HOARequirement { get; set; }
        public decimal? BuyersAgentCommission { get; set; }
        public CommissionType BuyersAgentCommissionType { get; set; }
        public CommunitySchoolsResponse Schools { get; set; }
    }
}
