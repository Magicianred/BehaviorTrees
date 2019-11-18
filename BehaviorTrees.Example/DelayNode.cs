using System.Threading.Tasks;

namespace BehaviorTrees.Example
{
    public class DelayNode : Node<int>
    {

        private readonly int _delay;

        public DelayNode(string name, int delay) : base(name) {
            _delay = delay;
        }

        public override ValueTask BeforeRunAsync() => new ValueTask();

        public override ValueTask<ResultType> RunAsync()
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(_delay);
                OnFinished(ResultType.Succeeded);
            });

            return new ValueTask<ResultType>(ResultType.Running);
        }

    }
}
