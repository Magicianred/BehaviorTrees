using System.Threading.Tasks;

namespace BehaviorTrees.UnitTests
{
    public class TestNode : Node<object>
    {

        public TestNode() : base("Test node") { }

        public override ValueTask BeforeRunAsync()
        {
            return new ValueTask();
        }

        public override ValueTask<ResultType> RunAsync()
        {
            return new ValueTask<ResultType>(ResultType.Running);
        }

        public void TriggerFinishedEvent(ResultType type)
        {
            OnFinished(type);
        }

    }
}
