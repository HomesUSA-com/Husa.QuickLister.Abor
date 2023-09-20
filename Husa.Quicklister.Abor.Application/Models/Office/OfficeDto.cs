namespace Husa.Quicklister.Abor.Application.Models.Office
{
    using System;
    using Husa.Downloader.CTX.Domain.Enums;
    using Cities = Husa.Quicklister.Abor.Domain.Enums.Domain.Cities;

    public class OfficeDto
    {
        public string MarketUniqueId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public Cities? City { get; set; }

        public StateOrProvince StateOrProvince { get; set; }

        public string Zip { get; set; }

        public string ZipExt { get; set; }

        public string Phone { get; set; }

        public OfficeStatus Status { get; set; }

        public DateTimeOffset MarketModified { get; set; }

        public OfficeType Type { get; set; }
    }
}
