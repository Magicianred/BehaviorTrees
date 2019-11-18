using BehaviorTrees.Decorator;
using System.Threading.Tasks;

namespace BehaviorTrees
{
    public class BehaviorTree<TSubject> : DecoratorNode<TSubject>
    {

        public TSubject Subject { get; private set; }
        public DataContext DataContext { get; private set; }

        public BehaviorTree(TSubject subject, Node<TSubject> child) : base("Root", child)
        {
            Subject = subject;
        }

        public override async ValueTask BeforeRunAsync()
        {
            DataContext = new DataContext();
            _child.SetDataContext(DataContext);
            _child.SetSubject(Subject);
            await _child.BeforeRunAsync();
        }

        public override async ValueTask<ResultType> RunAsync() => await _child.RunAsync();

        protected override void OnChildFinished(ResultType result) => OnFinished(result);

    }
}
