using System;
using System.Threading.Tasks;

namespace BehaviorTrees.Decorator
{
    public class RepeaterNode<TSubject> : DecoratorNode<TSubject>
    {

        private readonly int? _count;

        public int RanCount { get; private set; }

        /// <param name="child">The child to run repeatedly.</param>
        /// <param name="count">How many times to run the child. Set to null to repeat forever.</param>
        public RepeaterNode(string name, Node<TSubject> child, int? count = null) : base(name, child)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("Count must be greater than zero.");

            _count = count;
        }

        public override ValueTask BeforeRunAsync()
        {
            RanCount = 0;
            return new ValueTask();
        }

        public override async ValueTask<ResultType> RunAsync()
        {
            while (!_count.HasValue || RanCount < _count)
            {
                await _child.BeforeRunAsync();
                var result = await _child.RunAsync();

                if (result == ResultType.Running)
                    return ResultType.Running;

                RanCount++;
            }

            return ResultType.Succeeded;
        }

        protected override async void OnChildFinished(ResultType result)
        {
            RanCount++;
            await RunAsync();
        }

    }
}
