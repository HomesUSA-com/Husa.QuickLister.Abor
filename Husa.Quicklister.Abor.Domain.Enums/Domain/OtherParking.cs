namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum OtherParking
    {
        [EnumMember(Value = "NONE")]
        [Description("None/Not Applicable")]
        NotApplicable,
        [EnumMember(Value = "1CAR")]
        [Description("One Car Carport")]
        OneCarCarport,
        [EnumMember(Value = "2CAR")]
        [Description("Two Car Carport")]
        TwoCarCarport,
        [EnumMember(Value = "OPEN")]
        [Description("Open")]
        Open,
        [EnumMember(Value = "PAD")]
        [Description("Pad Only (Off Street)")]
        PadOnlyOffStreet,
        [EnumMember(Value = "STONL")]
        [Description("Street Parking Only")]
        StreetParkingOnly,
    }
}
