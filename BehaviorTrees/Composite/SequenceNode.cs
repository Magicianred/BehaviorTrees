using System;
using System.Threading.Tasks;

namespace BehaviorTrees.Composite
{
    public class SequenceNode<TSubject> : CompositeNode<TSubject>
    {

        private int _currentChildIndex;
        private ResultType _lastChildResult;

        public SequenceNode(string name, params Node<TSubject>[] children) : base(name, children) { }

        public override ValueTask BeforeRunAsync()
        {
            _currentChildIndex = -1;
            return new ValueTask();
        }

        public override async ValueTask<ResultType> RunAsync()
        {
            _currentChildIndex++;
            if (_currentChildIndex >= _children.Length)
            {
                OnFinished(_lastChildResult);
                return _lastChildResult;
            }

            var currentChild = _children[_currentChildIndex];
            await currentChild.BeforeRunAsync();
            _lastChildResult = await currentChild.RunAsync();

            return _lastChildResult switch
            {
                ResultType.Failed => ResultType.Failed,
                ResultType.Running => ResultType.Running,
                ResultType.Succeeded => await RunAsync(),
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
