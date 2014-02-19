namespace MonkeyArms
{
    public abstract class Mediator : IInjectingTarget
    {
        protected InvokerMap InvokerMap = new InvokerMap();

        // ReSharper disable once UnusedParameter.Local
        protected Mediator(IMediatorTarget target)
        {
        }

        public abstract void Register();

        public virtual void Unregister()
        {
            InvokerMap.RemoveAll();
        }

        public int GetInvokerMapHandlerCount(Invoker invoker)
        {
            return InvokerMap.GetInvokerHandlerCount(invoker);
        }
    }

    public interface IMediatorTarget
    {
    }
}