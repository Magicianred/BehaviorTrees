using System;
using System.Threading.Tasks;

namespace BehaviorTrees
{
    public class ActionNode<TSubject> : Node<TSubject>
    {

        private readonly Func<TSubject, ValueTask<ResultType>> _runFunc;
        private readonly Func<TSubject, ValueTask> _beforeRunFunc;

        public ActionNode(string name, Func<TSubject, ValueTask<ResultType>> runFunc,
            Func<TSubject, ValueTask> beforeRunFunc = null) : base(name)
        {
            _runFunc = runFunc;
            _beforeRunFunc = beforeRunFunc;
        }

        public override async ValueTask BeforeRunAsync()
        {
            if (_beforeRunFunc != null)
                await _beforeRunFunc(_subject);
        }

        public override async ValueTask<ResultType> RunAsync() => await _runFunc(_subject);

    }
}
