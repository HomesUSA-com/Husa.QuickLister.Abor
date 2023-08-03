namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum ShowingRequirements
    {
        [EnumMember(Value = "AGNTOROWN")]
        [Description("Agent or Owner Present")]
        AgentorOwnerPresent,
        [EnumMember(Value = "APPT")]
        [Description("Appointment Only")]
        AppointmentOnly,
        [EnumMember(Value = "CALL1ST")]
        [Description("CALL-1ST")]
        Call1st,
        [EnumMember(Value = "GO")]
        [Description("Go")]
        Go,
        [EnumMember(Value = "KEYLOCK")]
        [Description("Lockbox")]
        Lockbox,
        [EnumMember(Value = "KEYLOF")]
        [Description("Key In Office")]
        KeyInOffice,
        [EnumMember(Value = "KEYOWNR")]
        [Description("Key with Owner")]
        KeywithOwner,
        [EnumMember(Value = "OWNR")]
        [Description("Call Owner")]
        CallOwner,
        [EnumMember(Value = "SECSYS")]
        [Description("Security System")]
        SecuritySystem,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Showing Instructions")]
        SeeShowingInstructions,
        [EnumMember(Value = "SSERVICE")]
        [Description("Showing Service")]
        ShowingService,
    }
}
