using BehaviorTrees.Composite;
using BehaviorTrees.Decorator;
using System;
using System.Threading.Tasks;

namespace BehaviorTrees.Example
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            // await Example1();
            await Example2();

            Console.ReadKey();
        }

        private static async Task Example1()
        {
            var tree = new BehaviorTree<int>(10,
                new SequenceNode<int>("Seq1",
                    new ActionNode<int>("Start", s =>
                    {
                        Log("Start: " + s);
                        return new ValueTask<ResultType>(ResultType.Succeeded);
                    }),
                    new RepeaterNode<int>("Repeat3Times",
                        new ActionNode<int>("Repeated Action", s =>
                        {
                            Log("Repeated: " + s);
                            return new ValueTask<ResultType>(ResultType.Succeeded);
                        }),
                    3),
                    new ActionNode<int>("End", s =>
                    {
                        Log("End: " + s);
                        return new ValueTask<ResultType>(ResultType.Succeeded);
                    })
                )
            );

            await tree.BeforeRunAsync();
            await tree.RunAsync();
        }

        private static async Task Example2()
        {
            var tree = new BehaviorTree<int>(10,
                new SequenceNode<int>("Seq1",
                    new ActionNode<int>("Start", s =>
                    {
                        Log("Start: " + s);
                        return new ValueTask<ResultType>(ResultType.Succeeded);
                    }),
                    new DelayNode("Delay for 5 seconds", 5000),
                    new ActionNode<int>("End", s =>
                    {
                        Log("End: " + s);
                        return new ValueTask<ResultType>(ResultType.Succeeded);
                    })
                )
            );

            await tree.BeforeRunAsync();
            await tree.RunAsync();
        }

        private static void Log(string message)
        {
            Console.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), message);
        }

    }
}
