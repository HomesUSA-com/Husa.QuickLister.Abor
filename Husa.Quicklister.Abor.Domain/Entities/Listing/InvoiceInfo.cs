namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    public class InvoiceInfo
    {
        public InvoiceInfo(Guid userId, string invoiceId, string docNumber, DateTime createdDate)
            : this()
        {
            this.InvoiceRequestedBy = userId;
            this.InvoiceRequestedOn = createdDate;
            this.InvoiceId = invoiceId;
            this.DocNumber = docNumber;
        }

        public InvoiceInfo()
        {
        }

        public string InvoiceId { get; set; }
        public string DocNumber { get; set; }
        public Guid? InvoiceRequestedBy { get; set; }
        public DateTime? InvoiceRequestedOn { get; set; }
    }
}
