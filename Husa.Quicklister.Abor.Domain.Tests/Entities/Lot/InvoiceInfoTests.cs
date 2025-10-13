namespace Husa.Quicklister.Abor.Domain.Tests.Entities.Lot
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Xunit;

    public class InvoiceInfoTests
    {
        [Fact]
        public void ConstructorWithParametersShouldAssignProperties()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var invoiceId = "INV-001";
            var docNumber = "DOC-123";
            var createdDate = DateTime.UtcNow;

            // Act
            var invoiceInfo = new InvoiceInfo(userId, invoiceId, docNumber, createdDate);

            // Assert
            Assert.Equal(userId, invoiceInfo.InvoiceRequestedBy);
            Assert.Equal(createdDate, invoiceInfo.InvoiceRequestedOn);
            Assert.Equal(invoiceId, invoiceInfo.InvoiceId);
            Assert.Equal(docNumber, invoiceInfo.DocNumber);
        }

        [Fact]
        public void ConstructorEmptyShouldInitializeWithNulls()
        {
            // Act
            var invoiceInfo = new InvoiceInfo();

            // Assert
            Assert.Null(invoiceInfo.InvoiceId);
            Assert.Null(invoiceInfo.DocNumber);
            Assert.Null(invoiceInfo.InvoiceRequestedBy);
            Assert.Null(invoiceInfo.InvoiceRequestedOn);
        }

        [Fact]
        public void PropertiesShouldBeSettable()
        {
            // Arrange
            var invoiceInfo = new InvoiceInfo();
            var newUserId = Guid.NewGuid();
            var newInvoiceId = "INV-XYZ";
            var newDocNumber = "DOC-456";
            var newDate = DateTime.UtcNow;

            // Act
            invoiceInfo.InvoiceId = newInvoiceId;
            invoiceInfo.DocNumber = newDocNumber;
            invoiceInfo.InvoiceRequestedBy = newUserId;
            invoiceInfo.InvoiceRequestedOn = newDate;

            // Assert
            Assert.Equal(newInvoiceId, invoiceInfo.InvoiceId);
            Assert.Equal(newDocNumber, invoiceInfo.DocNumber);
            Assert.Equal(newUserId, invoiceInfo.InvoiceRequestedBy);
            Assert.Equal(newDate, invoiceInfo.InvoiceRequestedOn);
        }

        [Fact]
        public void ConstructorWithParametersAllowsEmptyStrings()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createdDate = DateTime.UtcNow;

            // Act
            var invoiceInfo = new InvoiceInfo(userId, string.Empty, string.Empty, createdDate);

            // Assert
            Assert.Equal(string.Empty, invoiceInfo.InvoiceId);
            Assert.Equal(string.Empty, invoiceInfo.DocNumber);
            Assert.Equal(userId, invoiceInfo.InvoiceRequestedBy);
            Assert.Equal(createdDate, invoiceInfo.InvoiceRequestedOn);
        }

        [Fact]
        public void InvoiceRequestedByShouldAcceptNullWhenUsingEmptyConstructor()
        {
            // Act
            var invoiceInfo = new InvoiceInfo
            {
                InvoiceRequestedBy = null,
            };

            // Assert
            Assert.Null(invoiceInfo.InvoiceRequestedBy);
        }
    }
}
