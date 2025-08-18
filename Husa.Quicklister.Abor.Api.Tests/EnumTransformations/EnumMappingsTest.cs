namespace Husa.Quicklister.Abor.Api.Tests.EnumTransformations
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Mappings.EnumTransformations;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Xunit;
    using Trestle = Husa.Downloader.CTX.Domain.Enums;

    [ExcludeFromCodeCoverage]
    public class EnumMappingsTest
    {
        [Theory]
        [InlineData(Trestle.Levels.One, Stories.One)]
        [InlineData(Trestle.Levels.OneAndOneHalf, Stories.OnePointFive)]
        [InlineData(Trestle.Levels.Two, Stories.Two)]
        [InlineData(Trestle.Levels.ThreeOrMore, Stories.ThreePlus)]
        [InlineData(Trestle.Levels.MultiLevelUnit, Stories.MultiLevel)]
        [InlineData((Trestle.Levels)999, null)]
        public void ToStories_ValidLevels_ReturnsExpectedStories(Trestle.Levels input, Stories? expected)
        {
            // Act
            var result = EnumMappings.ToStories(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToStories_NullInput_ReturnsNull()
        {
            // Arrange
            Trestle.Levels? input = null;

            // Act
            var result = EnumMappings.ToStories(input);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.AssociationFeeIncludes.CableTv, HoaIncludes.Cable)]
        [InlineData(Trestle.AssociationFeeIncludes.CommonAreaMaintenance, HoaIncludes.CommonAreasMaintenance)]
        [InlineData(Trestle.AssociationFeeIncludes.MaintenanceGrounds, HoaIncludes.MaintenanceGrounds)]
        [InlineData(Trestle.AssociationFeeIncludes.MaintenanceStructure, HoaIncludes.MaintenanceStructure)]
        [InlineData(Trestle.AssociationFeeIncludes.Internet, HoaIncludes.Internet)]
        [InlineData(Trestle.AssociationFeeIncludes.Security, HoaIncludes.Security)]
        [InlineData(Trestle.AssociationFeeIncludes.SeeRemarks, HoaIncludes.SeeRemarks)]
        public void ConvertsToHoaIncludesCorrectly(Trestle.AssociationFeeIncludes input, HoaIncludes? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToHoaIncludesReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.AssociationFeeIncludes)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.PropertyType.Residential, ListType.Residential)]
        [InlineData(Trestle.PropertyType.CommercialSale, ListType.Residential)]
        [InlineData(Trestle.PropertyType.Land, ListType.Lots)]
        [InlineData(Trestle.PropertyType.ResidentialLease, ListType.Lease)]
        [InlineData(Trestle.PropertyType.CommercialLease, ListType.Lease)]
        public void ConvertsToListTypeCorrectly(Trestle.PropertyType input, ListType? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToListTypeReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.PropertyType)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.MlsStatus.Active, MarketStatuses.Active)]
        [InlineData(Trestle.MlsStatus.ActiveUnderContract, MarketStatuses.ActiveUnderContract)]
        [InlineData(Trestle.MlsStatus.Canceled, MarketStatuses.Canceled)]
        [InlineData(Trestle.MlsStatus.Withdrawn, MarketStatuses.Canceled)]
        [InlineData(Trestle.MlsStatus.Closed, MarketStatuses.Closed)]
        [InlineData(Trestle.MlsStatus.Hold, MarketStatuses.Hold)]
        [InlineData(Trestle.MlsStatus.Pending, MarketStatuses.Pending)]
        public void ConvertsToMarketStatusesCorrectly(Trestle.MlsStatus input, MarketStatuses? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToStatusesThrowsArgumentException()
        {
            // Arrange
            var unknownInput = (Trestle.MlsStatus)100;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => EnumMappings.ToAborEnum(unknownInput));
        }

        [Theory]
        [InlineData(Trestle.StreetSuffix.Alley, StreetType.ALY)]
        [InlineData(Trestle.StreetSuffix.Arcade, StreetType.ARC)]
        [InlineData(Trestle.StreetSuffix.Avenue, StreetType.AVE)]
        [InlineData(Trestle.StreetSuffix.Beach, StreetType.BCH)]
        [InlineData(Trestle.StreetSuffix.Bend, StreetType.BND)]
        [InlineData(Trestle.StreetSuffix.Bluff, StreetType.BLF)]
        [InlineData(Trestle.StreetSuffix.Bluffs, StreetType.BLFS)]
        [InlineData(Trestle.StreetSuffix.Boulevard, StreetType.BLVD)]
        [InlineData(Trestle.StreetSuffix.Branch, StreetType.BR)]
        [InlineData(Trestle.StreetSuffix.Bridge, StreetType.BRG)]
        [InlineData(Trestle.StreetSuffix.Brook, StreetType.BRK)]
        [InlineData(Trestle.StreetSuffix.Bypass, StreetType.BYP)]
        [InlineData(Trestle.StreetSuffix.Camp, StreetType.CP)]
        [InlineData(Trestle.StreetSuffix.Canyon, StreetType.Canyon)]
        [InlineData(Trestle.StreetSuffix.Cape, StreetType.Cape)]
        [InlineData(Trestle.StreetSuffix.Causeway, StreetType.CSWY)]
        [InlineData(Trestle.StreetSuffix.Circle, StreetType.CIR)]
        [InlineData(Trestle.StreetSuffix.Cliff, StreetType.CLF)]
        [InlineData(Trestle.StreetSuffix.Cliffs, StreetType.CLFS)]
        [InlineData(Trestle.StreetSuffix.Club, StreetType.CLB)]
        [InlineData(Trestle.StreetSuffix.Common, StreetType.CMN)]
        [InlineData(Trestle.StreetSuffix.Corner, StreetType.COR)]
        [InlineData(Trestle.StreetSuffix.Corners, StreetType.CORS)]
        [InlineData(Trestle.StreetSuffix.Court, StreetType.CT)]
        [InlineData(Trestle.StreetSuffix.Cove, StreetType.Cove)]
        [InlineData(Trestle.StreetSuffix.Creek, StreetType.CRK)]
        [InlineData(Trestle.StreetSuffix.Crescent, StreetType.CRSE)]
        [InlineData(Trestle.StreetSuffix.Crest, StreetType.CRST)]
        [InlineData(Trestle.StreetSuffix.Crossing, StreetType.XING)]
        [InlineData(Trestle.StreetSuffix.Curve, StreetType.CURV)]
        [InlineData(Trestle.StreetSuffix.Dale, StreetType.DL)]
        [InlineData(Trestle.StreetSuffix.Dam, StreetType.DM)]
        [InlineData(Trestle.StreetSuffix.Drive, StreetType.DR)]
        [InlineData(Trestle.StreetSuffix.Estate, StreetType.EST)]
        [InlineData(Trestle.StreetSuffix.Estates, StreetType.ESTS)]
        [InlineData(Trestle.StreetSuffix.Expressway, StreetType.EXPY)]
        [InlineData(Trestle.StreetSuffix.Extension, StreetType.EXT)]
        [InlineData(Trestle.StreetSuffix.Fall, StreetType.FALL)]
        [InlineData(Trestle.StreetSuffix.Falls, StreetType.FLS)]
        [InlineData(Trestle.StreetSuffix.Field, StreetType.FLD)]
        [InlineData(Trestle.StreetSuffix.Fields, StreetType.FLDS)]
        [InlineData(Trestle.StreetSuffix.Flat, StreetType.FLT)]
        [InlineData(Trestle.StreetSuffix.Flats, StreetType.FLTS)]
        [InlineData(Trestle.StreetSuffix.Ford, StreetType.FRD)]
        [InlineData(Trestle.StreetSuffix.Forest, StreetType.FRST)]
        [InlineData(Trestle.StreetSuffix.Forge, StreetType.FRG)]
        [InlineData(Trestle.StreetSuffix.Fork, StreetType.FRK)]
        [InlineData(Trestle.StreetSuffix.Forks, StreetType.FRKS)]
        [InlineData(Trestle.StreetSuffix.Garden, StreetType.GRN)]
        [InlineData(Trestle.StreetSuffix.Gardens, StreetType.GRNS)]
        [InlineData(Trestle.StreetSuffix.Glen, StreetType.GLN)]
        [InlineData(Trestle.StreetSuffix.Green, StreetType.GRN)]
        [InlineData(Trestle.StreetSuffix.Grove, StreetType.GRV)]
        [InlineData(Trestle.StreetSuffix.Harbor, StreetType.HBR)]
        [InlineData(Trestle.StreetSuffix.Haven, StreetType.HVN)]
        [InlineData(Trestle.StreetSuffix.Heights, StreetType.HTS)]
        [InlineData(Trestle.StreetSuffix.Highway, StreetType.HWY)]
        [InlineData(Trestle.StreetSuffix.Hill, StreetType.HL)]
        [InlineData(Trestle.StreetSuffix.Hills, StreetType.HLS)]
        [InlineData(Trestle.StreetSuffix.Hollow, StreetType.HOLW)]
        [InlineData(Trestle.StreetSuffix.Island, StreetType.IS)]
        [InlineData(Trestle.StreetSuffix.Isle, StreetType.ISLE)]
        [InlineData(Trestle.StreetSuffix.Junction, StreetType.JCT)]
        [InlineData(Trestle.StreetSuffix.Key, StreetType.KY)]
        [InlineData(Trestle.StreetSuffix.Knoll, StreetType.KNL)]
        [InlineData(Trestle.StreetSuffix.Knolls, StreetType.KNLS)]
        [InlineData(Trestle.StreetSuffix.Lake, StreetType.LK)]
        [InlineData(Trestle.StreetSuffix.Lakes, StreetType.LKS)]
        [InlineData(Trestle.StreetSuffix.Land, StreetType.LAND)]
        [InlineData(Trestle.StreetSuffix.Landing, StreetType.LAND)]
        [InlineData(Trestle.StreetSuffix.Lane, StreetType.LN)]
        [InlineData(Trestle.StreetSuffix.Light, StreetType.LGT)]
        [InlineData(Trestle.StreetSuffix.Lodge, StreetType.LDG)]
        [InlineData(Trestle.StreetSuffix.Loop, StreetType.LOOP)]
        [InlineData(Trestle.StreetSuffix.Manor, StreetType.MNR)]
        [InlineData(Trestle.StreetSuffix.Meadow, StreetType.MDW)]
        [InlineData(Trestle.StreetSuffix.Meadows, StreetType.MDWS)]
        [InlineData(Trestle.StreetSuffix.Mews, StreetType.MEWS)]
        [InlineData(Trestle.StreetSuffix.Mill, StreetType.ML)]
        [InlineData(Trestle.StreetSuffix.Mills, StreetType.MLS)]
        [InlineData(Trestle.StreetSuffix.Mount, StreetType.MT)]
        [InlineData(Trestle.StreetSuffix.Mountain, StreetType.MTN)]
        [InlineData(Trestle.StreetSuffix.Orchard, StreetType.ORCH)]
        [InlineData(Trestle.StreetSuffix.Oval, StreetType.OVAL)]
        [InlineData(Trestle.StreetSuffix.Park, StreetType.PARK)]
        [InlineData(Trestle.StreetSuffix.Parkway, StreetType.PKWY)]
        [InlineData(Trestle.StreetSuffix.Pass, StreetType.PASS)]
        [InlineData(Trestle.StreetSuffix.Path, StreetType.PATH)]
        [InlineData(Trestle.StreetSuffix.Pike, StreetType.PIKE)]
        [InlineData(Trestle.StreetSuffix.Pine, StreetType.PINE)]
        [InlineData(Trestle.StreetSuffix.Place, StreetType.PL)]
        [InlineData(Trestle.StreetSuffix.Plain, StreetType.PLN)]
        [InlineData(Trestle.StreetSuffix.Plains, StreetType.PLNS)]
        [InlineData(Trestle.StreetSuffix.Plaza, StreetType.PLZ)]
        [InlineData(Trestle.StreetSuffix.Point, StreetType.PT)]
        [InlineData(Trestle.StreetSuffix.Port, StreetType.PRT)]
        [InlineData(Trestle.StreetSuffix.Prairie, StreetType.PR)]
        [InlineData(Trestle.StreetSuffix.Ranch, StreetType.RNCH)]
        [InlineData(Trestle.StreetSuffix.Rapids, StreetType.RPDS)]
        [InlineData(Trestle.StreetSuffix.Ridge, StreetType.RDG)]
        [InlineData(Trestle.StreetSuffix.River, StreetType.RIV)]
        [InlineData(Trestle.StreetSuffix.Road, StreetType.RD)]
        [InlineData(Trestle.StreetSuffix.Row, StreetType.ROW)]
        [InlineData(Trestle.StreetSuffix.Run, StreetType.RUN)]
        [InlineData(Trestle.StreetSuffix.Shores, StreetType.SHR)]
        [InlineData(Trestle.StreetSuffix.Skyway, StreetType.SKWY)]
        [InlineData(Trestle.StreetSuffix.Spring, StreetType.SPG)]
        [InlineData(Trestle.StreetSuffix.Springs, StreetType.SPGS)]
        [InlineData(Trestle.StreetSuffix.Spur, StreetType.SPUR)]
        [InlineData(Trestle.StreetSuffix.Square, StreetType.SQ)]
        [InlineData(Trestle.StreetSuffix.Station, StreetType.STA)]
        [InlineData(Trestle.StreetSuffix.Stream, StreetType.STRM)]
        [InlineData(Trestle.StreetSuffix.Street, StreetType.ST)]
        [InlineData(Trestle.StreetSuffix.Summit, StreetType.SMT)]
        [InlineData(Trestle.StreetSuffix.Terrace, StreetType.TER)]
        [InlineData(Trestle.StreetSuffix.Trace, StreetType.TRCE)]
        [InlineData(Trestle.StreetSuffix.Track, StreetType.TRAK)]
        [InlineData(Trestle.StreetSuffix.Trail, StreetType.TRL)]
        [InlineData(Trestle.StreetSuffix.Valley, StreetType.VLY)]
        [InlineData(Trestle.StreetSuffix.View, StreetType.VW)]
        [InlineData(Trestle.StreetSuffix.Village, StreetType.VLG)]
        [InlineData(Trestle.StreetSuffix.Vista, StreetType.VIS)]
        [InlineData(Trestle.StreetSuffix.Walk, StreetType.WALK)]
        [InlineData(Trestle.StreetSuffix.Way, StreetType.WAY)]
        [InlineData(Trestle.StreetSuffix.Wells, StreetType.WLS)]
        public void ConvertsToStreetTypeCorrectly(Trestle.StreetSuffix input, StreetType? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToStreetTypeReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.StreetSuffix)300;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.StreetDirection.E, StreetDirPrefix.East)]
        [InlineData(Trestle.StreetDirection.N, StreetDirPrefix.North)]
        [InlineData(Trestle.StreetDirection.NE, StreetDirPrefix.NorthEast)]
        [InlineData(Trestle.StreetDirection.NW, StreetDirPrefix.NorthWest)]
        [InlineData(Trestle.StreetDirection.S, StreetDirPrefix.South)]
        [InlineData(Trestle.StreetDirection.SE, StreetDirPrefix.SouthEast)]
        [InlineData(Trestle.StreetDirection.SW, StreetDirPrefix.SouthWest)]
        [InlineData(Trestle.StreetDirection.W, StreetDirPrefix.West)]
        public void ConvertsToStreetDirPrefixCorrectly(Trestle.StreetDirection input, StreetDirPrefix? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToStreetDirPrefixReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.StreetDirection)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.StateOrProvince.AL, States.Alabama)]
        [InlineData(Trestle.StateOrProvince.AK, States.Alaska)]
        [InlineData(Trestle.StateOrProvince.AR, States.Arkansas)]
        [InlineData(Trestle.StateOrProvince.AZ, States.Arizona)]
        [InlineData(Trestle.StateOrProvince.CA, States.California)]
        [InlineData(Trestle.StateOrProvince.CO, States.Colorado)]
        [InlineData(Trestle.StateOrProvince.CT, States.Connecticut)]
        [InlineData(Trestle.StateOrProvince.DC, States.DC)]
        [InlineData(Trestle.StateOrProvince.DE, States.Delaware)]
        [InlineData(Trestle.StateOrProvince.FL, States.Florida)]
        [InlineData(Trestle.StateOrProvince.GA, States.Georgia)]
        [InlineData(Trestle.StateOrProvince.HI, States.Hawaii)]
        [InlineData(Trestle.StateOrProvince.IA, States.Iowa)]
        [InlineData(Trestle.StateOrProvince.ID, States.Idaho)]
        [InlineData(Trestle.StateOrProvince.IL, States.Illinois)]
        [InlineData(Trestle.StateOrProvince.IN, States.Indiana)]
        [InlineData(Trestle.StateOrProvince.KS, States.Kansas)]
        [InlineData(Trestle.StateOrProvince.KY, States.Kentucky)]
        [InlineData(Trestle.StateOrProvince.LA, States.Louisiana)]
        [InlineData(Trestle.StateOrProvince.MA, States.Massachusetts)]
        [InlineData(Trestle.StateOrProvince.MD, States.Maryland)]
        [InlineData(Trestle.StateOrProvince.ME, States.Maine)]
        [InlineData(Trestle.StateOrProvince.MI, States.Michigan)]
        [InlineData(Trestle.StateOrProvince.MN, States.Minnesota)]
        [InlineData(Trestle.StateOrProvince.MO, States.Missouri)]
        [InlineData(Trestle.StateOrProvince.MS, States.Mississippi)]
        [InlineData(Trestle.StateOrProvince.MT, States.Montana)]
        [InlineData(Trestle.StateOrProvince.NC, States.NorthCarolina)]
        [InlineData(Trestle.StateOrProvince.ND, States.NorthDakota)]
        [InlineData(Trestle.StateOrProvince.NE, States.Nebraska)]
        [InlineData(Trestle.StateOrProvince.NH, States.NewHampshire)]
        [InlineData(Trestle.StateOrProvince.NJ, States.NewJersey)]
        [InlineData(Trestle.StateOrProvince.NM, States.NewMexico)]
        [InlineData(Trestle.StateOrProvince.NV, States.Nevada)]
        [InlineData(Trestle.StateOrProvince.NY, States.NewYork)]
        [InlineData(Trestle.StateOrProvince.OK, States.Oklahoma)]
        [InlineData(Trestle.StateOrProvince.OH, States.Ohio)]
        [InlineData(Trestle.StateOrProvince.OR, States.Oregon)]
        [InlineData(Trestle.StateOrProvince.PA, States.Pennsylvania)]
        [InlineData(Trestle.StateOrProvince.RI, States.RhodeIsland)]
        [InlineData(Trestle.StateOrProvince.SC, States.SouthCarolina)]
        [InlineData(Trestle.StateOrProvince.SD, States.SouthDakota)]
        [InlineData(Trestle.StateOrProvince.TN, States.Tennessee)]
        [InlineData(Trestle.StateOrProvince.TX, States.Texas)]
        [InlineData(Trestle.StateOrProvince.UT, States.Utah)]
        [InlineData(Trestle.StateOrProvince.VA, States.Virginia)]
        [InlineData(Trestle.StateOrProvince.VT, States.Vermont)]
        [InlineData(Trestle.StateOrProvince.WA, States.Washington)]
        [InlineData(Trestle.StateOrProvince.WI, States.Wisconsin)]
        [InlineData(Trestle.StateOrProvince.WV, States.WestVirginia)]
        [InlineData(Trestle.StateOrProvince.WY, States.Wyoming)]
        public void ConvertsToStatesCorrectly(Trestle.StateOrProvince input, States? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToStatesReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.StateOrProvince)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.PropertySubType.Condominium, PropertySubType.Condominium)]
        [InlineData(Trestle.PropertySubType.SingleFamilyResidence, PropertySubType.SingleFamilyResidence)]
        [InlineData(Trestle.PropertySubType.Townhouse, PropertySubType.Townhouse)]
        public void ConvertsToPropertySubTypeCorrectly(Trestle.PropertySubType input, PropertySubType? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToPropertySubTypeReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.PropertySubType)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.LotFeatures.BacksToGreenbeltPark, LotDescription.BackstoGreenbeltPark)]
        [InlineData(Trestle.LotFeatures.BackYard, LotDescription.BackYard)]
        [InlineData(Trestle.LotFeatures.Bluff, LotDescription.Bluff)]
        [InlineData(Trestle.LotFeatures.CityLot, LotDescription.CityLot)]
        [InlineData(Trestle.LotFeatures.CloseToClubhouse, LotDescription.ClosetoClubhouse)]
        [InlineData(Trestle.LotFeatures.CornerLot, LotDescription.CornerLot)]
        [InlineData(Trestle.LotFeatures.CulDeSac, LotDescription.CulDeSac)]
        [InlineData(Trestle.LotFeatures.FewTrees, LotDescription.FewTree)]
        [InlineData(Trestle.LotFeatures.FrontYard, LotDescription.FrontYard)]
        [InlineData(Trestle.LotFeatures.NearGolfCourse, LotDescription.NearGolfCourse)]
        [InlineData(Trestle.LotFeatures.GentleSloping, LotDescription.GentleSloping)]
        [InlineData(Trestle.LotFeatures.InteriorLot, LotDescription.InteriorLot)]
        [InlineData(Trestle.LotFeatures.IrregularLot, LotDescription.IrregularLot)]
        [InlineData(Trestle.LotFeatures.TreesLargeSize, LotDescription.TreesLarge)]
        [InlineData(Trestle.LotFeatures.Level, LotDescription.Level)]
        [InlineData(Trestle.LotFeatures.Landscaped, LotDescription.Landscaped)]
        [InlineData(Trestle.LotFeatures.TreesMediumSize, LotDescription.TreesMedium)]
        [InlineData(Trestle.LotFeatures.SprinklersManual, LotDescription.SprinklerManual)]
        [InlineData(Trestle.LotFeatures.ModerateTrees, LotDescription.TreesModerate)]
        [InlineData(Trestle.LotFeatures.None, LotDescription.None)]
        [InlineData(Trestle.LotFeatures.NativePlants, LotDescription.NativePlants)]
        [InlineData(Trestle.LotFeatures.OnGolfCourse, LotDescription.OnGolfCourse)]
        [InlineData(Trestle.LotFeatures.PieShapedLot, LotDescription.PieShapedLot)]
        [InlineData(Trestle.LotFeatures.PublicMaintainedRoad, LotDescription.PublicMaintainedRoad)]
        [InlineData(Trestle.LotFeatures.NearPublicTransit, LotDescription.NearPublicTransit)]
        [InlineData(Trestle.LotFeatures.SlopedDown, LotDescription.SlopedDown)]
        [InlineData(Trestle.LotFeatures.SlopedUp, LotDescription.SlopedUp)]
        [InlineData(Trestle.LotFeatures.TreesSmallSize, LotDescription.TreesSmall)]
        [InlineData(Trestle.LotFeatures.SprinklersAutomatic, LotDescription.SprinklerAutomatic)]
        [InlineData(Trestle.LotFeatures.SprinklersInFront, LotDescription.SprinklerInFront)]
        [InlineData(Trestle.LotFeatures.SprinklersInGround, LotDescription.SprinklerInground)]
        [InlineData(Trestle.LotFeatures.SprinklersPartial, LotDescription.SprinklerPartial)]
        [InlineData(Trestle.LotFeatures.SprinklerRainSensor, LotDescription.SprinklerRainSensor)]
        [InlineData(Trestle.LotFeatures.SprinklersInRear, LotDescription.SprinklerBackYard)]
        [InlineData(Trestle.LotFeatures.SprinklersOnSide, LotDescription.SprinklerSideYard)]
        [InlineData(Trestle.LotFeatures.Views, LotDescription.View)]
        [InlineData(Trestle.LotFeatures.ManyTrees, LotDescription.TreesMany)]
        [InlineData(Trestle.LotFeatures.ZeroLotLine, LotDescription.ZeroLotLine)]
        public void ConvertsToLotDescriptionCorrectly(Trestle.LotFeatures input, LotDescription? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToLotDescriptionReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.LotFeatures)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.DirectionFaces.East, HomeFaces.East)]
        [InlineData(Trestle.DirectionFaces.North, HomeFaces.North)]
        [InlineData(Trestle.DirectionFaces.Northeast, HomeFaces.NorthEast)]
        [InlineData(Trestle.DirectionFaces.Northwest, HomeFaces.NorthWest)]
        [InlineData(Trestle.DirectionFaces.South, HomeFaces.South)]
        [InlineData(Trestle.DirectionFaces.Southeast, HomeFaces.SouthEast)]
        [InlineData(Trestle.DirectionFaces.Southwest, HomeFaces.SouthWest)]
        [InlineData(Trestle.DirectionFaces.West, HomeFaces.West)]
        public void ConvertsToHomeFacesCorrectly(Trestle.DirectionFaces input, HomeFaces? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToHomeFacesReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.DirectionFaces)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.FoundationDetails.PillarPostPier, Foundation.PillarPostPier)]
        [InlineData(Trestle.FoundationDetails.Slab, Foundation.Slab)]
        public void ConvertsToFoundationCorrectly(Trestle.FoundationDetails input, Foundation? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToFoundationReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.FoundationDetails)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.Roof.BuiltUp, RoofDescription.BuiltUp)]
        [InlineData(Trestle.Roof.Composition, RoofDescription.Composition)]
        [InlineData(Trestle.Roof.Copper, RoofDescription.Copper)]
        [InlineData(Trestle.Roof.FlatTile, RoofDescription.FlatTile)]
        [InlineData(Trestle.Roof.GreenRoof, RoofDescription.GreenRoof)]
        [InlineData(Trestle.Roof.Metal, RoofDescription.Metal)]
        [InlineData(Trestle.Roof.Shingle, RoofDescription.Shingle)]
        [InlineData(Trestle.Roof.Slate, RoofDescription.Slate)]
        [InlineData(Trestle.Roof.SpanishTile, RoofDescription.SpanishTile)]
        [InlineData(Trestle.Roof.Synthetic, RoofDescription.Synthetic)]
        [InlineData(Trestle.Roof.Tile, RoofDescription.Tile)]
        [InlineData(Trestle.Roof.Wood, RoofDescription.Wood)]
        public void ConvertsToRoofDescriptionCorrectly(Trestle.Roof input, RoofDescription? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToRoofDescriptionReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.Roof)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.ConstructionMaterials.Adobe, ConstructionMaterials.Adobe)]
        [InlineData(Trestle.ConstructionMaterials.Asphalt, ConstructionMaterials.Asphalt)]
        [InlineData(Trestle.ConstructionMaterials.AtticCrawlHatchwaysInsulated, ConstructionMaterials.AtticCrawlHatchwaysInsulated)]
        [InlineData(Trestle.ConstructionMaterials.BlownInInsulation, ConstructionMaterials.BlownInInsulation)]
        [InlineData(Trestle.ConstructionMaterials.Brick, ConstructionMaterials.Brick)]
        [InlineData(Trestle.ConstructionMaterials.BrickVeneer, ConstructionMaterials.BrickVeneer)]
        [InlineData(Trestle.ConstructionMaterials.Cedar, ConstructionMaterials.Cedar)]
        [InlineData(Trestle.ConstructionMaterials.Clapboard, ConstructionMaterials.ClapBoard)]
        [InlineData(Trestle.ConstructionMaterials.Concrete, ConstructionMaterials.Concrete)]
        [InlineData(Trestle.ConstructionMaterials.Frame, ConstructionMaterials.Frame)]
        [InlineData(Trestle.ConstructionMaterials.Glass, ConstructionMaterials.Glass)]
        [InlineData(Trestle.ConstructionMaterials.HardiplankType, ConstructionMaterials.HardiPlankType)]
        [InlineData(Trestle.ConstructionMaterials.IcatRecessedLighting, ConstructionMaterials.ICatRecessedLighting)]
        [InlineData(Trestle.ConstructionMaterials.InsulatedConcreteForms, ConstructionMaterials.InsulatedConcreteForms)]
        [InlineData(Trestle.ConstructionMaterials.Log, ConstructionMaterials.Log)]
        [InlineData(Trestle.ConstructionMaterials.Masonry, ConstructionMaterials.MasonryAllSides)]
        [InlineData(Trestle.ConstructionMaterials.NaturalBuilding, ConstructionMaterials.NaturalBuilding)]
        [InlineData(Trestle.ConstructionMaterials.RadiantBarrier, ConstructionMaterials.RadiantBarrier)]
        [InlineData(Trestle.ConstructionMaterials.RecycledBioBasedInsulation, ConstructionMaterials.RecycledBioBasedInsulation)]
        [InlineData(Trestle.ConstructionMaterials.RedwoodSiding, ConstructionMaterials.SidingRedwood)]
        [InlineData(Trestle.ConstructionMaterials.WoodSiding, ConstructionMaterials.SidingWood)]
        [InlineData(Trestle.ConstructionMaterials.VinylSiding, ConstructionMaterials.SidingVinyl)]
        [InlineData(Trestle.ConstructionMaterials.SprayFoamInsulation, ConstructionMaterials.SprayFoamInsulation)]
        [InlineData(Trestle.ConstructionMaterials.Stone, ConstructionMaterials.Stone)]
        [InlineData(Trestle.ConstructionMaterials.StoneVeneer, ConstructionMaterials.StoneVeneer)]
        [InlineData(Trestle.ConstructionMaterials.Stucco, ConstructionMaterials.Stucco)]
        [InlineData(Trestle.ConstructionMaterials.SyntheticStucco, ConstructionMaterials.SyntheticStucco)]
        public void ConvertsToConstructionMaterialsCorrectly(Trestle.ConstructionMaterials input, ConstructionMaterials? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToConstructionMaterialsReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.ConstructionMaterials)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.FireplaceFeatures.Bath, FireplaceDescription.Bathroom)]
        [InlineData(Trestle.FireplaceFeatures.Bedroom, FireplaceDescription.Bedroom)]
        [InlineData(Trestle.FireplaceFeatures.Circulating, FireplaceDescription.Circulating)]
        [InlineData(Trestle.FireplaceFeatures.Decorative, FireplaceDescription.Decorative)]
        [InlineData(Trestle.FireplaceFeatures.Den, FireplaceDescription.Den)]
        [InlineData(Trestle.FireplaceFeatures.DoubleSided, FireplaceDescription.DoubleSided)]
        [InlineData(Trestle.FireplaceFeatures.Electric, FireplaceDescription.Electric)]
        [InlineData(Trestle.FireplaceFeatures.EpaCertifiedWoodStove, FireplaceDescription.EPACertifiedWoodStove)]
        [InlineData(Trestle.FireplaceFeatures.FamilyRoom, FireplaceDescription.FamilyRoom)]
        [InlineData(Trestle.FireplaceFeatures.Gas, FireplaceDescription.Gas)]
        [InlineData(Trestle.FireplaceFeatures.GasLog, FireplaceDescription.GasLog)]
        [InlineData(Trestle.FireplaceFeatures.GasStarter, FireplaceDescription.GasStarter)]
        [InlineData(Trestle.FireplaceFeatures.GreatRoom, FireplaceDescription.GreatRoom)]
        [InlineData(Trestle.FireplaceFeatures.LivingRoom, FireplaceDescription.LivingRoom)]
        [InlineData(Trestle.FireplaceFeatures.Masonry, FireplaceDescription.Masonry)]
        [InlineData(Trestle.FireplaceFeatures.Metal, FireplaceDescription.Metal)]
        [InlineData(Trestle.FireplaceFeatures.None, FireplaceDescription.None)]
        [InlineData(Trestle.FireplaceFeatures.Outside, FireplaceDescription.Outside)]
        [InlineData(Trestle.FireplaceFeatures.SeeThrough, FireplaceDescription.SeeThrough)]
        [InlineData(Trestle.FireplaceFeatures.Ventless, FireplaceDescription.Ventless)]
        [InlineData(Trestle.FireplaceFeatures.WoodBurning, FireplaceDescription.WoodBurning)]
        public void ConvertsToFireplaceDescriptionCorrectly(Trestle.FireplaceFeatures input, FireplaceDescription? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToFireplaceDescriptionReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.FireplaceFeatures)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.Flooring.Carpet, Flooring.Carpet)]
        [InlineData(Trestle.Flooring.Concrete, Flooring.Concrete)]
        [InlineData(Trestle.Flooring.Laminate, Flooring.Laminate)]
        [InlineData(Trestle.Flooring.Linoleum, Flooring.Linoleum)]
        [InlineData(Trestle.Flooring.Marble, Flooring.Marble)]
        [InlineData(Trestle.Flooring.Parquet, Flooring.Parquet)]
        [InlineData(Trestle.Flooring.SeeRemarks, Flooring.SeeRemarks)]
        [InlineData(Trestle.Flooring.Slate, Flooring.Slate)]
        [InlineData(Trestle.Flooring.Terrazzo, Flooring.Terrazzo)]
        [InlineData(Trestle.Flooring.Tile, Flooring.Tile)]
        [InlineData(Trestle.Flooring.Vinyl, Flooring.Vinyl)]
        [InlineData(Trestle.Flooring.Wood, Flooring.Wood)]
        public void ConvertsToFlooringCorrectly(Trestle.Flooring input, Flooring? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToFlooringReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.Flooring)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.LaundryFeatures.InKitchen, LaundryLocation.Kitchen)]
        [InlineData(Trestle.LaundryFeatures.LaundryCloset, LaundryLocation.LaundryCloset)]
        [InlineData(Trestle.LaundryFeatures.LaundryRoom, LaundryLocation.LaundryRoom)]
        [InlineData(Trestle.LaundryFeatures.MainLevel, LaundryLocation.MainLevel)]
        [InlineData(Trestle.LaundryFeatures.UpperLevel, LaundryLocation.UpperLevel)]
        public void ConvertsToLaundryLocationCorrectly(Trestle.LaundryFeatures input, LaundryLocation? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToLaundryLocationReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.LaundryFeatures)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.ParkingFeatures.AlleyAccess, GarageDescription.AlleyAccess)]
        [InlineData(Trestle.ParkingFeatures.Attached, GarageDescription.Attached)]
        [InlineData(Trestle.ParkingFeatures.CircularDriveway, GarageDescription.CircularDriveway)]
        [InlineData(Trestle.ParkingFeatures.Concrete, GarageDescription.Concrete)]
        [InlineData(Trestle.ParkingFeatures.Covered, GarageDescription.Covered)]
        [InlineData(Trestle.ParkingFeatures.Detached, GarageDescription.Detached)]
        [InlineData(Trestle.ParkingFeatures.DirectAccess, GarageDescription.DirectAccess)]
        [InlineData(Trestle.ParkingFeatures.DoorMulti, GarageDescription.DoorMulti)]
        [InlineData(Trestle.ParkingFeatures.DoorSingle, GarageDescription.DoorSingle)]
        [InlineData(Trestle.ParkingFeatures.DriveThrough, GarageDescription.DriveThrough)]
        [InlineData(Trestle.ParkingFeatures.Driveway, GarageDescription.Driveway)]
        [InlineData(Trestle.ParkingFeatures.ElectricGate, GarageDescription.ElectricGate)]
        [InlineData(Trestle.ParkingFeatures.GarageDoorOpener, GarageDescription.GarageDoorOpener)]
        [InlineData(Trestle.ParkingFeatures.GarageFacesFront, GarageDescription.GarageFacesFront)]
        [InlineData(Trestle.ParkingFeatures.GarageFacesRear, GarageDescription.GarageFacesRear)]
        [InlineData(Trestle.ParkingFeatures.GarageFacesSide, GarageDescription.GarageFacesSide)]
        [InlineData(Trestle.ParkingFeatures.Gravel, GarageDescription.Gravel)]
        [InlineData(Trestle.ParkingFeatures.InsideEntrance, GarageDescription.InsideEntrance)]
        [InlineData(Trestle.ParkingFeatures.KitchenLevel, GarageDescription.KitchenLevel)]
        [InlineData(Trestle.ParkingFeatures.Lighted, GarageDescription.Lighted)]
        [InlineData(Trestle.ParkingFeatures.Oversized, GarageDescription.Oversized)]
        [InlineData(Trestle.ParkingFeatures.Private, GarageDescription.Private)]
        [InlineData(Trestle.ParkingFeatures.SideBySide, GarageDescription.SideBySide)]
        [InlineData(Trestle.ParkingFeatures.Storage, GarageDescription.Storage)]
        [InlineData(Trestle.ParkingFeatures.Tandem, GarageDescription.Tandem)]
        public void ConvertsToGarageDescriptionCorrectly(Trestle.ParkingFeatures input, GarageDescription? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToGarageDescriptionReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.ParkingFeatures)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.PatioAndPorchFeatures.Awnings, PatioAndPorchFeatures.Awnings)]
        [InlineData(Trestle.PatioAndPorchFeatures.Covered, PatioAndPorchFeatures.Covered)]
        [InlineData(Trestle.PatioAndPorchFeatures.Deck, PatioAndPorchFeatures.Deck)]
        [InlineData(Trestle.PatioAndPorchFeatures.Enclosed, PatioAndPorchFeatures.Enclosed)]
        [InlineData(Trestle.PatioAndPorchFeatures.FrontPorch, PatioAndPorchFeatures.FrontPorch)]
        [InlineData(Trestle.PatioAndPorchFeatures.GlassEnclosed, PatioAndPorchFeatures.GlassEnclosed)]
        [InlineData(Trestle.PatioAndPorchFeatures.MosquitoSystem, PatioAndPorchFeatures.MosquitoSystem)]
        [InlineData(Trestle.PatioAndPorchFeatures.None, PatioAndPorchFeatures.None)]
        [InlineData(Trestle.PatioAndPorchFeatures.Patio, PatioAndPorchFeatures.Patio)]
        [InlineData(Trestle.PatioAndPorchFeatures.Porch, PatioAndPorchFeatures.Porch)]
        [InlineData(Trestle.PatioAndPorchFeatures.RearPorch, PatioAndPorchFeatures.RearPorch)]
        [InlineData(Trestle.PatioAndPorchFeatures.Screened, PatioAndPorchFeatures.Screened)]
        [InlineData(Trestle.PatioAndPorchFeatures.SeeRemarks, PatioAndPorchFeatures.SeeRemarks)]
        [InlineData(Trestle.PatioAndPorchFeatures.SidePorch, PatioAndPorchFeatures.SidePorch)]
        [InlineData(Trestle.PatioAndPorchFeatures.Terrace, PatioAndPorchFeatures.Terrace)]
        [InlineData(Trestle.PatioAndPorchFeatures.WrapAround, PatioAndPorchFeatures.WrapAround)]
        public void ConvertsToPatioAndPorchFeaturesCorrectly(Trestle.PatioAndPorchFeatures input, PatioAndPorchFeatures? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToPatioAndPorchFeaturesReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.PatioAndPorchFeatures)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.InteriorOrRoomFeatures.MultiplePrimarySuites, InteriorFeatures.TwoPrimarySuites)]
        [InlineData(Trestle.InteriorOrRoomFeatures.Bar, InteriorFeatures.Bar)]
        [InlineData(Trestle.InteriorOrRoomFeatures.Bookcases, InteriorFeatures.Bookcases)]
        [InlineData(Trestle.InteriorOrRoomFeatures.BreakfastBar, InteriorFeatures.BreakfastBar)]
        [InlineData(Trestle.InteriorOrRoomFeatures.BuiltInFeatures, InteriorFeatures.BuiltinFeatures)]
        [InlineData(Trestle.InteriorOrRoomFeatures.CeilingFans, InteriorFeatures.CeilingFans)]
        [InlineData(Trestle.InteriorOrRoomFeatures.BeamedCeilings, InteriorFeatures.CeilingsBeamed)]
        [InlineData(Trestle.InteriorOrRoomFeatures.CathedralCeilings, InteriorFeatures.CeilingsCathedral)]
        [InlineData(Trestle.InteriorOrRoomFeatures.CofferedCeilings, InteriorFeatures.CeilingsCoffered)]
        [InlineData(Trestle.InteriorOrRoomFeatures.HighCeilings, InteriorFeatures.CeilingsHigh)]
        [InlineData(Trestle.InteriorOrRoomFeatures.TrayCeilings, InteriorFeatures.CeilingsTray)]
        [InlineData(Trestle.InteriorOrRoomFeatures.VaultedCeilings, InteriorFeatures.CeilingsVaulted)]
        [InlineData(Trestle.InteriorOrRoomFeatures.Chandelier, InteriorFeatures.Chandelier)]
        [InlineData(Trestle.InteriorOrRoomFeatures.CrownMolding, InteriorFeatures.CrownMolding)]
        [InlineData(Trestle.InteriorOrRoomFeatures.DoubleVanity, InteriorFeatures.DoubleVanity)]
        [InlineData(Trestle.InteriorOrRoomFeatures.EatInKitchen, InteriorFeatures.EatinKitchen)]
        [InlineData(Trestle.InteriorOrRoomFeatures.EntranceFoyer, InteriorFeatures.EntranceFoyer)]
        [InlineData(Trestle.InteriorOrRoomFeatures.FrenchDoorsAtriumDoors, InteriorFeatures.FrenchDoors)]
        [InlineData(Trestle.InteriorOrRoomFeatures.HighSpeedInternet, InteriorFeatures.HighSpeedInternet)]
        [InlineData(Trestle.InteriorOrRoomFeatures.InLawFloorplan, InteriorFeatures.InLawFloorplan)]
        [InlineData(Trestle.InteriorOrRoomFeatures.InteriorSteps, InteriorFeatures.InteriorSteps)]
        [InlineData(Trestle.InteriorOrRoomFeatures.KitchenIsland, InteriorFeatures.KitchenIsland)]
        [InlineData(Trestle.InteriorOrRoomFeatures.LowFlowPlumbingFixtures, InteriorFeatures.LowFlowPlumbingFixtures)]
        [InlineData(Trestle.InteriorOrRoomFeatures.MultipleDiningAreas, InteriorFeatures.MultipleDiningAreas)]
        [InlineData(Trestle.InteriorOrRoomFeatures.MultipleLivingAreas, InteriorFeatures.MultipleLivingAreas)]
        [InlineData(Trestle.InteriorOrRoomFeatures.NaturalWoodwork, InteriorFeatures.NaturalWoodwork)]
        [InlineData(Trestle.InteriorOrRoomFeatures.NoInteriorSteps, InteriorFeatures.NoInteriorSteps)]
        [InlineData(Trestle.InteriorOrRoomFeatures.None, InteriorFeatures.None)]
        [InlineData(Trestle.InteriorOrRoomFeatures.OpenFloorplan, InteriorFeatures.OpenFloorplan)]
        [InlineData(Trestle.InteriorOrRoomFeatures.Pantry, InteriorFeatures.Pantry)]
        [InlineData(Trestle.InteriorOrRoomFeatures.MainLevelPrimary, InteriorFeatures.PrimaryBedroomonMain)]
        [InlineData(Trestle.InteriorOrRoomFeatures.RecessedLighting, InteriorFeatures.RecessedLighting)]
        [InlineData(Trestle.InteriorOrRoomFeatures.SeeRemarks, InteriorFeatures.SeeRemarks)]
        [InlineData(Trestle.InteriorOrRoomFeatures.SmartHome, InteriorFeatures.SmartHome)]
        [InlineData(Trestle.InteriorOrRoomFeatures.SmartThermostat, InteriorFeatures.SmartThermostat)]
        [InlineData(Trestle.InteriorOrRoomFeatures.SoakingTub, InteriorFeatures.SoakingTub)]
        [InlineData(Trestle.InteriorOrRoomFeatures.SolarTubes, InteriorFeatures.SolarTubes)]
        [InlineData(Trestle.InteriorOrRoomFeatures.SoundSystem, InteriorFeatures.SoundSystem)]
        [InlineData(Trestle.InteriorOrRoomFeatures.Storage, InteriorFeatures.Storage)]
        [InlineData(Trestle.InteriorOrRoomFeatures.TrackLighting, InteriorFeatures.TrackLighting)]
        [InlineData(Trestle.InteriorOrRoomFeatures.WalkInClosets, InteriorFeatures.WalkInClosets)]
        [InlineData(Trestle.InteriorOrRoomFeatures.WaterSenseFixtures, InteriorFeatures.WaterSenseFixtures)]
        [InlineData(Trestle.InteriorOrRoomFeatures.WiredForData, InteriorFeatures.WiredforData)]
        [InlineData(Trestle.InteriorOrRoomFeatures.WiredForSound, InteriorFeatures.WiredforSound)]
        public void ConvertsTInteriorFeaturesCorrectly(Trestle.InteriorOrRoomFeatures input, InteriorFeatures? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToInteriorFeaturesReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.InteriorOrRoomFeatures)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.Appliances.BarFridge, Appliances.BarFridge)]
        [InlineData(Trestle.Appliances.BuiltInElectricOven, Appliances.BuiltInElectricOven)]
        [InlineData(Trestle.Appliances.BuiltInElectricRange, Appliances.BuiltInElectricRange)]
        [InlineData(Trestle.Appliances.BuiltInFreezer, Appliances.BuiltInFreezer)]
        [InlineData(Trestle.Appliances.BuiltInGasOven, Appliances.BuiltInGasOven)]
        [InlineData(Trestle.Appliances.BuiltInGasRange, Appliances.BuiltInGasRange)]
        [InlineData(Trestle.Appliances.BuiltInOven, Appliances.BuiltInOven)]
        [InlineData(Trestle.Appliances.BuiltInRange, Appliances.BuiltInRange)]
        [InlineData(Trestle.Appliances.BuiltInRefrigerator, Appliances.BuiltInRefrigerator)]
        [InlineData(Trestle.Appliances.ConvectionOven, Appliances.ConvectionOven)]
        [InlineData(Trestle.Appliances.Cooktop, Appliances.Cooktop)]
        [InlineData(Trestle.Appliances.Dishwasher, Appliances.Dishwasher)]
        [InlineData(Trestle.Appliances.Disposal, Appliances.Disposal)]
        [InlineData(Trestle.Appliances.DownDraft, Appliances.DownDraft)]
        [InlineData(Trestle.Appliances.Dryer, Appliances.Dryer)]
        [InlineData(Trestle.Appliances.ElectricCooktop, Appliances.ElectricCooktop)]
        [InlineData(Trestle.Appliances.ElectricRange, Appliances.ElectricRange)]
        [InlineData(Trestle.Appliances.EnergyStarQualifiedAppliances, Appliances.EnergyStarQualifiedAppliances)]
        [InlineData(Trestle.Appliances.EnergyStarQualifiedDishwasher, Appliances.EnergyStarQualifiedDishwasher)]
        [InlineData(Trestle.Appliances.EnergyStarQualifiedDryer, Appliances.EnergyStarQualifiedDryer)]
        [InlineData(Trestle.Appliances.EnergyStarQualifiedFreezer, Appliances.EnergyStarQualifiedFreezer)]
        [InlineData(Trestle.Appliances.EnergyStarQualifiedRefrigerator, Appliances.EnergyStarQualifiedRefrigerator)]
        [InlineData(Trestle.Appliances.EnergyStarQualifiedWasher, Appliances.EnergyStarQualifiedWasher)]
        [InlineData(Trestle.Appliances.EnergyStarQualifiedWaterHeater, Appliances.EnergyStarQualifiedWaterHeater)]
        [InlineData(Trestle.Appliances.ExhaustFan, Appliances.ExhaustFan)]
        [InlineData(Trestle.Appliances.GasCooktop, Appliances.GasCooktop)]
        [InlineData(Trestle.Appliances.GasRange, Appliances.GasRange)]
        [InlineData(Trestle.Appliances.InstantHotWater, Appliances.InstantHotWater)]
        [InlineData(Trestle.Appliances.Microwave, Appliances.Microwave)]
        [InlineData(Trestle.Appliances.None, Appliances.None)]
        [InlineData(Trestle.Appliances.Oven, Appliances.Oven)]
        [InlineData(Trestle.Appliances.ElectricOven, Appliances.OvenElectric)]
        [InlineData(Trestle.Appliances.FreeStandingElectricOven, Appliances.OvenFreeStandingElectric)]
        [InlineData(Trestle.Appliances.FreeStandingGasOven, Appliances.OvenFreeStandingGas)]
        [InlineData(Trestle.Appliances.GasOven, Appliances.OvenGas)]
        [InlineData(Trestle.Appliances.DoubleOven, Appliances.OvenDouble)]
        [InlineData(Trestle.Appliances.PlumbedForIceMaker, Appliances.PlumbedForIceMaker)]
        [InlineData(Trestle.Appliances.Range, Appliances.Range)]
        [InlineData(Trestle.Appliances.FreeStandingElectricRange, Appliances.RangeFreeStandingElectric)]
        [InlineData(Trestle.Appliances.FreeStandingRange, Appliances.RangeFreeStanding)]
        [InlineData(Trestle.Appliances.FreeStandingGasRange, Appliances.RangeFreeStandingGas)]
        [InlineData(Trestle.Appliances.RangeHood, Appliances.RangeHood)]
        [InlineData(Trestle.Appliances.Refrigerator, Appliances.Refrigerator)]
        [InlineData(Trestle.Appliances.FreeStandingRefrigerator, Appliances.RefrigeratorFreeStanding)]
        [InlineData(Trestle.Appliances.SeeRemarks, Appliances.SeeRemarks)]
        [InlineData(Trestle.Appliances.SelfCleaningOven, Appliances.SelfCleaningOven)]
        [InlineData(Trestle.Appliances.SolarHotWater, Appliances.SolarHotWater)]
        [InlineData(Trestle.Appliances.StainlessSteelAppliances, Appliances.StainlessSteelAppliance)]
        [InlineData(Trestle.Appliances.TanklessWaterHeater, Appliances.TanklessWaterHeater)]
        [InlineData(Trestle.Appliances.TrashCompactor, Appliances.TrashCompactor)]
        [InlineData(Trestle.Appliances.VentedExhaustFan, Appliances.VentedExhaustFan)]
        [InlineData(Trestle.Appliances.WasherDryer, Appliances.WasherDryer)]
        [InlineData(Trestle.Appliances.ElectricWaterHeater, Appliances.WaterHeaterElectric)]
        [InlineData(Trestle.Appliances.GasWaterHeater, Appliances.WaterHeaterGas)]
        public void ConvertsToAppliancesCorrectly(Trestle.Appliances input, Appliances? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToAppliancesReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.Appliances)200;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.Fencing.BackYard, Fencing.BackYard)]
        [InlineData(Trestle.Fencing.Block, Fencing.Block)]
        [InlineData(Trestle.Fencing.Brick, Fencing.Brick)]
        [InlineData(Trestle.Fencing.Gate, Fencing.Gate)]
        [InlineData(Trestle.Fencing.Masonry, Fencing.Masonry)]
        [InlineData(Trestle.Fencing.None, Fencing.None)]
        [InlineData(Trestle.Fencing.Partial, Fencing.Partial)]
        [InlineData(Trestle.Fencing.Privacy, Fencing.Privacy)]
        [InlineData(Trestle.Fencing.Security, Fencing.Security)]
        [InlineData(Trestle.Fencing.Stone, Fencing.Stone)]
        [InlineData(Trestle.Fencing.Vinyl, Fencing.Vinyl)]
        [InlineData(Trestle.Fencing.Wood, Fencing.Wood)]
        [InlineData(Trestle.Fencing.WroughtIron, Fencing.WroughtIron)]
        public void ConvertsToFencingCorrectly(Trestle.Fencing input, Fencing? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToFencingReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.Fencing)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.WaterSource.Public, WaterSource.Public)]
        [InlineData(Trestle.WaterSource.Private, WaterSource.Private)]
        [InlineData(Trestle.WaterSource.MunicipalUtilityDistrict, WaterSource.MUD)]
        public void ConvertsToWaterSourceCorrectly(Trestle.WaterSource input, WaterSource? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToWaterSourceReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.WaterSource)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.ExteriorFeatures.Balcony, ExteriorFeatures.Balcony)]
        [InlineData(Trestle.ExteriorFeatures.ExteriorSteps, ExteriorFeatures.ExteriorSteps)]
        [InlineData(Trestle.ExteriorFeatures.ElectricGrill, ExteriorFeatures.GrillElectric)]
        [InlineData(Trestle.ExteriorFeatures.GasGrill, ExteriorFeatures.GrillGas)]
        [InlineData(Trestle.ExteriorFeatures.Lighting, ExteriorFeatures.Lighting)]
        [InlineData(Trestle.ExteriorFeatures.NoExteriorSteps, ExteriorFeatures.NoExteriorSteps)]
        [InlineData(Trestle.ExteriorFeatures.None, ExteriorFeatures.None)]
        [InlineData(Trestle.ExteriorFeatures.OutdoorGrill, ExteriorFeatures.OutdoorGrill)]
        [InlineData(Trestle.ExteriorFeatures.Playground, ExteriorFeatures.Playground)]
        [InlineData(Trestle.ExteriorFeatures.PrivateYard, ExteriorFeatures.PrivateYard)]
        [InlineData(Trestle.ExteriorFeatures.TennisCourts, ExteriorFeatures.TennisCourts)]
        public void ConvertsToExteriorFeaturesCorrectly(Trestle.ExteriorFeatures input, ExteriorFeatures? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToExteriorFeaturesReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.ExteriorFeatures)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.ViewTrestle.Bridges, View.Bridges)]
        [InlineData(Trestle.ViewTrestle.Canal, View.Canal)]
        [InlineData(Trestle.ViewTrestle.Canyon, View.Canyon)]
        [InlineData(Trestle.ViewTrestle.City, View.City)]
        [InlineData(Trestle.ViewTrestle.CityLights, View.CityLights)]
        [InlineData(Trestle.ViewTrestle.CreekStream, View.CreekStream)]
        [InlineData(Trestle.ViewTrestle.Downtown, View.Downtown)]
        [InlineData(Trestle.ViewTrestle.Garden, View.Garden)]
        [InlineData(Trestle.ViewTrestle.GolfCourse, View.GolfCourse)]
        [InlineData(Trestle.ViewTrestle.Hills, View.HillCountry)]
        [InlineData(Trestle.ViewTrestle.Lake, View.Lake)]
        [InlineData(Trestle.ViewTrestle.Marina, View.Marina)]
        [InlineData(Trestle.ViewTrestle.Neighborhood, View.Neighborhood)]
        [InlineData(Trestle.ViewTrestle.None, View.None)]
        [InlineData(Trestle.ViewTrestle.Orchard, View.Orchard)]
        [InlineData(Trestle.ViewTrestle.Panoramic, View.Panoramic)]
        [InlineData(Trestle.ViewTrestle.ParkGreenbelt, View.ParkGreenbelt)]
        [InlineData(Trestle.ViewTrestle.Pasture, View.Pasture)]
        [InlineData(Trestle.ViewTrestle.Pond, View.Pond)]
        [InlineData(Trestle.ViewTrestle.Pool, View.Pool)]
        [InlineData(Trestle.ViewTrestle.River, View.River)]
        [InlineData(Trestle.ViewTrestle.Rural, View.Rural)]
        [InlineData(Trestle.ViewTrestle.Skyline, View.Skyline)]
        [InlineData(Trestle.ViewTrestle.TreesWoods, View.TreesWoods)]
        [InlineData(Trestle.ViewTrestle.Vineyard, View.Vineyard)]
        [InlineData(Trestle.ViewTrestle.Water, View.Water)]
        public void ConvertsToViewCorrectly(Trestle.ViewTrestle input, View? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToViewReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.ViewTrestle)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.CommunityFeatures.Clubhouse, NeighborhoodAmenities.Clubhouse)]
        [InlineData(Trestle.CommunityFeatures.CommunityMailbox, NeighborhoodAmenities.ClusterMailbox)]
        [InlineData(Trestle.CommunityFeatures.CommonGroundsArea, NeighborhoodAmenities.CommonGrounds)]
        [InlineData(Trestle.CommunityFeatures.Courtyard, NeighborhoodAmenities.Courtyard)]
        [InlineData(Trestle.CommunityFeatures.Curbs, NeighborhoodAmenities.Curbs)]
        [InlineData(Trestle.CommunityFeatures.DogPark, NeighborhoodAmenities.DogParkPlayArea)]
        [InlineData(Trestle.CommunityFeatures.Fishing, NeighborhoodAmenities.Fishing)]
        [InlineData(Trestle.CommunityFeatures.FitnessCenter, NeighborhoodAmenities.FitnessCenter)]
        [InlineData(Trestle.CommunityFeatures.GameRoom, NeighborhoodAmenities.GameRoom)]
        [InlineData(Trestle.CommunityFeatures.Gated, NeighborhoodAmenities.Gated)]
        [InlineData(Trestle.CommunityFeatures.GolfCourseCommunity, NeighborhoodAmenities.GolfCourse)]
        [InlineData(Trestle.CommunityFeatures.InternetAccess, NeighborhoodAmenities.HighSpeedInternet)]
        [InlineData(Trestle.CommunityFeatures.KitchenFacilities, NeighborhoodAmenities.KitchenFacilities)]
        [InlineData(Trestle.CommunityFeatures.Lake, NeighborhoodAmenities.Lake)]
        [InlineData(Trestle.CommunityFeatures.MediaRoom, NeighborhoodAmenities.MediaCenterMovieTheatre)]
        [InlineData(Trestle.CommunityFeatures.None, NeighborhoodAmenities.None)]
        [InlineData(Trestle.CommunityFeatures.Park, NeighborhoodAmenities.Park)]
        [InlineData(Trestle.CommunityFeatures.PetAmenities, NeighborhoodAmenities.PetAmenities)]
        [InlineData(Trestle.CommunityFeatures.GardenArea, NeighborhoodAmenities.PicnicArea)]
        [InlineData(Trestle.CommunityFeatures.Playground, NeighborhoodAmenities.Playground)]
        [InlineData(Trestle.CommunityFeatures.Pool, NeighborhoodAmenities.Pool)]
        [InlineData(Trestle.CommunityFeatures.SeeRemarks, NeighborhoodAmenities.SeeRemarks)]
        [InlineData(Trestle.CommunityFeatures.Sidewalks, NeighborhoodAmenities.Sidewalks)]
        [InlineData(Trestle.CommunityFeatures.SportCourts, NeighborhoodAmenities.SportCourtFacility)]
        [InlineData(Trestle.CommunityFeatures.StorageFacilities, NeighborhoodAmenities.Storage)]
        [InlineData(Trestle.CommunityFeatures.StreetLights, NeighborhoodAmenities.StreetLights)]
        [InlineData(Trestle.CommunityFeatures.TennisCourts, NeighborhoodAmenities.TennisCourt)]
        [InlineData(Trestle.CommunityFeatures.TrailsPaths, NeighborhoodAmenities.WalkBikeHikeJogTrails)]
        public void ConvertsToNeighborhoodAmenitiesCorrectly(Trestle.CommunityFeatures input, NeighborhoodAmenities? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToNeighborhoodAmenitiesReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.CommunityFeatures)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.Heating.Ceiling, HeatingSystem.Ceiling)]
        [InlineData(Trestle.Heating.Central, HeatingSystem.Central)]
        [InlineData(Trestle.Heating.Electric, HeatingSystem.Electric)]
        [InlineData(Trestle.Heating.EnergyStarQualifiedEquipment, HeatingSystem.EnergyStarQualifiedEquipment)]
        [InlineData(Trestle.Heating.ExhaustFan, HeatingSystem.ExhaustFan)]
        [InlineData(Trestle.Heating.Fireplaces, HeatingSystem.Fireplace)]
        [InlineData(Trestle.Heating.NaturalGas, HeatingSystem.NaturalGas)]
        [InlineData(Trestle.Heating.PassiveSolar, HeatingSystem.PassiveSolar)]
        [InlineData(Trestle.Heating.Propane, HeatingSystem.Propane)]
        [InlineData(Trestle.Heating.Zoned, HeatingSystem.Zoned)]
        public void ConvertsToHeatingSystemCorrectly(Trestle.Heating input, HeatingSystem? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToHeatingSystemReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.Heating)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.Cooling.CeilingFans, CoolingSystem.CeilingFan)]
        [InlineData(Trestle.Cooling.CentralAir, CoolingSystem.CentralAir)]
        [InlineData(Trestle.Cooling.Dual, CoolingSystem.Dual)]
        [InlineData(Trestle.Cooling.Electric, CoolingSystem.Electric)]
        [InlineData(Trestle.Cooling.EnergyStarQualifiedEquipment, CoolingSystem.EnergyStarQualifiedEquipment)]
        [InlineData(Trestle.Cooling.MultiUnits, CoolingSystem.MultiUnits)]
        [InlineData(Trestle.Cooling.Zoned, CoolingSystem.Zoned)]
        public void ConvertsToCoolingSystemCorrectly(Trestle.Cooling input, CoolingSystem? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToCoolingSystemReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.Cooling)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.Sewer.AerobicSeptic, WaterSewer.AerobicSeptic)]
        [InlineData(Trestle.Sewer.EngineeredSeptic, WaterSewer.EngineeredSeptic)]
        [InlineData(Trestle.Sewer.MunicipalUtilityDistrict, WaterSewer.MUD)]
        [InlineData(Trestle.Sewer.PrivateSewer, WaterSewer.PrivateSewer)]
        [InlineData(Trestle.Sewer.PublicSewer, WaterSewer.PublicSewer)]
        [InlineData(Trestle.Sewer.SepticTank, WaterSewer.SepticTank)]
        public void ConvertsToWaterSewerCorrectly(Trestle.Sewer input, WaterSewer? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToWaterSewerReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.Sewer)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.Utilities.AboveGroundUtilities, UtilitiesDescription.AboveGroundUtilities)]
        [InlineData(Trestle.Utilities.CableAvailable, UtilitiesDescription.CableAvailable)]
        [InlineData(Trestle.Utilities.ElectricityAvailable, UtilitiesDescription.ElectricityAvailable)]
        [InlineData(Trestle.Utilities.HighSpeedInternetAvailable, UtilitiesDescription.InternetCable)]
        [InlineData(Trestle.Utilities.FiberOpticAvailable, UtilitiesDescription.InternetFiber)]
        [InlineData(Trestle.Utilities.SatelliteInternetAvailable, UtilitiesDescription.InternetSatelliteOther)]
        [InlineData(Trestle.Utilities.NaturalGasAvailable, UtilitiesDescription.NaturalGasAvailable)]
        [InlineData(Trestle.Utilities.PhoneAvailable, UtilitiesDescription.PhoneAvailable)]
        [InlineData(Trestle.Utilities.Propane, UtilitiesDescription.Propane)]
        [InlineData(Trestle.Utilities.SewerAvailable, UtilitiesDescription.SewerAvailable)]
        [InlineData(Trestle.Utilities.SewerConnected, UtilitiesDescription.SewerConnected)]
        [InlineData(Trestle.Utilities.UndergroundUtilities, UtilitiesDescription.UndergroundUtilities)]
        [InlineData(Trestle.Utilities.WaterAvailable, UtilitiesDescription.WaterAvailable)]
        [InlineData(Trestle.Utilities.WaterConnected, UtilitiesDescription.WaterConnected)]
        public void ConvertsToUtilitiesDescriptionCorrectly(Trestle.Utilities input, UtilitiesDescription? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToUtilitiesDescriptionReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.Utilities)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.ListingTerms.Cash, AcceptableFinancing.Cash)]
        [InlineData(Trestle.ListingTerms.Conventional, AcceptableFinancing.Conventional)]
        [InlineData(Trestle.ListingTerms.FHA, AcceptableFinancing.FHA)]
        [InlineData(Trestle.ListingTerms.SeeRemarks, AcceptableFinancing.SeeRemarks)]
        [InlineData(Trestle.ListingTerms.LeaseBack, AcceptableFinancing.LeaseBack)]
        [InlineData(Trestle.ListingTerms.TexasVet, AcceptableFinancing.TexasVet)]
        [InlineData(Trestle.ListingTerms.UsdaLoan, AcceptableFinancing.USDALoan)]
        [InlineData(Trestle.ListingTerms.VaLoan, AcceptableFinancing.VALoan)]
        public void ConvertsToAcceptableFinancingCorrectly(Trestle.ListingTerms input, AcceptableFinancing? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToAcceptableFinancingReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.ListingTerms)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.CompensationType.Dollars, CommissionType.Amount)]
        [InlineData(Trestle.CompensationType.Percent, CommissionType.Percent)]
        public void ConvertsToCommissionTypeCorrectly(Trestle.CompensationType input, CommissionType? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToCommissionTypeReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.CompensationType)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.FeeFrequency.Annually, BillingFrequency.Annually)]
        [InlineData(Trestle.FeeFrequency.Monthly, BillingFrequency.Monthly)]
        [InlineData(Trestle.FeeFrequency.Quarterly, BillingFrequency.Quarterly)]
        [InlineData(Trestle.FeeFrequency.SemiAnnually, BillingFrequency.SemiAnnually)]
        public void ConvertsToBillingFrequencyCorrectly(Trestle.FeeFrequency input, BillingFrequency? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToBillingFrequencyReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.FeeFrequency)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.LockBoxType.Combo, LockBoxType.Combo)]
        [InlineData(Trestle.LockBoxType.Electronic, LockBoxType.Supra)]
        [InlineData(Trestle.LockBoxType.Supra, LockBoxType.Supra)]
        [InlineData(Trestle.LockBoxType.None, LockBoxType.None)]
        [InlineData(Trestle.LockBoxType.SeeRemarks, LockBoxType.SeeRemarks)]
        public void ConvertsToLockBoxTypeCorrectly(Trestle.LockBoxType input, LockBoxType? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToLockBoxTypeReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.LockBoxType)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.ShowingRequirements.ListingAgentPresent, ShowingRequirements.AgentOrOwnerPresent)]
        [InlineData(Trestle.ShowingRequirements.AppointmentOnly, ShowingRequirements.AppointmentOnly)]
        [InlineData(Trestle.ShowingRequirements.CallBeforeShowing, ShowingRequirements.CallFirst)]
        [InlineData(Trestle.ShowingRequirements.GoDirect, ShowingRequirements.Go)]
        [InlineData(Trestle.ShowingRequirements.Lockbox, ShowingRequirements.Lockbox)]
        [InlineData(Trestle.ShowingRequirements.KeyInOffice, ShowingRequirements.KeyInOffice)]
        [InlineData(Trestle.ShowingRequirements.CallOwner, ShowingRequirements.CallOwner)]
        [InlineData(Trestle.ShowingRequirements.SecuritySystem, ShowingRequirements.SecuritySystem)]
        [InlineData(Trestle.ShowingRequirements.SeeShowingInstructions, ShowingRequirements.SeeShowingInstructions)]
        [InlineData(Trestle.ShowingRequirements.ShowingService, ShowingRequirements.ShowingService)]
        public void ConvertsToShowingRequirementsCorrectly(Trestle.ShowingRequirements input, ShowingRequirements? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToShowingRequirementsReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.ShowingRequirements)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.SecurityFeatures.CarbonMonoxideDetectors, SecurityFeatures.CarbonMonoxideDetectors)]
        [InlineData(Trestle.SecurityFeatures.FireAlarm, SecurityFeatures.FireAlarm)]
        [InlineData(Trestle.SecurityFeatures.FireSprinklerSystem, SecurityFeatures.FireSprinklerSystem)]
        [InlineData(Trestle.SecurityFeatures.GatedWithGuard, SecurityFeatures.GatedwithGuard)]
        [InlineData(Trestle.SecurityFeatures.None, SecurityFeatures.None)]
        [InlineData(Trestle.SecurityFeatures.Prewired, SecurityFeatures.Prewired)]
        [InlineData(Trestle.SecurityFeatures.SecuredGarageParking, SecurityFeatures.SecuredGarageParking)]
        [InlineData(Trestle.SecurityFeatures.SecuritySystem, SecurityFeatures.SecuritySystem)]
        [InlineData(Trestle.SecurityFeatures.SecuritySystemLeased, SecurityFeatures.SecuritySystemLeased)]
        [InlineData(Trestle.SecurityFeatures.SecuritySystemOwned, SecurityFeatures.SecuritySystemOwned)]
        [InlineData(Trestle.SecurityFeatures.SeeRemarks, SecurityFeatures.SeeRemarks)]
        [InlineData(Trestle.SecurityFeatures.SmokeDetectors, SecurityFeatures.SmokeDetectors)]
        public void ConvertsToSecurityFeaturesCorrectly(Trestle.SecurityFeatures input, SecurityFeatures? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToSecurityFeaturesReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.SecurityFeatures)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.WindowFeatures.BayWindows, WindowFeatures.BayWindows)]
        [InlineData(Trestle.WindowFeatures.Blinds, WindowFeatures.Blinds)]
        [InlineData(Trestle.WindowFeatures.DoublePaneWindows, WindowFeatures.DoublePaneWindows)]
        [InlineData(Trestle.WindowFeatures.EnergyStarQualifiedWindows, WindowFeatures.EnergyStarQualifiedWindows)]
        [InlineData(Trestle.WindowFeatures.InsulatedWindows, WindowFeatures.InsulatedWindows)]
        [InlineData(Trestle.WindowFeatures.None, WindowFeatures.None)]
        [InlineData(Trestle.WindowFeatures.PlantationShutters, WindowFeatures.PlantationShutters)]
        [InlineData(Trestle.WindowFeatures.Screens, WindowFeatures.Screens)]
        [InlineData(Trestle.WindowFeatures.Shutters, WindowFeatures.Shutters)]
        [InlineData(Trestle.WindowFeatures.StormWindows, WindowFeatures.StormWindows)]
        [InlineData(Trestle.WindowFeatures.Vinyl, WindowFeatures.VinylWindows)]
        public void ConvertsToWindowFeaturesCorrectly(Trestle.WindowFeatures input, WindowFeatures? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToWindowFeaturesReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.WindowFeatures)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.WaterfrontFeatures.CanalFront, WaterfrontFeatures.CanalFront)]
        [InlineData(Trestle.WaterfrontFeatures.Creek, WaterfrontFeatures.Creek)]
        [InlineData(Trestle.WaterfrontFeatures.LakeFront, WaterfrontFeatures.LakeFront)]
        [InlineData(Trestle.WaterfrontFeatures.LakePrivileges, WaterfrontFeatures.LakePrivileges)]
        [InlineData(Trestle.WaterfrontFeatures.None, WaterfrontFeatures.None)]
        [InlineData(Trestle.WaterfrontFeatures.Pond, WaterfrontFeatures.Pond)]
        [InlineData(Trestle.WaterfrontFeatures.RiverFront, WaterfrontFeatures.RiverFront)]
        [InlineData(Trestle.WaterfrontFeatures.Waterfront, WaterfrontFeatures.WaterFront)]
        public void ConvertsToWaterfrontFeaturesCorrectly(Trestle.WaterfrontFeatures input, WaterfrontFeatures? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToWaterfrontFeaturesReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.WaterfrontFeatures)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(Trestle.BuyerFinancing.Cash, SaleTerms.Cash)]
        [InlineData(Trestle.BuyerFinancing.TexasVet, SaleTerms.TexasVet)]
        [InlineData(Trestle.BuyerFinancing.Conventional, SaleTerms.Conventional)]
        [InlineData(Trestle.BuyerFinancing.Usda, SaleTerms.UsdaEligible)]
        [InlineData(Trestle.BuyerFinancing.Fha, SaleTerms.FHA)]
        [InlineData(Trestle.BuyerFinancing.Va, SaleTerms.VA)]
        public void ConvertsToSaleTermsCorrectly(Trestle.BuyerFinancing input, SaleTerms? expected)
        {
            // Arrange

            // Act
            var result = EnumMappings.ToAborEnum(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertsToSaleTermsReturnsNull()
        {
            // Arrange
            var unknownInput = (Trestle.BuyerFinancing)100;

            // Act
            var result = EnumMappings.ToAborEnum(unknownInput);

            // Assert
            Assert.Null(result);
        }
    }
}
