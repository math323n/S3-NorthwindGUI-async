using Entities;
using System;
using System.Collections.Generic;
using Utilities;
using Xunit;

namespace Tests
{
    public class ValidationTests
    {
        [Fact]
        public void InitializeValidOrder()
        {
            // Arrange
            OrderDetail detail = default;

           // Assert 
          detail = new OrderDetail(3, 12, 65, 1, 4);

        }

        [Fact]
        public void InitializeInvalidObject()
        {
            // Arrange
            OrderDetail detail = default;

            // Actsert

            Assert.Throws<ArgumentException>(
                () => detail = new OrderDetail(3, 12, 65, -1, 4));
        }
        [Fact]
        public void OrderDetailCanMutateToInvalidState()
        {
            // Arrange
            OrderDetail detail = new OrderDetail(3, 12, 65, 1, 4);

            // Act

            // Assert
            Assert.Throws<ArgumentException>(
                () => detail.Quantity = -3);
        }

    }
}