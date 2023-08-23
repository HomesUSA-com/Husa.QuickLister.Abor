namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum UtilitiesDescription
    {
        [EnumMember(Value = "AboveGroundUtilities")]
        [Description("Above Ground Utilities")]
        AboveGroundUtilities,
        [EnumMember(Value = "CableAvailable")]
        [Description("Cable Available")]
        CableAvailable,
        [EnumMember(Value = "ElectricityAvailable")]
        [Description("Electricity Available")]
        ElectricityAvailable,
        [EnumMember(Value = "InternetCable")]
        [Description("Internet-Cable")]
        InternetCable,
        [EnumMember(Value = "InternetFiber")]
        [Description("Internet-Fiber")]
        InternetFiber,
        [EnumMember(Value = "InternetSatelliteOther")]
        [Description("Internet-Satellite/Other")]
        InternetSatelliteOther,
        [EnumMember(Value = "NaturalGasAvailable")]
        [Description("Natural Gas Available")]
        NaturalGasAvailable,
        [EnumMember(Value = "PhoneAvailable")]
        [Description("Phone Available")]
        PhoneAvailable,
        [EnumMember(Value = "Propane")]
        [Description("Propane")]
        Propane,
        [EnumMember(Value = "SewerAvailable")]
        [Description("Sewer Available")]
        SewerAvailable,
        [EnumMember(Value = "SewerConnected")]
        [Description("Sewer Connected")]
        SewerConnected,
        [EnumMember(Value = "Solar")]
        [Description("Solar")]
        Solar,
        [EnumMember(Value = "UndergroundUtilities")]
        [Description("Underground Utilities")]
        UndergroundUtilities,
        [EnumMember(Value = "WaterAvailable")]
        [Description("Water Available")]
        WaterAvailable,
        [EnumMember(Value = "WaterConnected")]
        [Description("Water Connected")]
        WaterConnected,
    }
}
