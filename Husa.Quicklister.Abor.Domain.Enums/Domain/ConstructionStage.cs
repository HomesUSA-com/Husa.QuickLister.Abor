namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum ConstructionStage
    {
        [EnumMember(Value = "NWCONSTR")]
        [Description("Complete")]
        Complete,
        [EnumMember(Value = "TBD")]
        [Description("To Be Built")]
        ToBeBuilt,
        [EnumMember(Value = "UC")]
        [Description("Incomplete")]
        Incomplete,
    }
}
