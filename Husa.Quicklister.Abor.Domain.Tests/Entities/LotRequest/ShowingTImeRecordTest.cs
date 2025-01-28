namespace Husa.Quicklister.Abor.Domain.Tests.Entities.LotRequest
{
    using Husa.Quicklister.Extensions.Domain.Entities.Request;
    using Husa.Quicklister.Extensions.Domain.Enums.ShowingTime;

    public record ShowingTimeRecordTest : ShowingTimeRecord
    {
        public ShowingTimeRecordTest()
        {
            this.AppointmentType = AppointmentType.AppointmentRequired;
            this.AppointmentRestrictions = new()
            {
                AllowAppraisals = true,
                AllowInspectionsAndWalkThroughs = true,
                LeadTime = true,
                RequiredTimeHours = 0,
                SuggestedTimeHours = 0,
            };
            this.AdditionalInstructions = new()
            {
                NotesForApptStaff = "Test",
                NotesForShowingAgent = "Test",
            };
            this.AccessInformation = new()
            {
                AccessMethod = AccessMethod.GateGuardDoormanConcierge,
                Location = "Test",
                Serial = "Test",
                Combination = "Test",
                SharingCode = "Test",
                CbsCode = "Test",
                Code = "Test",
                DeviceId = "Test",
                ProvideAlarmDetails = true,
                AlarmArmCode = "Test",
                AlarmDisarmCode = "Test",
                AlarmNotes = "Test",
                AlarmPasscode = "Test",
            };
        }
    }
}
