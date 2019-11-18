using BehaviorTrees.Composite;
using System.Threading.Tasks;
using Xunit;

namespace BehaviorTrees.UnitTests
{
    public class SelectorNodeTests
    {

        [Fact]
        public async Task RunAsync_ShouldRunEveryChildUntilOneSucceeds()
        {
            // Arrange
            var child1 = new TestNode();
            var child2 = new TestNode();
            var child3 = new TestNode();
            var node = new SelectorNode<object>("", child1, child2, child3);
            ResultType? nodeResult = null;

            node.Finished += (r) =>
            {
                nodeResult = r;
            };

            // Act
            await node.BeforeRunAsync();
            var result = await node.RunAsync();
            child1.TriggerFinishedEvent(ResultType.Failed);
            child2.TriggerFinishedEvent(ResultType.Succeeded);
            child3.TriggerFinishedEvent(ResultType.Failed);

            // Assert
            Assert.Equal(ResultType.Running, result);
            Assert.NotNull(nodeResult);
            Assert.Equal(ResultType.Succeeded, nodeResult);

        }

        [Fact]
        public async Task RunAsync_ShouldFailIfAllChildrenFail()
        {
            // Arrange
            var child1 = new TestNode();
            var child2 = new TestNode();
            var node = new SelectorNode<object>("", child1, child2);
            ResultType? nodeResult = null;

            node.Finished += (r) =>
            {
                nodeResult = r;
            };

            // Act
            await node.BeforeRunAsync();
            var result = await node.RunAsync();
            child1.TriggerFinishedEvent(ResultType.Failed);
            child2.TriggerFinishedEvent(ResultType.Failed);

            // Assert
            Assert.Equal(ResultType.Running, result);
            Assert.NotNull(nodeResult);
            Assert.Equal(ResultType.Failed, nodeResult);

        }

    }
}
