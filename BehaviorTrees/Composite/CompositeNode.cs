using System;

namespace BehaviorTrees.Composite
{
    public abstract class CompositeNode<TSubject> : Node<TSubject>
    {

        protected readonly Node<TSubject>[] _children;

        public CompositeNode(string name, params Node<TSubject>[] children) : base(name)
        {
            if (children.Length == 0)
                throw new ArgumentException("Composite nodes must contain at least one child node.");

            _children = children;

            foreach (var child in _children)
                child.Finished += OnChildFinished;
        }

        protected abstract void OnChildFinished(ResultType result);

        public override void SetDataContext(DataContext dataContext)
        {
            base.SetDataContext(dataContext);
            foreach (var child in _children)
                child.SetDataContext(dataContext);
        }

        public override void SetSubject(TSubject subject)
        {
            base.SetSubject(subject);
            foreach (var child in _children)
                child.SetSubject(subject);
        }

    }

}
