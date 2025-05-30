namespace Husa.Quicklister.Abor.Domain.Entities.ShowingTime
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Extensions.Domain.Enums.ShowingTime;
    using Husa.Quicklister.Extensions.Domain.Interfaces.ShowingTime;

    public class ShowingTimeContact : Entity, IShowingTimeContact, IProvideCompany
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OfficePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public bool? ConfirmAppointmentsByOfficePhone { get; set; }
        public ConfirmAppointmentCaller? ConfirmAppointmentCallerByOfficePhone { get; set; }
        public bool? NotifyAppointmentChangesByOfficePhone { get; set; }
        public AppointmentChangesNotificationOptions? AppointmentChangesNotificationsOptionsOfficePhone { get; set; }
        public bool? ConfirmAppointmentsByMobilePhone { get; set; }
        public ConfirmAppointmentCaller? ConfirmAppointmentCallerByMobilePhone { get; set; }
        public bool? NotifyAppointmentChangesByMobilePhone { get; set; }
        public AppointmentChangesNotificationOptions? AppointmentChangesNotificationsOptionsMobilePhone { get; set; }
        public bool? ConfirmAppointmentsByText { get; set; }
        public bool? NotifyAppointmentsChangesByText { get; set; }
        public bool? SendOnFYIByText { get; set; }
        public bool? ConfirmAppointmentsByEmail { get; set; }
        public bool? NotifyAppointmentChangesByEmail { get; set; }
        public bool? SendOnFYIByEmail { get; set; }
        public virtual ICollection<CommunitySale> Communities { get; set; }
        public virtual ICollection<SaleListing> Listings { get; set; }

        protected override void DeleteChildren(Guid userId)
        {
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.FirstName;
            yield return this.LastName;
            yield return this.OfficePhone;
            yield return this.MobilePhone;
            yield return this.Email;
            yield return this.ConfirmAppointmentsByOfficePhone;
            yield return this.ConfirmAppointmentCallerByOfficePhone;
            yield return this.NotifyAppointmentChangesByOfficePhone;
            yield return this.AppointmentChangesNotificationsOptionsOfficePhone;
            yield return this.ConfirmAppointmentsByMobilePhone;
            yield return this.ConfirmAppointmentCallerByMobilePhone;
            yield return this.NotifyAppointmentChangesByMobilePhone;
            yield return this.AppointmentChangesNotificationsOptionsMobilePhone;
            yield return this.ConfirmAppointmentsByText;
            yield return this.NotifyAppointmentsChangesByText;
            yield return this.SendOnFYIByText;
            yield return this.ConfirmAppointmentsByEmail;
            yield return this.NotifyAppointmentChangesByEmail;
            yield return this.SendOnFYIByEmail;
            yield return this.Communities;
        }
    }
}
