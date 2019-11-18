using BehaviorTrees.Composite;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BehaviorTrees.UnitTests
{
    public class SequenceNodeTests
    {

        [Fact]
        public async Task RunAsync_ShouldReturnFailedIfAChildFails()
        {
            // Arrange
            var child1 = new Mock<Node<object>>();
            child1.Setup(n => n.RunAsync())
                .Returns(new ValueTask<ResultType>(ResultType.Failed));

            var child2 = new Mock<Node<object>>();
            child2.Setup(n => n.RunAsync())
                .Returns(new ValueTask<ResultType>(ResultType.Succeeded));

            var node = new SequenceNode<object>("", child1.Object, child2.Object);

            // Act
            await node.BeforeRunAsync();
            var result = await node.RunAsync();

            // Assert
            Assert.Equal(ResultType.Failed, result);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnSucceededIfAllChildrenSucceededImmediately()
        {
            // Arrange
            var child1 = new Mock<Node<object>>();
            child1.Setup(n => n.RunAsync())
                .Returns(new ValueTask<ResultType>(ResultType.Succeeded));

            var child2 = new Mock<Node<object>>();
            child2.Setup(n => n.RunAsync())
                .Returns(new ValueTask<ResultType>(ResultType.Succeeded));

            var node = new SequenceNode<object>("", child1.Object, child2.Object);

            // Act
            await node.BeforeRunAsync();
            var result = await node.RunAsync();

            // Assert
            Assert.Equal(ResultType.Succeeded, result);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnRunningAndRaiseEventWhenDone()
        {
            // Arrange
            var child1 = new TestNode();
            var node = new SequenceNode<object>("", child1);
            ResultType? nodeResult = null;

            node.Finished += (r) =>
            {
                nodeResult = r;
            };

            // Act
            await node.BeforeRunAsync();
            var result = await node.RunAsync();
            child1.TriggerFinishedEvent(ResultType.Succeeded);

            // Assert
            Assert.Equal(ResultType.Running, result);
            Assert.NotNull(nodeResult);
            Assert.Equal(ResultType.Succeeded, nodeResult);
        }

    }
}
