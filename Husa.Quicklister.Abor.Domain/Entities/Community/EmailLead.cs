namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using Husa.Quicklister.Extensions.Domain.Extensions;
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

            emailLeadInfo.UpdateEmailLeadFromXml(subdivision);

            return emailLeadInfo;
        }

        public virtual EmailLead UpdateFromXml(SubdivisionResponse subdivision)
        {
            var clonnedEmailLead = this.Clone();
            clonnedEmailLead.UpdateEmailLeadFromXml(subdivision);
            return clonnedEmailLead;
        }

        public EmailLead Clone()
        {
            return (EmailLead)this.MemberwiseClone();
        }
    }
}
