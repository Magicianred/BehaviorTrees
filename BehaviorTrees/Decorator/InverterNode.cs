using System;
using System.Threading.Tasks;

namespace BehaviorTrees.Decorator
{
    public class InverterNode<TSubject> : DecoratorNode<TSubject>
    {

        public InverterNode(string name, Node<TSubject> child) : base(name, child) { }

        public override ValueTask BeforeRunAsync() => new ValueTask();

        public override async ValueTask<ResultType> RunAsync()
        {
            await _child.BeforeRunAsync();
            var result = await _child.RunAsync();
            return InvertResult(result);
        }

        protected override void OnChildFinished(ResultType result) => OnFinished(InvertResult(result));

        private static ResultType InvertResult(ResultType result)
        {
            return result switch
            {
                ResultType.Failed => ResultType.Succeeded,
                ResultType.Succeeded => ResultType.Failed,
                ResultType.Running => ResultType.Running,
                _ => throw new Exception("Invalid ResultType"),
            };
        }

    }
}
