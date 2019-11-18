using LiteGuard;

namespace BehaviorTrees.Decorator
{
    public abstract class DecoratorNode<TSubject> : Node<TSubject>
    {

        protected readonly Node<TSubject> _child;

        public DecoratorNode(string name, Node<TSubject> child) : base(name)
        {
            Guard.AgainstNullArgument(nameof(child), child);

            _child = child;
            _child.Finished += OnChildFinished;
        }

        protected abstract void OnChildFinished(ResultType result);

        public override void SetDataContext(DataContext dataContext)
        {
            base.SetDataContext(dataContext);
            _child.SetDataContext(dataContext);
        }

        public override void SetSubject(TSubject subject)
        {
            base.SetSubject(subject);
            _child.SetSubject(subject);
        }

    }
}
