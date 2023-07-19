namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Domain.Comparers;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class ListingHoaComparerTests
    {
        [Fact]
        public void EqualsResturnTrue()
        {
            // Arrange
            var currentHoa = new Mock<IProvideHoaInfo>();
            var newHoa = new Mock<IProvideHoaInfo>();
            var comparer = new Mock<ListingHoaComparer>();
            comparer.SetupAllProperties();

            // Act
            var result = comparer.Object.Equals(currentHoa.Object, newHoa.Object);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsResturnFalse()
        {
            // Arrange
            var currentHoa = new Mock<IProvideHoaInfo>();
            IProvideHoaInfo newHoa = null;
            var comparer = new Mock<ListingHoaComparer>();
            comparer.SetupAllProperties();

            // Act
            var result = comparer.Object.Equals(currentHoa.Object, newHoa);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCodeReturnCodeNumber()
        {
            // Arrange
            var name = string.Empty;
            var currentHoa = new Mock<IProvideHoaInfo>();
            currentHoa.Setup(x => x.BillingFrequency).Returns(BillingFrequency.Annually);
            currentHoa.Setup(x => x.Name).Returns(name);
            var comparer = new Mock<ListingHoaComparer>();
            comparer.SetupAllProperties();

            // Act
            var result = comparer.Object.GetHashCode(currentHoa.Object);

            // Assert
            Assert.True(result != 0);
        }

        [Fact]
        public void GetHashCodeReturn0()
        {
            // Arrange
            IProvideHoaInfo newHoa = null;
            var comparer = new Mock<ListingHoaComparer>();
            comparer.SetupAllProperties();

            // Act
            var result = comparer.Object.GetHashCode(newHoa);

            // Assert
            Assert.Equal(0, result);
        }
    }
}
