using System;
using System.Threading.Tasks;

namespace BehaviorTrees.Composite
{
    public class SelectorNode<TSubject> : CompositeNode<TSubject>
    {

        private int _currentChildIndex;
        private ResultType? _lastChildResult;

        public SelectorNode(string name, params Node<TSubject>[] children) : base(name, children) { }

        public override ValueTask BeforeRunAsync()
        {
            _currentChildIndex = -1;
            return new ValueTask();
        }

        public override async ValueTask<ResultType> RunAsync()
        {
            // Child succeeded
            if (_lastChildResult == ResultType.Succeeded)
            {
                OnFinished(ResultType.Succeeded);
                return _lastChildResult.Value;
            }

            // Run next child
            _currentChildIndex++;
            if (_currentChildIndex >= _children.Length)
            {
                OnFinished(_lastChildResult.Value);
                return _lastChildResult.Value;
            }

            var currentChild = _children[_currentChildIndex];
            await currentChild.BeforeRunAsync();
            _lastChildResult = await currentChild.RunAsync();

            return _lastChildResult switch
            {
                ResultType.Failed => await RunAsync(),
                ResultType.Running => ResultType.Running,
                ResultType.Succeeded => ResultType.Succeeded,
                _ => throw new Exception("Invalid ResultType"),
            };
        }

        protected override async void OnChildFinished(ResultType result)
        {
            _lastChildResult = result;
            await RunAsync();
        }

    }
}
