using System;
using System.Threading.Tasks;

namespace BehaviorTrees
{
    public abstract class Node<TSubject>
    {

        protected TSubject _subject;
        protected DataContext _dataContext;

        public string Name { get; }

        public event Action<ResultType> Finished;

        public Node(string name)
        {
            Name = name;
        }

        public virtual void SetDataContext(DataContext dataContext) => _dataContext = dataContext;
        public virtual void SetSubject(TSubject subject) => _subject = subject;

        protected void OnFinished(ResultType result)
        {
            if (result == ResultType.Running)
                throw new Exception("Cannot return Running when finished.");

            Finished?.Invoke(result);
        }

        public abstract ValueTask BeforeRunAsync();
        public abstract ValueTask<ResultType> RunAsync();

    }

    public enum ResultType
    {
        Succeeded,
        Failed,
        Running
    }
}
