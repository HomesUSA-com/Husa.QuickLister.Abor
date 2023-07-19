namespace Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class HoaRequest
    {
        private const string WebsiteRegex = @"^(?:(?:(?:https?):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z0-9\u00a1-\uffff][a-z0-9\u00a1-\uffff_-]{0,86})?[a-z0-9\u00a1-\uffff]\.)+(?:[a-z\u00a1-\uffff]{2,}))(?::\d{2,5})?(?:[/?#]\S*)?$";
        public Guid ListingId { get; set; }
        public string Name { get; set; }
        public decimal TransferFee { get; set; }
        public decimal Fee { get; set; }
        public BillingFrequency BillingFrequency { get; set; }
        [RegularExpression(WebsiteRegex, ErrorMessage = "Please provide a valid HOA website. It must begin with 'http://' or 'https://'")]
        [MaxLength(100)]
        public string Website { get; set; }
        public string ContactPhone { get; set; }
        public string HoaType { get; set; }
    }
}
