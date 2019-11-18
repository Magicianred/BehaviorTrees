using System;
using Xunit;

namespace BehaviorTrees.UnitTests
{
    public class NodeTests
    {

        [Fact]
        public void OnFinished_ShouldThrowException_WhenResultTypeIsRunning()
        {
            // Arrange
            var node = new TestNode();

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => node.TriggerFinishedEvent(ResultType.Running));
            Assert.Equal("Cannot return Running when finished.", ex.Message);
        }

        [Fact]
        public void OnFinished_ShouldTriggerEvent_WhenResultTypeIsValid()
        {
            // Arrange
            var node = new TestNode();
            ResultType? result = null;
            node.Finished += (r) => result = r;

            // Act
            node.TriggerFinishedEvent(ResultType.Failed);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ResultType.Failed, result);

        }

    }
}
