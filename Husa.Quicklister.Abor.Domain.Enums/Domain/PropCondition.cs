namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum PropCondition
    {
        [EnumMember(Value = "TD")]
        [Description("Tear Down")]
        TearDown,
        [EnumMember(Value = "TBD")]
        [Description("To Be Built")]
        ToBeBuilt,
        [EnumMember(Value = "UC")]
        [Description("Under Construction")]
        UnderConstruction,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
    }
}
