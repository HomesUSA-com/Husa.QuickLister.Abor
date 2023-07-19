namespace Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest
{
    public class SummaryFieldContract
    {
        public string FieldName { get; set; }

        public object OldValue { get; set; }

        public object NewValue { get; set; }
    }
}
