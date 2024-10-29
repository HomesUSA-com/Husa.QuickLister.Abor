namespace Husa.Quicklister.Abor.Api.Contracts.Response.ResidentialIdx
{
    public class ResidentialIdxResponse : Husa.Quicklister.Extensions.Api.Contracts.Response.ResidentialIdx.ResidentialIdxResponse
    {
        public FinancialIdxResponse Financial { get; set; }
        public SchoolsIdxResponse Schools { get; set; }
        public PropertyIdxResponse Property { get; set; }
    }
}
