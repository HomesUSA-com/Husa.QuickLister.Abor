namespace Husa.Quicklister.Abor.Domain.Tests.Entities.LotRequest
{
    using System.Collections.Generic;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Entities.Request;
    using ShowingTimeEnums = Husa.Quicklister.Extensions.Domain.Enums.ShowingTime;

    public record ShowingTimeRecordDummyTest : ShowingTimeRecord
    {
        public override IEnumerable<SummarySection> GetSummarySections(ShowingTimeRecord record)
        {
            yield return new SummarySection
            {
                Name = AppointmentSettingsRecord.SummarySection,
                Fields =
                [
                    new SummaryField
                    {
                        FieldName = nameof(AppointmentSettingsRecord.AppointmentType),
                        NewValue = ShowingTimeEnums.AppointmentType.AppointmentRequiredConfirmWithAll,
                        OldValue = null,
                    },
                ],
            };
            yield return new SummarySection
            {
                Name = AppointmentRestrictionsRecord.SummarySection,
                Fields =
                [
                    new SummaryField
                    {
                        FieldName = nameof(AppointmentRestrictionsRecord.SuggestedTimeHours),
                        NewValue = 0,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AppointmentRestrictionsRecord.RequiredTimeHours),
                        NewValue = 0,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AppointmentRestrictionsRecord.AllowAppraisals),
                        NewValue = false,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AppointmentRestrictionsRecord.AllowInspectionsAndWalkThroughs),
                        NewValue = false,
                        OldValue = null,
                    },
                ],
            };
            yield return new SummarySection
            {
                Name = AdditionalInstructionsRecord.SummarySection,
                Fields =
                [
                    new SummaryField
                    {
                        FieldName = nameof(AdditionalInstructionsRecord.NotesForApptStaff),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AdditionalInstructionsRecord.NotesForShowingAgent),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                ],
            };
            yield return new SummarySection
            {
                Name = AccessInformationRecord.SummarySection,
                Fields =
                [
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.AccessMethod),
                        NewValue = ShowingTimeEnums.AccessMethod.GateGuardDoormanConcierge,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.AlarmArmCode),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.AlarmDisarmCode),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.AlarmNotes),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.AlarmPasscode),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.CbsCode),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.Code),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.Combination),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.DeviceId),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.Location),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.ProvideAlarmDetails),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.Serial),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                    new SummaryField
                    {
                        FieldName = nameof(AccessInformationRecord.SharingCode),
                        NewValue = string.Empty,
                        OldValue = null,
                    },
                ],
            };
        }
    }
}
