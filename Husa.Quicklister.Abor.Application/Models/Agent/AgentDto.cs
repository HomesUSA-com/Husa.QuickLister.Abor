namespace Husa.Quicklister.Abor.Application.Models.Agent
{
    using System;
    using Husa.Downloader.CTX.Domain.Enums;

    public class AgentDto
    {
        public string MarketUniqueId { get; set; }

        public string OfficeId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public MemberStatus Status { get; set; }

        public string CellPhone { get; set; }

        public string WorkPhone { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string HomePhone { get; set; }

        public string Web { get; set; }

        public DateTimeOffset MarketModified { get; set; }

        public bool MlsAccess { get; set; }

        public MemberMlsSecurityClass MlsSecurityClass { get; set; }

        public DateTime SysModified { get; set; }
        public string MemberStateLicense { get; set; }
        public string MlsId { get; set; }
    }
}
