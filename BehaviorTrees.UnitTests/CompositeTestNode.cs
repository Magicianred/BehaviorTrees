using BehaviorTrees.Composite;
using System;
using System.Threading.Tasks;

namespace BehaviorTrees.UnitTests
{
    public class CompositeTestNode : CompositeNode<object>
    {

        public CompositeTestNode(params Node<object>[] children) : base("", children) { }

        public override ValueTask BeforeRunAsync()
        {
            throw new NotImplementedException();
        }

        public override ValueTask<ResultType> RunAsync()
        {
            throw new NotImplementedException();
        }

        protected override void OnChildFinished(ResultType result)
        {
            throw new NotImplementedException();
        }

    }
}
