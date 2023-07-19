namespace Husa.Quicklister.Abor.Crosscutting
{
    using System;
    using Husa.Extensions.Authorization.Models;

    public class XmlUserSettings
    {
        public const string Section = "XmlUser";

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool MLSAdministrator { get; set; }

        public UserContext GetXmlUser() => new()
        {
            Email = this.Email,
            Name = this.Name,
            Id = this.Id,
            IsMLSAdministrator = this.MLSAdministrator,
        };
    }
}
