namespace Husa.Quicklister.Abor.Domain.Enums.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public enum Appliances
    {
        [EnumMember(Value = "BFRDG")]
        [Description("Bar Fridge")]
        BarFridge,
        [EnumMember(Value = "BIEO")]
        [Description("Built-In Electric Oven")]
        BuiltInElectricOven,
        [EnumMember(Value = "BIER")]
        [Description("Built-In Electric Range")]
        BuiltInElectricRange,
        [EnumMember(Value = "BIFRZ")]
        [Description("Built-In Freezer")]
        BuiltInFreezer,
        [EnumMember(Value = "BIGO")]
        [Description("Built-In Gas Oven")]
        BuiltInGasOven,
        [EnumMember(Value = "BIGR")]
        [Description("Built-In Gas Range")]
        BuiltInGasRange,
        [EnumMember(Value = "BIOVN")]
        [Description("Built-In Oven(s)")]
        BuiltInOven,
        [EnumMember(Value = "BIRNG")]
        [Description("Built-In Range")]
        BuiltInRange,
        [EnumMember(Value = "BIFRDG")]
        [Description("Built-In Refrigerator")]
        BuiltInRefrigerator,
        [EnumMember(Value = "CONVE")]
        [Description("Convection Oven")]
        ConvectionOven,
        [EnumMember(Value = "CKTP")]
        [Description("Cooktop")]
        Cooktop,
        [EnumMember(Value = "DW")]
        [Description("Dishwasher")]
        Dishwasher,
        [EnumMember(Value = "DSPSL")]
        [Description("Disposal")]
        Disposal,
        [EnumMember(Value = "DWNDF")]
        [Description("Down Draft")]
        DownDraft,
        [EnumMember(Value = "DRY")]
        [Description("Dryer")]
        Dryer,
        [EnumMember(Value = "CKTPE")]
        [Description("Electric Cooktop")]
        ElectricCooktop,
        [EnumMember(Value = "ERNG")]
        [Description("Electric Range")]
        ElectricRange,
        [EnumMember(Value = "ENGAP")]
        [Description("ENERGY STAR Qualified Appliances")]
        EnergyStarQualifiedAppliances,
        [EnumMember(Value = "EQDWSH")]
        [Description("ENERGY STAR Qualified Dishwasher")]
        EnergyStarQualifiedDishwasher,
        [EnumMember(Value = "EQDRYR")]
        [Description("ENERGY STAR Qualified Dryer")]
        EnergyStarQualifiedDryer,
        [EnumMember(Value = "EQFR")]
        [Description("ENERGY STAR Qualified Freezer")]
        EnergyStarQualifiedFreezer,
        [EnumMember(Value = "EQFZ")]
        [Description("ENERGY STAR Qualified Refrigerator")]
        EnergyStarQualifiedRefrigerator,
        [EnumMember(Value = "EQWSH")]
        [Description("ENERGY STAR Qualified Washer")]
        EnergyStarQualifiedWasher,
        [EnumMember(Value = "EQWHTR")]
        [Description("ENERGY STAR Qualified Water Heater")]
        EnergyStarQualifiedWaterHeater,
        [EnumMember(Value = "EXFEN")]
        [Description("Exhaust Fan")]
        ExhaustFan,
        [EnumMember(Value = "CCKT")]
        [Description("Gas Cooktop")]
        GasCooktop,
        [EnumMember(Value = "GRNG")]
        [Description("Gas Range")]
        GasRange,
        [EnumMember(Value = "IHWTR")]
        [Description("Instant Hot Water")]
        InstantHotWater,
        [EnumMember(Value = "MCWOV")]
        [Description("Microwave")]
        Microwave,
        [EnumMember(Value = "None")]
        [Description("None")]
        None,
        [EnumMember(Value = "Oven")]
        [Description("Oven")]
        Oven,
        [EnumMember(Value = "EOVN")]
        [Description("Oven Electric")]
        OvenElectric,
        [EnumMember(Value = "FSEOV")]
        [Description("Oven Free-Standing Electric")]
        OvenFreeStandingElectric,
        [EnumMember(Value = "FSGOVN")]
        [Description("Oven Free-Standing Gas")]
        OvenFreeStandingGas,
        [EnumMember(Value = "GOVN")]
        [Description("Oven Gas")]
        OvenGas,
        [EnumMember(Value = "DBLOV")]
        [Description("Oven-Double")]
        OvenDouble,
        [EnumMember(Value = "PICMK")]
        [Description("Plumbed For Ice Maker")]
        PlumbedForIceMaker,
        [EnumMember(Value = "Range")]
        [Description("Range")]
        Range,
        [EnumMember(Value = "FSERN")]
        [Description("Range Free Standing Electric")]
        RangeFreeStandingElectric,
        [EnumMember(Value = "FSRNG")]
        [Description("Range Free-Standing")]
        RangeFreeStanding,
        [EnumMember(Value = "FSGRNG")]
        [Description("Range Free-Standing Gas")]
        RangeFreeStandingGas,
        [EnumMember(Value = "HOOD")]
        [Description("Range Hood")]
        RangeHood,
        [EnumMember(Value = "REF")]
        [Description("Refrigerator")]
        Refrigerator,
        [EnumMember(Value = "FSFRDGE")]
        [Description("Refrigerator Free-Standing")]
        RefrigeratorFreeStanding,
        [EnumMember(Value = "SRMRKS")]
        [Description("See Remarks")]
        SeeRemarks,
        [EnumMember(Value = "SCLOV")]
        [Description("Self Cleaning Oven")]
        SelfCleaningOven,
        [EnumMember(Value = "SHTWT")]
        [Description("Solar Hot Water")]
        SolarHotWater,
        [EnumMember(Value = "SSTL")]
        [Description("Stainless Steel Appliance(s)")]
        StainlessSteelAppliance,
        [EnumMember(Value = "TNKWH")]
        [Description("Tankless Water Heater")]
        TanklessWaterHeater,
        [EnumMember(Value = "TRCMP")]
        [Description("Trash Compactor")]
        TrashCompactor,
        [EnumMember(Value = "VXHFN")]
        [Description("Vented Exhaust Fan")]
        VentedExhaustFan,
        [EnumMember(Value = "WD")]
        [Description("Washer Dryer")]
        WasherDryer,
        [EnumMember(Value = "EWHTR")]
        [Description("Water Heater-Electric")]
        WaterHeaterElectric,
        [EnumMember(Value = "GWHTR")]
        [Description("Water Heater-Gas")]
        WaterHeaterGas,
    }
}
