using BehaviorTrees.Decorator;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BehaviorTrees.UnitTests
{
    public class InverterNodeTests
    {

        [Fact]
        public async Task RunAsync_ShouldReturnFailed_WhenChildSucceeds()
        {
            // Arrange
            var child = new Mock<Node<object>>();
            child.Setup(c => c.RunAsync())
                .Returns(new ValueTask<ResultType>(ResultType.Succeeded));

            var node = new InverterNode<object>("", child.Object);

            // Act
            await node.BeforeRunAsync();
            var result = await node.RunAsync();

            // Assert
            Assert.Equal(ResultType.Failed, result);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnSucceeded_WhenChildFails()
        {
            // Arrange
            var child = new Mock<Node<object>>();
            child.Setup(c => c.RunAsync())
                .Returns(new ValueTask<ResultType>(ResultType.Failed));

            var node = new InverterNode<object>("", child.Object);

            // Act
            await node.BeforeRunAsync();
            var result = await node.RunAsync();

            // Assert
            Assert.Equal(ResultType.Succeeded, result);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnRunningAndTriggerEventWithSucceeded_WhenChildRunsThenFails()
        {
            // Arrange
            var child = new TestNode();
            var node = new InverterNode<object>("", child);
            ResultType? nodeResult = null;

            node.Finished += (r) =>
            {
                nodeResult = r;
            };

            // Act
            await node.BeforeRunAsync();
            var result = await node.RunAsync();
            child.TriggerFinishedEvent(ResultType.Failed);

            // Assert
            Assert.Equal(ResultType.Running, result);
            Assert.NotNull(nodeResult);
            Assert.Equal(ResultType.Succeeded, nodeResult);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnRunningAndTriggerEventWithFailed_WhenChildRunsThenSucceeds()
        {
            // Arrange
            var child = new TestNode();
            var node = new InverterNode<object>("", child);
            ResultType? nodeResult = null;

            node.Finished += (r) =>
            {
                nodeResult = r;
            };

            // Act
            await node.BeforeRunAsync();
            var result = await node.RunAsync();
            child.TriggerFinishedEvent(ResultType.Succeeded);

            // Assert
            Assert.Equal(ResultType.Running, result);
            Assert.NotNull(nodeResult);
            Assert.Equal(ResultType.Failed, nodeResult);
        }

    }
}
