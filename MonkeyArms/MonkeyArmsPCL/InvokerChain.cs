using System.Collections.Generic;
using System.Linq;

namespace MonkeyArms
{
    public class InvokerChain
    {
        public readonly Invoker Failed = new Invoker();

        public readonly Invoker Completed = new Invoker();

        protected InvokerMap HandlerMap = new InvokerMap();

        protected Dictionary<IInvoker, object> ArgsMap = new Dictionary<IInvoker, object>();

        protected int ArgsToCollectCount = 0;

        public void FailOn(Invoker[] invokersToFailOn)
        {
        }

        public void CollectArgsForInvokers(IInvoker[] invokersToCollectArgsFrom)
        {
            ArgsMap.Clear();
            ArgsToCollectCount = invokersToCollectArgsFrom.Length;
            foreach (var invoker in invokersToCollectArgsFrom)
            {
                HandlerMap.Add(invoker, ((sender, e) =>
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var iInvoker = sender as IInvoker;
                    if (iInvoker != null)
                    {
                        ArgsMap[iInvoker] = e;
                    }
                    if (ArgsMap.Keys.Count == ArgsToCollectCount)
                    {
                        Complete();
                    }
                }));
            }
        }

        private void Complete()
        {
            HandlerMap.RemoveAll();
            Completed.Invoke();
        }

        public void Add(IInvoker previous, IChainableInvoker next)
        {
            HandlerMap.Add(previous, ((sender, e) => next.InvokeFrom(e as InvokerArgs)));
        }

        public List<TInvokerArg> GetArgsOfType<TInvokerArg>() where TInvokerArg : class
        {
            return
                ArgsMap.Values.Select(value => value as TInvokerArg)
                    .Where(value => value != null)
                    .ToList();
        }

        public void Start(Invoker invoker, InvokerArgs args = null)
        {
            ArgsMap.Clear();
            invoker.Invoke(args);
        }
    }

    public interface IChainableInvoker : IInvoker
    {
        void InvokeFrom(InvokerArgs args);
    }
}