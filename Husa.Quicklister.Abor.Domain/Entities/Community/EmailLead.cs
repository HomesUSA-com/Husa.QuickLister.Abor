namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using Husa.Xml.Api.Contracts.Response;
    using ExtensionEmailLead = Husa.Quicklister.Extensions.Domain.Entities.Community.EmailLead;

    public class EmailLead : ExtensionEmailLead
    {
        public static EmailLead ImportFromXml(SubdivisionResponse subdivision, EmailLead emailLead)
        {
            var emailLeadInfo = new EmailLead();
            if (emailLead != null)
            {
                emailLeadInfo = emailLead.Clone();
            }

            if (!string.IsNullOrWhiteSpace(subdivision.LeadsEmails))
            {
                var emails = subdivision.LeadsEmails.Split(';');
                if (emails.Length > 0)
                {
                    emailLeadInfo.EmailLeadPrincipal = emails[0];
                }

                if (emails.Length > 1)
                {
                    emailLeadInfo.EmailLeadSecondary = emails[1];
                }

                if (emails.Length > 2)
                {
                    emailLeadInfo.EmailLeadOther = emails[2];
                }
            }
            else if (!string.IsNullOrWhiteSpace(subdivision.SaleOffice.Email))
            {
                emailLeadInfo.EmailLeadPrincipal = subdivision.SaleOffice.Email;
            }

            return emailLeadInfo;
        }

        public virtual EmailLead UpdateFromXml(SubdivisionResponse subdivision)
        {
            var clonnedEmailLead = this.Clone();

            if (!string.IsNullOrWhiteSpace(subdivision.LeadsEmails))
            {
                var emails = subdivision.LeadsEmails.Split(';');
                if (emails.Length > 0)
                {
                    clonnedEmailLead.EmailLeadPrincipal = emails[0];
                }

                if (emails.Length > 1)
                {
                    clonnedEmailLead.EmailLeadSecondary = emails[1];
                }

                if (emails.Length > 2)
                {
                    clonnedEmailLead.EmailLeadOther = emails[2];
                }
            }
            else if (!string.IsNullOrWhiteSpace(subdivision.SaleOffice.Email))
            {
                clonnedEmailLead.EmailLeadPrincipal = subdivision.SaleOffice.Email;
            }

            return clonnedEmailLead;
        }

        public EmailLead Clone()
        {
            return (EmailLead)this.MemberwiseClone();
        }
    }
}
