namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum UtilitiesDescription
    {
        [EnumMember(Value = "ABVGRD")]
        [Description("Above Ground Utilities")]
        AboveGroundUtilities,
        [EnumMember(Value = "CABLEA")]
        [Description("Cable Available")]
        CableAvailable,
        [EnumMember(Value = "EAVL")]
        [Description("Electricity Available")]
        ElectricityAvailable,
        [EnumMember(Value = "INTRNTC")]
        [Description("Internet-Cable")]
        InternetCable,
        [EnumMember(Value = "INTRNTF")]
        [Description("Internet-Fiber")]
        InternetFiber,
        [EnumMember(Value = "INTRNTO")]
        [Description("Internet-Satellite/Other")]
        InternetSatelliteOther,
        [EnumMember(Value = "GAS")]
        [Description("Natural Gas Available")]
        NaturalGasAvailable,
        [EnumMember(Value = "PHAV")]
        [Description("Phone Available")]
        PhoneAvailable,
        [EnumMember(Value = "PROPN")]
        [Description("Propane")]
        Propane,
        [EnumMember(Value = "SAVAL")]
        [Description("Sewer Available")]
        SewerAvailable,
        [EnumMember(Value = "SCONN")]
        [Description("Sewer Connected")]
        SewerConnected,
        [EnumMember(Value = "SOLAR")]
        [Description("Solar")]
        Solar,
        [EnumMember(Value = "UDUTL")]
        [Description("Underground Utilities")]
        UndergroundUtilities,
        [EnumMember(Value = "WTRAV")]
        [Description("Water Available")]
        WaterAvailable,
        [EnumMember(Value = "WTRCN")]
        [Description("Water Connected")]
        WaterConnected,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
