namespace Husa.Quicklister.Abor.Application.Models.Agent
{
    using System;

    public class AgentDto
    {
        public string MarketUniqueId { get; set; }

        public string LoginName { get; set; }
        public string OfficeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Status { get; set; }

        public string CellPhone { get; set; }

        public string WorkPhone { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string OtherPhone { get; set; }

        public DateTime MarketModified { get; set; }

        public DateTime SysModified { get; set; }
    }
}
