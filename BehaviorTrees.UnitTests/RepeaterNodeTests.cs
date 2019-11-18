using BehaviorTrees.Decorator;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BehaviorTrees.UnitTests
{
    public class RepeaterNodeTests
    {

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public async Task RunAsync_ShouldRunChildCountTimes(int count)
        {
            // Arrange
            var child = new Mock<Node<object>>();
            child.Setup(c => c.RunAsync())
                .Returns(new ValueTask<ResultType>(ResultType.Succeeded));

            var node = new RepeaterNode<object>("", child.Object, count);

            // Act
            await node.BeforeRunAsync();
            var result = await node.RunAsync();

            // Assert
            Assert.Equal(ResultType.Succeeded, result);
            Assert.Equal(count, node.RanCount);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnRunningAndTriggerEventWhenCountIsDone()
        {
            // Arrange
            var child = new TestNode();
            var node = new RepeaterNode<object>("", child, 1);

            // Act
            await node.BeforeRunAsync();
            var result = await node.RunAsync();
            child.TriggerFinishedEvent(ResultType.Succeeded);

            // Assert
            Assert.Equal(ResultType.Running, result);
            Assert.Equal(1, node.RanCount);
        }

    }
}
