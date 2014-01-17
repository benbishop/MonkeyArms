using System;
using System.Collections.Generic;

namespace MonkeyArms
{
	public class InvokerChain
	{
		public Invoker Failed = new Invoker();

		public Invoker Completed = new Invoker();

		public InvokerChain ()
		{
		}

		public void FailOn(Invoker[] invokersToFailOn)
		{

		}

		public void CollectArgsForInvokers(Invoker[] invokersToCollectArgsFrom)
		{

		}

		public void Add(IChainableInvoker previous, IChainableInvoker next)
		{

		}

		public List<InvokerArgs> GetArgs<TInvokerArg>() where TInvokerArg:class{
			List<InvokerArgs> args = new List<InvokerArgs>();

			return args;
		}
	}

	public interface IChainableInvoker
	{
		void InvokeFrom(Invoker args);
	}

}

