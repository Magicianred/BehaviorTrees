using System;
using Xunit;

namespace BehaviorTrees.UnitTests
{
    public class CompositeNodeTests
    {

        [Fact]
        public void Constructor_ShouldThrowException_WhenGivenZeroChildren()
        {
            // Arrange, Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new CompositeTestNode());
            Assert.Equal("Composite nodes must contain at least one child node.", ex.Message);
        }

    }
}
