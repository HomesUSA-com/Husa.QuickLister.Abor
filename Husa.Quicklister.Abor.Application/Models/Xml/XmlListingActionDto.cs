namespace Husa.Quicklister.Abor.Application.Models.Xml
{
    using System;
    using Husa.Xml.Domain.Enums;

    public class XmlListingActionDto
    {
        public ListActionType Type { get; set; }

        public DateTime? ListOn { get; set; }
    }
}
